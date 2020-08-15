module HafasLibrary

open System
open Client
open FSharp.Data
open Request
open Response
open Hafas

type Profile =
    | Bvg
    | Cmta
    | Cfl
    | Db
    | DbBusradarNrw
    | Hvv
    | Insa
    | Invg
    | Nahsh
    | Nvv
    | Oebb
    | Pkp
    | Rmv
    | Rsag
    | Saarfahrplan
    | SBahnMunich
    | Sncb
    | Svv
    | Vbb
    | Vbn
    | Vmt
    | Vsn

type JourneySummary =
    { origin: string
      departure: string
      destination: string
      arrival: string }

let defaultLocationsOptions =
    { results = Some 1
      fuzzy = Some true
      stops = None
      addresses = None
      poi = None
      subStops = None
      entrances = None
      linesOfStops = None
      language = Some "de" }

let getProfile (client: Client) =
    async {
        client.SendRequest(serializeProfileRequest ())
        let! response = client.Receive()

        match parseResponse (JsonValue.Parse response) with
        | Some result ->
            let p = parsProfile result
            return Some p
        | _ -> return None
    }
    |> Async.RunSynchronously

let getLocations (client: Client) (name: string) (options: LocationsOptions option) =
    let realoptions =
        match options with
        | Some o -> o
        | None -> defaultLocationsOptions

    async {
        client.SendRequest(serializeLocationsRequest (createLocationParams name realoptions))
        let! response = client.Receive()

        match parseResponse (JsonValue.Parse response) with
        | Some result ->
            let stops = parseStops result
            return Some stops
        | _ -> return None
    }
    |> Async.RunSynchronously

let getIdOfFirstStop (stops: Stop [] option) =
    match stops with
    | Some realstops when realstops.Length > 0 -> Some realstops.[0].id
    | _ -> None

let defaultJourneysOptions =
    { departure = None
      arrival = None
      earlierThan = None
      laterThan = None
      results = None
      via = None
      stopovers = None
      transfers = None
      transferTime = None
      accessibility = None
      bike = None
      products = None
      tickets = None
      polylines = None
      subStops = None
      entrances = None
      remarks = None
      walkingSpeed = None
      startWithWalking = None
      language = None
      scheduledDays = None
      ``when`` = None }

let getJourneys (client: Client) (from: string) (``to``: string) (options: JourneysOptions option) =
    let realoptions =
        match options with
        | Some o -> o
        | None -> defaultJourneysOptions

    async {
        client.SendRequest(serializeJourneysRequest (createJourneysOptions from ``to`` realoptions))
        let! response = client.Receive()

        match parseResponse (JsonValue.Parse response) with
        | Some result ->
            let journeys = parseJourneys result
            return Some journeys
        | _ -> return None
    }
    |> Async.RunSynchronously

let getName (o: U2<Station, Stop>) =
    match o with
    | U2.Case1 station -> station.name
    | U2.Case2 stop -> stop.name

let getJourneySummary (journeys: Journey) =
    if journeys.legs.Length > 0 then
        let legO = journeys.legs.[0]
        let legD = journeys.legs.[journeys.legs.Length - 1]
        match (getName legO.origin), legO.plannedDeparture, (getName legD.destination), legD.plannedArrival with
        | Some o, Some dep, Some d, Some ar ->
            Some
                { origin = o
                  departure = dep
                  destination = d
                  arrival = ar }
        | _ -> None
    else
        None

let startClient (profile: Profile, verbose: bool) =
    // debug params: --inspect-brk
    let client =
        Client
            ("/usr/bin/node",
             "/home/bergmann/projects/hafas-jsonrpc-server/build/index.js "
             + profile.ToString(),
             verbose)

    client.Start()
    client
