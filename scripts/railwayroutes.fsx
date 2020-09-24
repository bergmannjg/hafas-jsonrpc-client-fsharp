#r "../.nuget/packages/fsharp.systemtextjson/0.12.12/lib/netstandard2.0/FSharp.SystemTextJson.dll"
#r "../src/RailwayRouteJsonRpcClient/bin/Debug/netstandard2.1/JsonRpcClient.dll"
#r "../src/RailwayRouteJsonRpcClient/bin/Debug/netstandard2.1/RailwayRouteJsonRpcClient.dll"

open RailwayrouteLibrary
open RailwayRoute

let client =
    startClient
        ({ defaultClientOptions with
               verbose = false })

let maybeRailwayroutes =
    getRailwayroutes
        client
        [| 8000059
           8003288
           8002824
           8000162
           8001118
           8000036
           8000036
           8000149
           8000080
           8000041
           8000098
           8000086
           8000082
           8000085 |]

printfn "%A" (getRailwayRouteNrs maybeRailwayroutes)
