module Response

open Hafas

type Response<'a> =
    { jsonrpc: string
      id: int option
      result: 'a option
      error: string option }

type JourneysResponse = Journeys

type LocationsResponse = array<U3StationStopLocation>

let parseLocationsResponse (response: string) =
    Serializer.Deserialize<Response<LocationsResponse>>(response).result

let parseProfileResponse (response: string) =
    Some(Serializer.Deserialize<Profile>(response))

let parseJourneysResponse (response: string) =
    Serializer.Deserialize<Response<JourneysResponse>>(response).result

let parseTrip (response: string) =
    Serializer.Deserialize<Response<Trip>>(response).result
