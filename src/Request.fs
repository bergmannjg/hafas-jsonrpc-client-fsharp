module Request

open Hafas

type Request<'a> =
    { jsonrpc: string
      id: int option
      method: string
      ``params``: 'a }

let req<'a> (method: string) (p: 'a) =
    { jsonrpc = "2.0"
      id = Some 0
      method = method
      ``params`` = p }

//
// ProfileRequest
//

type ProfileRequest = Request<string>

let serializeProfileRequest () =
    Serializer.Serialize<ProfileRequest>(req<string> "profile" "null")

//
// LocationsRequest
//

type LocationParams =
    { name: string
      options: LocationsOptions }

type LocationRequest = Request<LocationParams>

let serializeLocationsRequest (p: LocationParams) =
    Serializer.Serialize<LocationRequest>(req<LocationParams> "locations" p)

let createLocationParams (name: string) (options: LocationsOptions) = { name = name; options = options }

//
// JourneyRequest
//

type JourneyParams =
    { from: string
      ``to``: string
      options: JourneysOptions }

type JourneyRequest = Request<JourneyParams>

let serializeJourneysRequest (p: JourneyParams) =
    Serializer.Serialize<JourneyRequest>(req<JourneyParams> "journeys" p)

let createJourneyParams (from: string) (``to``: string) (options: JourneysOptions) =
    { from = from
      ``to`` = ``to``
      options = options }

//
// JourneyRequest
//

type TripParams =
    { id: string
      name: string
      options: TripOptions }

type TripRequest = Request<TripParams>

let serializeTripRequest (p: TripParams) =
    Serializer.Serialize<TripRequest>(req<TripParams> "trip" p)

let createTripParams (id: string) (name: string) (options: TripOptions) =
    { id = id
      name = name
      options = options }
