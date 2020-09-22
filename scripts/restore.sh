#!/usr/bin/env bash

if [ ! -d "./scripts" ]; then
    echo "please run from project directory"
    exit 1
fi

rm -rf node_modules

mkdir node_modules

# import hafas-client types
wget -q https://raw.githubusercontent.com/DefinitelyTyped/DefinitelyTyped/master/types/hafas-client/index.d.ts
mkdir node_modules/@types
mkdir node_modules/@types/hafas-client

sed -i '/CreateClient.IExports/d' index.d.ts
mv index.d.ts node_modules/@types/hafas-client/index.d.ts

npx ts2fable node_modules/@types/hafas-client/index.d.ts HafasClientTypes.fs
sed -i '/CreateClient.IExports/d' HafasClientTypes.fs

dotnet run --project src/Transformer/Transformer.fsproj Hafas HafasClientTypes.fs src/HafasJsonRpcClient/Types-Hafas.fs

rm -f HafasClientTypes.fs

# import railwayroute types
wget -q https://raw.githubusercontent.com/bergmannjg/railwaytrip-to-railwayroute/master/src/db-data-railway-routes-types.ts
mkdir node_modules/railwaytrip-to-railwayroute

sed -i 's/export type/export/' db-data-railway-routes-types.ts
mv db-data-railway-routes-types.ts node_modules/railwaytrip-to-railwayroute/db-data-railway-routes-types.ts

npx ts2fable node_modules/railwaytrip-to-railwayroute/db-data-railway-routes-types.ts RailwayRouteTypes.fs

dotnet run --project src/Transformer/Transformer.fsproj  RailwayRoute RailwayRouteTypes.fs src/RailwayRouteJsonRpcClient/Types-RailwayRoute.fs

rm -f RailwayRouteTypes.fs

# rm -rf node_modules

dotnet build --packages ./.nuget/packages src/HafasJsonRpcClient/HafasJsonRpcClient.fsproj

dotnet build --packages ./.nuget/packages src/RailwayRouteJsonRpcClient/RailwayRouteJsonRpcClient.fsproj
