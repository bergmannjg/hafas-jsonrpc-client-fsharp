module RailwayrouteLibrary

open System
open Client
open Request
open Response
open RailwayRoute

let getRailwayroutes (client: Client) (uic_refs: int []) =
    client.SendReceive(serializeRailwayrouteRequest (createRailwayrouteParams uic_refs))
    |> parseRailwayrouteResponse

let getRailwayRouteNrs (maybyRR: RailwayRouteOfTripResult option) =
    match maybyRR with
    | Some rr ->
        rr.railwayRoutes
        |> Array.choose (fun r -> r.railwayRouteNr)
    | _ -> Array.empty

type ClientOptions =
    {
      /// path to node executable
      node: string
      /// path of scriptfile
      script: string
      /// show transfer message
      verbose: bool }

let defaultClientOptions =
    { node = "/usr/bin/node"
      script = "../railwayroute-jsonrpc-server/build/index.js"
      verbose = false }

let startClient (options: ClientOptions) =
    Serializer.addConverters ([||])

    let client =
        Client(options.node, options.script, options.verbose)

    client.Start()
    client
