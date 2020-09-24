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

let kmi_to_kilometer (km_i: float) =
    let x = km_i - 100000000.0
    let d1_meter = Math.Floor(x / 10000.0) * 100.0
    let d2_meter = Math.Floor(x % 100.0)
    (d1_meter + d2_meter) / 1000.0

let getDistance (mayBeFrom: BetriebsstelleRailwayRoutePosition option)
                (mayBeTo: BetriebsstelleRailwayRoutePosition option)
                =
    match mayBeFrom, mayBeTo with
    | Some bsFrom, Some bsTo ->
        if bsTo.KM_I > bsFrom.KM_I then
            kmi_to_kilometer (bsTo.KM_I)
            - kmi_to_kilometer (bsFrom.KM_I)
        else
            kmi_to_kilometer (bsFrom.KM_I)
            - kmi_to_kilometer (bsTo.KM_I)
    | _ -> 0.0

let getRailwayRouteDistance (maybyRR: RailwayRouteOfTripResult option) =
    match maybyRR with
    | Some rr ->
        rr.railwayRoutes
        |> Array.map (fun r ->
            match r.from, r.``to`` with
            | Some bsFrom, Some bsTo -> getDistance bsTo.railwayRoutePosition bsFrom.railwayRoutePosition
            | _ -> 0.0)
        |> Array.fold (+) 0.0
    | _ -> 0.0

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
