#r "../../../.nuget/packages/fsharp.systemtextjson/0.12.12/lib/netstandard2.0/FSharp.SystemTextJson.dll"
#r "../src/bin/Debug/netstandard2.1/HafasJsonRpcClient.dll"

open HafasLibrary
open Hafas

let client =
    startClient
        (Db,
         { defaultClientOptions with
               verbose = false })

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
          results = Some 2 }

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

let maybeTripIdsOfJourneys =
    maybeJourneys
    |> Option.map (fun journeys ->
        journeys
        |> Array.map (fun journey ->
            journey.legs
            |> Array.map (fun leg -> leg.tripId)
            |> Array.choose id))

match maybeTripIdsOfJourneys with
| Some tripIdsOfJourneys ->
    tripIdsOfJourneys
    |> Array.map (fun tripIdsOfJourney ->
        tripIdsOfJourney
        |> Array.iter (fun tripId ->
            let summary =
                getTripSummary (getTrip client tripId "ignored" None)

            printfn "%A" summary))
| _ -> Array.empty

let tripIdsOfFirstJourney =
    match maybeTripIdsOfJourneys with
    | Some tripIdsOfJourneys -> if tripIdsOfJourneys.Length > 0 then tripIdsOfJourneys.[0] else Array.empty
    | None -> Array.empty

let maybeFirstTripId =
    if tripIdsOfFirstJourney.Length > 0 then Some tripIdsOfFirstJourney.[0] else None

let locations =
    match maybeFirstTripId with
    | Some tripId -> getTripLocations (getTrip client tripId "ignored" None)
    | None -> Array.empty

printfn "length: %d" locations.Length

printfn "%A" locations
