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
    Serializer.Deserialize<Response<Profile>>(response).result

let parseJourneysResponse (response: string) =
    try
        Serializer.Deserialize<Response<JourneysResponse>>(response).result
    with ex -> None // todo

let parseTrip (response: string) =
    Serializer.Deserialize<Response<Trip>>(response).result
