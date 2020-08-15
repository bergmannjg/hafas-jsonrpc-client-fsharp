# F# client for the hafas JSON-RPC server

F# client for the [hafas-jsonrpc-server](https://github.com/bergmannjg/hafas-jsonrpc-server).

The node server app is started from the client library.

F# client and node server can run on Linux and Windows.

## Use cases

Example use cases for the client library:

* use with F# Interactive in this [script](./hafas-example.fsx)
* use in a [Jupyter Notebook](https://jupyter.org/) app with this [document](./hafas-example.ipynb).

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





