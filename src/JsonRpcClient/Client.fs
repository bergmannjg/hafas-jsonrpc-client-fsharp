// use module LowLevel and type Client from https://github.com/fsharp/FsAutoComplete/blob/master/src/LanguageServerProtocol/LanguageServerProtocol.fs
module Client

open System
open System.Diagnostics
open System.IO
open System.Text

module private LowLevel =
    let private headerBufferSize = 300
    let private minimumHeaderLength = 21
    let private cr = byte '\r'
    let private lf = byte '\f'
    let private headerEncoding = Encoding.ASCII

    let private readLine (stream: Stream) =
        let buffer = Array.zeroCreate<byte> headerBufferSize
        let readCount = stream.Read(buffer, 0, 2)
        let mutable count = readCount
        if count < 2 then
            None
        else
            // TODO: Check that we don't over-fill headerBufferSize
            while count < headerBufferSize
                  && (buffer.[count - 2]
                      <> cr
                      && buffer.[count - 1] <> lf) do
                let additionalBytesRead = stream.Read(buffer, count, 1)
                // TODO: exit when additionalBytesRead = 0, end of stream
                count <- count + additionalBytesRead

            if count >= headerBufferSize
            then None
            else Some(headerEncoding.GetString(buffer, 0, count - 2))

    let rec private readHeaders (stream: Stream) =
        let line = readLine stream
        match line with
        | Some "" -> []
        | Some line ->
            let separatorPos = line.IndexOf(": ")
            if separatorPos = -1 then
                raise (Exception(sprintf "Separator not found in header '%s'" line))
            else
                let name = line.Substring(0, separatorPos)
                let value = line.Substring(separatorPos + 2)
                let otherHeaders = readHeaders stream
                (name, value) :: otherHeaders
        | None -> raise (EndOfStreamException())

    let read (stream: Stream) =
        let headers = readHeaders stream

        let contentLength =
            headers
            |> List.tryFind (fun (name, _) -> name = "Content-Length")
            |> Option.map snd
            |> Option.bind (fun s ->
                match Int32.TryParse(s) with
                | true, x -> Some x
                | _ -> None)

        if contentLength = None then
            failwithf "Content-Length header not found"
        else
            let result =
                Array.zeroCreate<byte> contentLength.Value

            let mutable readCount = 0
            while readCount < contentLength.Value do
                let toRead = contentLength.Value - readCount
                let readInCurrentBatch = stream.Read(result, readCount, toRead)
                readCount <- readCount + readInCurrentBatch

            let str =
                Encoding.UTF8.GetString(result, 0, readCount)

            headers, str

    let write (stream: Stream) (data: string) =
        let bytes = Encoding.UTF8.GetBytes(data)

        let header =
            sprintf "Content-Length: %d\r\n\r\n" bytes.Length

        let headerBytes = Encoding.ASCII.GetBytes header

        use ms =
            new MemoryStream(headerBytes.Length + bytes.Length)

        ms.Write(headerBytes, 0, headerBytes.Length)
        ms.Write(bytes, 0, bytes.Length)
        stream.Write(ms.ToArray(), 0, int ms.Position)

type Client(exec: string, args: string, verbose: bool) =

    let mutable inputStream: StreamWriter option = None
    let mutable outuptStream: StreamReader option = None
    let verbose = verbose

    member __.SendRequest(reqString: string) =
        inputStream
        |> Option.iter (fun input ->
            if verbose
            then fprintfn stderr "[CLIENT] Writing: %s" reqString
            LowLevel.write input.BaseStream reqString
            input.BaseStream.Flush())

    member __.Receive(): Async<string> =
        async {
            let response =
                match outuptStream with
                | Some sr ->
                    let outStream = sr.BaseStream
                    try
                        let headers, notificationString = LowLevel.read outStream
                        if verbose
                        then 
                            fprintfn stderr "[CLIENT] READING: %s" (headers.ToString())
                            fprintfn stderr "[CLIENT] READING: %s" notificationString
                        notificationString
                    with
                    | ex ->
                        fprintfn stderr "[CLIENT] Ex: %s" (ex.ToString()) 
                        ""
                | _ -> ""

            return response
        }

    member __.SendReceive(request: string) =
        async {
            __.SendRequest(request)
            return! __.Receive()
        }
        |> Async.RunSynchronously

    member __.Start() =

        let si = ProcessStartInfo()
        si.RedirectStandardOutput <- true
        si.RedirectStandardInput <- true
        si.RedirectStandardError <- true
        si.UseShellExecute <- false
        si.WorkingDirectory <- Environment.CurrentDirectory
        si.FileName <- exec
        si.Arguments <- args

        let proc =
            try
                Process.Start(si)
            with ex ->
                let newEx =
                    System.Exception(sprintf "%s on %s" ex.Message exec, ex)

                raise newEx

        inputStream <- Some(proc.StandardInput)
        outuptStream <- Some(proc.StandardOutput)

        if verbose then
            async {
                let errorStream = proc.StandardError.BaseStream
                let buffer = Array.zeroCreate<byte> 1024
                let mutable quit = false
                while not quit do
                    try
                        let readCount = errorStream.Read(buffer, 0, 1024)

                        let str =
                            Encoding.UTF8.GetString(buffer, 0, readCount)

                        fprintfn stderr "[SERVER] DEBUGMSG: %s" str
                    with
                    | :? EndOfStreamException -> quit <- true
                    | ex -> ()

                return ()
            }
            |> Async.Start
