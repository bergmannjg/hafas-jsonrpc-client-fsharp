module rec Response

open Hafas
open System.Text.Json
open System.Text.Json.Serialization

type Response<'a> =
    { jsonrpc: string
      id: int option
      result: 'a option
      error: string option }

type JourneysResponse = Journeys

type LocationsResponse = array<U3StationStopLocation>

let options = JsonSerializerOptions()

options.Converters.Add
    (JsonFSharpConverter
        (JsonUnionEncoding.InternalTag
         ||| JsonUnionEncoding.UnwrapRecordCases
         ||| JsonUnionEncoding.UnwrapOption,
         unionTagName = "type",
         unionTagCaseInsensitive = true))

let parseLocationsResponse (response: string) =
    JsonSerializer.Deserialize<Response<LocationsResponse>>(response, options).result

let parseProfileResponse (response: string) =
    Some(JsonSerializer.Deserialize<Profile>(response, options))

let parseJourneysResponse (response: string) =
    JsonSerializer.Deserialize<Response<JourneysResponse>>(response, options).result

let parseTrip (response: string) =
    JsonSerializer.Deserialize<Response<Trip>>(response, options).result
