FROM node:current-alpine3.12

RUN apk add git bash wget

WORKDIR /usr/src/apps

RUN git clone https://github.com/bergmannjg/hafas-jsonrpc-server.git

WORKDIR /usr/src/apps/hafas-jsonrpc-server

RUN npm install

RUN npx tsc

WORKDIR /usr/src/apps

RUN wget https://dot.net/v1/dotnet-install.sh

RUN chmod +x dotnet-install.sh

RUN ./dotnet-install.sh -c Current

ENV PATH /root/.dotnet:$PATH

RUN apk add --no-cache icu-libs
RUN apk add --no-cache libintl

WORKDIR /usr/src/apps/hafas-jsonrpc-client-fsharp

COPY ./src src/

RUN dotnet build --packages ./.nuget/packages src/HafasJsonRpcClient.fsproj 

COPY ./scripts/journeys.docker.fsx scripts/

ENTRYPOINT  ["dotnet", "fsi", "scripts/journeys.docker.fsx"]

