module HafasLibrary

open System
open Client
open Request
open Response
open Hafas
open BRouter

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

type TripSummary =
    { line: string option
      origin: string
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
    client.SendReceive(serializeProfileRequest ())
    |> parseProfileResponse

let getProducts (maybeProfile: Hafas.Profile option, mode: ProductTypeMode) =
    match maybeProfile with
    | Some profile ->
        profile.products
        |> Array.fold (fun (m: Map<string, bool>) p -> m.Add(p.id, p.mode = mode)) Map.empty<string, bool>
    | None -> Map.empty<string, bool>

let getLocations (client: Client) (name: string) (options: LocationsOptions option) =
    let realoptions =
        options
        |> Option.defaultValue defaultLocationsOptions

    client.SendReceive(serializeLocationsRequest (createLocationParams name realoptions))
    |> parseLocationsResponse

let getIdOfFirstStop (response: LocationsResponse option) =
    match response with
    | Some realstops when realstops.Length > 0 ->
        match realstops.[0] with
        | Station station -> station.id
        | Stop stop when stop.id.IsSome -> Some stop.id.Value
        | Location location -> location.id
        | _ -> None
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

    client.SendReceive(serializeJourneysRequest (createJourneyParams from ``to`` realoptions))
    |> parseJourneysResponse

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

let stopover2location (stopover: StopOver) =
    match stopover.stop with
    | U2StationStop.Station s -> s.location
    | U2StationStop.Stop s -> s.location

let stopover2id (stopover: StopOver) =
    match stopover.stop with
    | U2StationStop.Station s -> s.id
    | U2StationStop.Stop s when s.id.IsSome -> Some s.id.Value
    | _ -> None

let location2lonlat (loc: Location) =
    match loc.longitude, loc.latitude with
    | Some longitude, Some latitude ->
        Some
            { longitude = longitude
              latitude = latitude }
    | _, _ -> None

let getJourneyIds (journey: Journey) =
    journey.legs
    |> Array.collect (fun leg ->
        match leg.stopovers with
        | Some stopovers -> stopovers |> Array.choose stopover2id
        | None -> Array.empty)

let getJourneyLocations (journey: Journey) =
    journey.legs
    |> Array.collect (fun leg ->
        match leg.stopovers with
        | Some stopovers ->
            stopovers
            |> Array.choose stopover2location
            |> Array.choose location2lonlat
        | None -> Array.empty)

let defaultTripOptions =
    { stopovers = Some true
      polyline = None
      subStops = None
      entrances = None
      remarks = None
      language = Some "de" }

let getTrip (client: Client) (id: string) (name: string) (options: TripOptions option) =
    let realoptions =
        options |> Option.defaultValue defaultTripOptions

    client.SendReceive(serializeTripRequest (createTripParams id name realoptions))
    |> parseTrip

let getTripSummary (maybeTrip: Trip option) =
    maybeTrip
    |> Option.map (fun trip ->
        match (getName trip.origin), trip.plannedDeparture, (getName trip.destination), trip.plannedArrival, trip.line with
        | Some o, Some dep, Some d, Some ar, Some line ->
            Some
                { line = line.name
                  origin = o
                  departure = dep
                  destination = d
                  arrival = ar }
        | _ -> None)
    |> Option.flatten

let getTripLocations (maybeTrip: Trip option) =
    match maybeTrip with
    | Some trip ->
        match trip.stopovers with
        | Some stopovers ->
            stopovers
            |> Array.choose stopover2location
            |> Array.choose location2lonlat
        | None -> Array.empty
    | None -> Array.empty

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
    Serializer.addConverters ([| Serializer.UnionConverter<ProductTypeMode>() |])

    let client =
        Client(options.node, options.script + " " + profile.ToString(), options.verbose)

    client.Start()
    client
