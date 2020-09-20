#!/usr/bin/env bash

if [ ! -d "./scripts" ]; then
    echo "please run from project directory"
    exit 1
fi

wget -q https://raw.githubusercontent.com/bergmannjg/hafas-client-fable/master/src/HafasClientTypes.fs

dotnet run --project src/transformer/Transformer.fsproj HafasClientTypes.fs src/Types-Hafas.fs

rm -f HafasClientTypes.fs

dotnet build --packages ./.nuget/packages src/HafasJsonRpcClient.fsproj

