module rec Response

open System
open FSharp.Reflection
open FSharp.Data
open Hafas

type Response =
    { jsonrpc: string
      id: int option
      result: JsonValue option
      error: JsonValue option }

let private parseProductTypeModeFromString (s: string): ProductTypeMode =
    match FSharpType.GetUnionCases typedefof<ProductTypeMode>
          |> Array.filter (fun case -> String.Equals(case.Name, s, StringComparison.CurrentCultureIgnoreCase)) with
    | [| case |] -> FSharpValue.MakeUnion(case, [||]) :?> ProductTypeMode
    | _ -> raise (Exception(sprintf "%s is not a known ProductTypeMode" s))

let private parseLocation =
    LSP.Json.Ser.deserializerFactory<Location> defaultJsonReadOptions

let private parseStop =
    LSP.Json.Ser.deserializerFactory<Stop> defaultJsonReadOptions

let private parseStation =
    LSP.Json.Ser.deserializerFactory<Station> defaultJsonReadOptions

let private getTypeProperty (v: JsonValue): string option =
    match v with
    | JsonValue.Record properties when properties.Length > 0 ->
        match properties.[0] with
        | "type", JsonValue.String s -> Some s
        | _ -> None
    | _ -> None

let private parseU2StationStopFromJsonValue (v: JsonValue): U2<Station, Stop> =
    match getTypeProperty v with
    | Some "station" -> U2.Case1(parseStation v)
    | Some "stop" -> U2.Case2(parseStop v)
    | _ -> raise (Exception(sprintf "Don't know how to deserialize %A from JSON" v))

let private parseU3StationStopObjFromJsonValue (v: JsonValue): U3<Station, Stop, obj> =
    match getTypeProperty v with
    | Some "station" -> U3.Case1(parseStation v)
    | Some "stop" -> U3.Case2(parseStop v)
    | _ -> raise (Exception(sprintf "Don't know how to deserialize %A from JSON" v))

let private parseU3StationStopLocationFromJsonValue (v: JsonValue): U3<Station, Stop, Location> =
    match getTypeProperty v with
    | Some "station" -> U3.Case1(parseStation v)
    | Some "stop" -> U3.Case2(parseStop v)
    | Some "location" -> U3.Case3(parseLocation v)
    | _ -> raise (Exception(sprintf "Don't know how to deserialize %A from JSON" v))

let private defaultJsonReadOptions: LSP.Json.Ser.JsonReadOptions =
    { customReaders =
          [ parseProductTypeModeFromString
            parseU2StationStopFromJsonValue
            parseU3StationStopObjFromJsonValue
            parseU3StationStopLocationFromJsonValue ] }

let parseStops =
    LSP.Json.Ser.deserializerFactory<Stop []> defaultJsonReadOptions

let parseProfile =
    LSP.Json.Ser.deserializerFactory<Profile> defaultJsonReadOptions

let parseJourneys =
    LSP.Json.Ser.deserializerFactory<Journeys> defaultJsonReadOptions

let parseTrip =
    LSP.Json.Ser.deserializerFactory<Trip> defaultJsonReadOptions

let parseResponse (v: JsonValue) =
    try
        let deserializer =
            LSP.Json.Ser.deserializerFactory<Response> defaultJsonReadOptions

        let rawResponse = deserializer v
        rawResponse.result
    with :? Exception as ex ->
        printfn "%s" ex.Message
        printfn "%s" ex.InnerException.Message
        None

let rec dumpJsonValue (v: JsonValue) =
    let w = new System.IO.StringWriter()
    v.WriteTo(w, JsonSaveOptions.None)
    w.ToString()
