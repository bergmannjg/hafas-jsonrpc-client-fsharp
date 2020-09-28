module Response

open Hafas

type Error = { code: int; message: string }

type Response<'a> =
    { jsonrpc: string
      id: int option
      result: 'a option
      error: Error option }

let parseResponse<'a> (response: string) =
    try
        let resp =
            Serializer.Deserialize<Response<'a>>(response)

        match resp.result, resp.error with
        | Some result, _ -> Some result
        | _, Some error ->
            fprintfn stderr "error: %s" error.message
            None
        | _, _ -> None
    with ex -> None // todo

let parseProfileResponse (response: string) = parseResponse<Profile> (response)

type LocationsResponse = array<U3StationStopLocation>

let parseLocationsResponse (response: string) =
    parseResponse<LocationsResponse> (response)

type JourneysResponse = Journeys

let parseJourneysResponse (response: string) =
    parseResponse<JourneysResponse> (response)

let parseTrip (response: string) = parseResponse<Trip> (response)
