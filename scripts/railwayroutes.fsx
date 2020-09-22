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
    getRailwayroutes client [| 8007768; 8001768; 8000316 |]

printfn "%A" (getRailwayRouteNrs maybeRailwayroutes)
