module Request

open Hafas

type Request<'a> =
    { jsonrpc: string
      id: int option
      method: string
      ``params``: 'a }

type LocationParams =
    { name: string
      options: LocationsOptions }

type LocationRequest = Request<LocationParams>

type ProfileRequest = Request<string>

let private writeProducts (v: Products option): string option = None

let private defaultJsonWriteOptions: LSP.Json.Ser.JsonWriteOptions =
    { customWriters = [ writeProducts ]
      filterNoneValue = true }

let serializeProfileRequest () =
    let req =
        { jsonrpc = "2.0"
          id = Some 0
          method = "profile"
          ``params`` = "null" }

    let serializer =
        LSP.Json.Ser.serializerFactory<ProfileRequest> defaultJsonWriteOptions

    serializer req

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

let createJourneysOptions (from: string) (``to``: string) (options: JourneysOptions) =
    { from = from
      ``to`` = ``to``
      options = options }
