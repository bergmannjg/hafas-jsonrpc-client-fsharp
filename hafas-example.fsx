#r "../../.nuget/packages/fsharp.data/3.3.3/lib/netstandard2.0/FSharp.Data.dll"
#r "./src/bin/Debug/netstandard2.1/HafasJsonRpcClient.dll"

open HafasLibrary
open Hafas

let client =
    startClient
        (HafasLibrary.Profile.Db,
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

let maybeTripIds =
    maybeJourneys
    |> Option.map (fun journeys ->
        journeys
        |> Array.collect (fun journey -> journey.legs |> Array.map (fun j -> j.tripId)))

let tripSummaries =
    match maybeTripIds with
    | Some tripIds ->
        tripIds
        |> Array.map (fun maybeTripId ->
            let trip =
                match maybeTripId with
                | Some tripId -> getTrip client tripId "ignored" None
                | _ -> None

            (getTripSummary trip))
        |> Array.choose id
    | _ -> Array.empty

printfn "%A" tripSummaries
