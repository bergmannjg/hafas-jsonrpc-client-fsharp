#r "../../.nuget/packages/fsharp.data/3.3.3/lib/netstandard2.0/FSharp.Data.dll"
#r "./src/bin/Debug/netstandard2.1/HafasJsonRpcClient.dll"

open HafasLibrary
open Hafas

let client =
    startClient (HafasLibrary.Profile.Db, defaultClientOptions)

(*
let p = getProfile(client)
printfn "%A" p
*)

let idBielefeld =
    getIdOfFirstStop (getLocations client "Bielefeld" None)

let idBerlin =
    getIdOfFirstStop (getLocations client "Berlin" None)

let maybeJourneys =
    match idBielefeld, idBerlin with
    | Some from, Some ``to`` ->
        match (getJourneys client from ``to`` None) with
        | Some result -> result.journeys
        | _ -> None
    | _ -> None

match maybeJourneys with
| Some journeys ->
    journeys
    |> Array.iter (getJourneySummary >> (printfn "%A"))
| None -> ()
