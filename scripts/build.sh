#!/usr/bin/env bash

if [ ! -d "./scripts" ]; then
    echo "please run from project directory"
    exit 1
fi

dotnet build --packages ./.nuget/packages src/HafasJsonRpcClient/HafasJsonRpcClient.fsproj

dotnet build --packages ./.nuget/packages src/RailwayRouteJsonRpcClient/RailwayRouteJsonRpcClient.fsproj
