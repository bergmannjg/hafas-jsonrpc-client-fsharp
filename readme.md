# F# clients for JSON-RPC server

F# clients for the node server apps

* [hafas-jsonrpc-server](https://github.com/bergmannjg/hafas-jsonrpc-server).
* [railwayroute-jsonrpc-server](https://github.com/bergmannjg/railwayroute-jsonrpc-server).

The node server app is started from the client library.

F# client and node server can run on Linux and Windows.

## Use cases

Example use cases for the client library:

* with F# Interactive in this [script](./scripts/journeys.fsx)
* in a [Jupyter Notebook](https://jupyter.org/) app with this [document](./notebooks/hafas-example.ipynb) using the new [.net-fsharp kernel](https://github.com/dotnet/interactive).

## Configuration of node server app

The server is configured via the following options

```
type ClientOptions =
    {
      /// path to node executable, default is "/usr/bin/node"
      node: string
      /// path of scriptfile, default is "../hafas-jsonrpc-server/build/index.js"
      script: string
      /// show transfer messages, default is false
      verbose: bool }
```

## (De)Serialization of F# types

The (de)serialization of F# types from/to JsonRpc messages is done with the following steps:

* byte -> string
  * uses [LowLevel](https://github.com/fsharp/FsAutoComplete/blob/2346b3e2f3dcfbfdb14381484879514d6f43f1f0/src/LanguageServerProtocol/LanguageServerProtocol.fs#L1691) and [Client](https://github.com/fsharp/FsAutoComplete/blob/2346b3e2f3dcfbfdb14381484879514d6f43f1f0/src/LanguageServerProtocol/LanguageServerProtocol.fs#L2418) from [FsAutoComplete](https://github.com/fsharp/FsAutoComplete/blob/master/src/LanguageServerProtocol/LanguageServerProtocol.fs)
* string -> F# type
  * uses [FSharp.SystemTextJson](https://github.com/Tarmil/FSharp.SystemTextJson)

## F# type defintions from TypeScript declaration file

The types are generated from 

* the TypeScript declaration file [hafas-client](https://github.com/DefinitelyTyped/DefinitelyTyped/blob/master/types/hafas-client/index.d.ts) 
* the TypeScript file [railway-routes-types](https://github.com/bergmannjg/railwaytrip-to-railwayroute/blob/master/src/db-data-railway-routes-types.ts
)

with the following steps (see [restore.sh](./scripts/restore.sh)):

* F# interface types with [ts2fable](https://github.com/fable-compiler/ts2fable)
* F# interface types to record types with [transformer](./src/transformer).



