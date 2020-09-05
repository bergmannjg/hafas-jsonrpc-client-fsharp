#r "../../../.nuget/packages/fsharp.systemtextjson/0.12.12/lib/netstandard2.0/FSharp.SystemTextJson.dll"
#r "../src/bin/Debug/netstandard2.1/HafasJsonRpcClient.dll"

open HafasLibrary
open Hafas
open BRouter

let client =
    startClient
        (Db,
         { defaultClientOptions with
               verbose = true })

(*
let p = getProfile(client)
printfn "%A" p
*)

let maybeFrom =
    getIdOfFirstStop (getLocations client "Bielefeld" None)

let maybeTo =
    getIdOfFirstStop (getLocations client "Berlin" None)

let journeysOptions =
    { defaultJourneysOptions with
          results = Some 1
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

let maybeFirstJourney =
    match maybeJourneys with
    | Some journeys -> if journeys.Length > 0 then Some journeys.[0] else None
    | _ -> None

let journeyLocations =
    match maybeFirstJourney with
    | Some journey -> getJourneyLocations journey
    | None -> Array.empty

printfn "%A" journeyLocations

let uri = getUri journeyLocations

printfn "%s" uri