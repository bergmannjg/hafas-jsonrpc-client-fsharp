# F# client for the hafas JSON-RPC server

F# client for the [hafas-jsonrpc-server](https://github.com/bergmannjg/hafas-jsonrpc-server).

The node server app is started from the client library.

F# client and node server can run on Linux and Windows.

## Use cases

Example use cases for the client library:

* with F# Interactive in this [script](./hafas-example.fsx)
* in a [Jupyter Notebook](https://jupyter.org/) app with this [document](./hafas-example.ipynb) using the new [.net-fsharp kernel](https://github.com/dotnet/interactive).

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

The module Hafas contains the F# types corresponding to the the hafas-jsonrpc-server api.

The types are generated from the TypeScript declaration file [hafas-client](https://github.com/DefinitelyTyped/DefinitelyTyped/blob/master/types/hafas-client/index.d.ts) with the following steps:

* F# interface types with [ts2fable](https://github.com/fable-compiler/ts2fable)
* F# interface types to record types with the script [interface2record](./scripts/interface2record.sh).



