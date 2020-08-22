module HafasLibrary

open System
open Client
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

        return parseProfileResponse response
    }
    |> Async.RunSynchronously

let getLocations (client: Client) (name: string) (options: LocationsOptions option) =
    let realoptions =
        options
        |> Option.defaultValue defaultLocationsOptions

    async {
        client.SendRequest(serializeLocationsRequest (createLocationParams name realoptions))
        let! response = client.Receive()

        return parseLocationsResponse response
    }
    |> Async.RunSynchronously

let getIdOfFirstStop (response: LocationsResponse option) =
    match response with
    | Some realstops when realstops.Length > 0 ->
        match realstops.[0] with
        | Station station -> station.id
        | Stop stop -> Some stop.id
        | Location location -> location.id
    | _ -> None

let defaultJourneysOptions =
    { departure = None
      arrival = None
      earlierThan = None
      laterThan = None
      results = Some 3
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
        options
        |> Option.defaultValue defaultJourneysOptions

    async {
        client.SendRequest(serializeJourneysRequest (createJourneyParams from ``to`` realoptions))
        let! response = client.Receive()

        return parseJourneysResponse response
    }
    |> Async.RunSynchronously

let getName (o: U2StationStop) =
    match o with
    | U2StationStop.Station station -> station.name
    | U2StationStop.Stop stop -> stop.name

let getJourneySummary (journey: Journey) =
    if journey.legs.Length > 0 then
        let legO = journey.legs.[0]
        let legD = journey.legs.[journey.legs.Length - 1]
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

let defaultTripOptions =
    { stopovers = None
      polyline = None
      subStops = None
      entrances = None
      remarks = None
      language = Some "de" }

let getTrip (client: Client) (id: string) (name: string) (options: TripOptions option) =
    let realoptions =
        options |> Option.defaultValue defaultTripOptions

    async {
        client.SendRequest(serializeTripRequest (createTripParams id name realoptions))
        let! response = client.Receive()

        return parseTrip response
    }
    |> Async.RunSynchronously

let getTripSummary (maybeTrip: Trip option) =
    maybeTrip
    |> Option.map (fun trip ->
        match (getName trip.origin), trip.plannedDeparture, (getName trip.destination), trip.plannedArrival with
        | Some o, Some dep, Some d, Some ar ->
            Some
                { origin = o
                  departure = dep
                  destination = d
                  arrival = ar }
        | _ -> None)
    |> Option.flatten


type ClientOptions =
    {
      /// path to node executable
      node: string
      /// path of scriptfile
      script: string
      /// show transfer message
      verbose: bool }

let defaultClientOptions =
    { node = "/usr/bin/node"
      script = "../hafas-jsonrpc-server/build/index.js"
      verbose = false }

let startClient (profile: Profile, options: ClientOptions) =
    let client =
        Client(options.node, options.script + " " + profile.ToString(), options.verbose)

    client.Start()
    client
