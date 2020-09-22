module Response

type Response<'a> =
    { jsonrpc: string
      id: int option
      result: 'a option
      error: string option }

type RailwayrouteResponse = RailwayRoute.RailwayRouteOfTripResult

let parseRailwayrouteResponse (response: string) =
    Serializer.Deserialize<Response<RailwayrouteResponse>>(response).result

