module Request

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
// RailwayrouteRequest
//

type RailwayrouteParams = { uic_refs: int [] }

type RailwayrouteRequest = Request<RailwayrouteParams>

let serializeRailwayrouteRequest (p: RailwayrouteParams) =
    Serializer.Serialize<RailwayrouteRequest>(req<RailwayrouteParams> "findRailwayRoutesOfTrip" p)

let createRailwayrouteParams (uic_refs: int []) = { uic_refs = uic_refs }

