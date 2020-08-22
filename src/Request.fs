module Request

open Hafas
open System.Text.Json
open System.Text.Json.Serialization

type Request<'a> =
    { jsonrpc: string
      id: int option
      method: string
      ``params``: 'a }

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

    let jsonOptions =
        JsonSerializerOptions(IgnoreNullValues = true)

    jsonOptions.Converters.Add(JsonFSharpConverter())
    JsonSerializer.Serialize<ProfileRequest>(req, jsonOptions)

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

    let jsonOptions =
        JsonSerializerOptions(IgnoreNullValues = true)

    jsonOptions.Converters.Add(JsonFSharpConverter())
    JsonSerializer.Serialize<LocationRequest>(req, jsonOptions)

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

    let jsonOptions =
        JsonSerializerOptions(IgnoreNullValues = true)

    jsonOptions.Converters.Add(JsonFSharpConverter())
    JsonSerializer.Serialize<JourneyRequest>(req, jsonOptions)

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

    let jsonOptions =
        JsonSerializerOptions(IgnoreNullValues = true)

    jsonOptions.Converters.Add(JsonFSharpConverter())
    JsonSerializer.Serialize<TripRequest>(req, jsonOptions)

let createTripParams (id: string) (name: string) (options: TripOptions) =
    { id = id
      name = name
      options = options }
