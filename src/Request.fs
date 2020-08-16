module Request

open Hafas

type Request<'a> =
    { jsonrpc: string
      id: int option
      method: string
      ``params``: 'a }

let private writeProducts (v: Products option): string option = None

let private defaultJsonWriteOptions: LSP.Json.Ser.JsonWriteOptions =
    { customWriters = [ writeProducts ]
      filterNoneValue = true }

//
// ProfileRequest
//

type ProfileRequest = Request<string>

let serializeProfileRequest () =
    let req =
        { jsonrpc = "2.0"
          id = Some 0
          method = "profile"
          ``params`` = "null" }

    let serializer =
        LSP.Json.Ser.serializerFactory<ProfileRequest> defaultJsonWriteOptions

    serializer req

//
// LocationsRequest
//

type LocationParams =
    { name: string
      options: LocationsOptions }

type LocationRequest = Request<LocationParams>

let serializeLocationsRequest (p: LocationParams) =
    let req =
        { jsonrpc = "2.0"
          id = Some 0
          method = "locations"
          ``params`` = p }

    let serializer =
        LSP.Json.Ser.serializerFactory<LocationRequest> defaultJsonWriteOptions

    serializer req

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
    let req =
        { jsonrpc = "2.0"
          id = Some 0
          method = "journeys"
          ``params`` = p }

    let serializer =
        LSP.Json.Ser.serializerFactory<JourneyRequest> defaultJsonWriteOptions

    serializer req

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
    let req =
        { jsonrpc = "2.0"
          id = Some 0
          method = "trip"
          ``params`` = p }

    let serializer =
        LSP.Json.Ser.serializerFactory<TripRequest> defaultJsonWriteOptions

    serializer req

let createTripParams (id: string) (name: string) (options: TripOptions) =
    { id = id
      name = name
      options = options }
