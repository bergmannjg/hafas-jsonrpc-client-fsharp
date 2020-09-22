#r "../.nuget/packages/fsharp.systemtextjson/0.12.12/lib/netstandard2.0/FSharp.SystemTextJson.dll"
#r "../src/JsonRpcClient/bin/Debug/netstandard2.1/JsonRpcClient.dll"
#r "../src/HafasJsonRpcClient/bin/Debug/netstandard2.1/HafasJsonRpcClient.dll"

open HafasLibrary
open Hafas

let client =
    startClient
        (Db,
         { defaultClientOptions with
               node = "/usr/local/bin/node"
               verbose = false })

(*
let p = getProfile(client)
printfn "%A" p
*)

let maybeFrom =
    getIdOfFirstStop (getLocations client "Köln" None)

let maybeTo =
    getIdOfFirstStop (getLocations client "Berlin" None)

let journeysOptions =
    { defaultJourneysOptions with
          results = Some 2
          stopovers = Some true }

let maybeJourneys =
    match maybeFrom, maybeTo with
    | Some from, Some ``to`` ->
        match (getJourneys client from ``to`` (Some journeysOptions)) with
        | Some result -> result.journeys
        | _ -> None
    | _ -> None

let journeySummaries =
    match maybeJourneys with
    | Some journeys ->
        journeys
        |> Array.map getJourneySummary
        |> Array.choose id
    | _ -> Array.empty

printfn "%A" journeySummaries
