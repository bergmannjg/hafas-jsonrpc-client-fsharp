// analyze journeydata from the calendar export of DB Navigator app

#r "../.nuget/packages/fsharp.systemtextjson/0.12.12/lib/netstandard2.0/FSharp.SystemTextJson.dll"
#r "../.nuget/packages/fsharp.data/3.3.3/lib/netstandard2.0/FSharp.Data.dll"
#r "../src/JsonRpcClient/bin/Debug/netstandard2.1/JsonRpcClient.dll"
#r "../src/HafasJsonRpcClient/bin/Debug/netstandard2.1/HafasJsonRpcClient.dll"
#r "../src/RailwayRouteJsonRpcClient/bin/Debug/netstandard2.1/RailwayRouteJsonRpcClient.dll"

if fsi.CommandLineArgs.Length <> 2 then failwith "filename expected"

let hafasClient =
    HafasLibrary.startClient
        (HafasLibrary.Db,
         { HafasLibrary.defaultClientOptions with
               node = "/usr/local/bin/node"
               verbose = false })

let p = HafasLibrary.getProfile (hafasClient)

let railwayClient =
    RailwayrouteLibrary.startClient
        { RailwayrouteLibrary.defaultClientOptions with
              node = "/usr/local/bin/node"
              verbose = false }

let train =
    HafasLibrary.getProducts (p, Hafas.ProductTypeMode.Train)

let journeysOptions =
    { HafasLibrary.defaultJourneysOptions with
          results = Some 1
          products = Some(train)
          stopovers = Some true }

type Journeydata =
    { von: string
      nach: string
      start: System.DateTime
      ende: System.DateTime }

type JourneydataEx =
    { von: string
      nach: string
      start: System.DateTime
      ende: System.DateTime
      kilometer: float }

let csvjourneydata =
    FSharp.Data.CsvFile.Load(fsi.CommandLineArgs.[1]).Cache()

// the column subject is like 'Fahrt von Berlin Hbf (tief) nach Köln Hbf; keine Umstiege'
let pattern = @"^Fahrt von (.+) nach ([^;]+)"
let year = 2020

let journeydata =
    csvjourneydata.Rows
    |> Seq.map (fun row ->
        let subject = row.GetColumn "Subject"
        let strStartDate = row.GetColumn "Start Date"
        let strStartTime = row.GetColumn "Start Time"
        let strEndDate = row.GetColumn "End Date"
        let strEndTime = row.GetColumn "End Time"

        let m =
            System.Text.RegularExpressions.Regex.Match
                (subject, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase)

        let startdate =
            System.DateTime.ParseExact(strStartDate + " " + strStartTime, "MM/dd/yy h:mm:ss tt", null)

        let enddate =
            System.DateTime.ParseExact(strEndDate + " " + strEndTime, "MM/dd/yy h:mm:ss tt", null)

        if m.Groups.Count = 3 && startdate.Year = year then
            Some
                { von = m.Groups.[1].ToString()
                  nach = m.Groups.[2].ToString()
                  start = startdate
                  ende = enddate }
        else
            None)
    |> Seq.choose id
    |> Seq.toArray

let journeydataEx =
    journeydata
    |> Array.map (fun reise ->
        let maybeFrom =
            HafasLibrary.getIdOfFirstStop (HafasLibrary.getLocations hafasClient reise.von None)

        let maybeTo =
            HafasLibrary.getIdOfFirstStop (HafasLibrary.getLocations hafasClient reise.nach None)

        let maybeJourneys =
            match maybeFrom, maybeTo with
            | Some from, Some ``to`` ->
                match (HafasLibrary.getJourneys hafasClient from ``to`` (Some journeysOptions)) with
                | Some result -> result.journeys
                | _ -> None
            | _ -> None

        let journeyIds =
            match maybeJourneys with
            | Some journeys when journeys.Length > 0 ->
                journeys.[0]
                |> HafasLibrary.getJourneyIds
                |> Array.map int
            | _ -> Array.empty

        let rr =
            RailwayrouteLibrary.getRailwayroutes railwayClient journeyIds

        let distance =
            RailwayrouteLibrary.getRailwayRouteDistance rr

        printfn
            "von: %s, nach: %s, start: %s, kilometer: %.0f"
            reise.von
            reise.nach
            (reise.start.ToShortDateString())
            distance

        { von = reise.von
          nach = reise.nach
          start = reise.start
          ende = reise.ende
          kilometer = distance })

let distance =
    journeydataEx
    |> Array.fold (fun s j -> j.kilometer + s) 0.0

printfn "kilometer: %.3f" distance

let minutes =
    journeydataEx
    |> Array.fold (fun s j ->
        let span = j.ende - j.start
        s + span.TotalMinutes) 0.0

printfn "hours: %.0f" (minutes / 60.0)
