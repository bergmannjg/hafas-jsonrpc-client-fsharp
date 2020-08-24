module rec Response

open Hafas
open System.Text.Json
open System.Text.Json.Serialization
open System
open Microsoft.FSharp.Reflection

type Response<'a> =
    { jsonrpc: string
      id: int option
      result: 'a option
      error: string option }

type JourneysResponse = Journeys

type LocationsResponse = array<U3StationStopLocation>

type ProductTypeModeValueConverter() =
    inherit JsonConverter<ProductTypeMode>()
 
    static member fromString<'a>(s: string) =
        match FSharpType.GetUnionCases typeof<'a>
              |> Array.filter (fun case -> String.Compare(case.Name, s, StringComparison.OrdinalIgnoreCase) = 0) with
        | [| case |] -> Some(FSharpValue.MakeUnion(case, [||]) :?> 'a)
        | _ -> None

    override this.Read(reader: byref<Utf8JsonReader>, _typ: Type, options: JsonSerializerOptions) =
        let s =
            JsonSerializer.Deserialize<string>(&reader, options)

        match ProductTypeModeValueConverter.fromString<ProductTypeMode> s with
        | Some v -> v
        | None -> ProductTypeMode.Train // todo

    override this.Write(writer: Utf8JsonWriter, value: ProductTypeMode, options: JsonSerializerOptions) =
        JsonSerializer.Serialize(writer, value.ToString(), options)

type ProductTypeModeConverter() =
    inherit JsonConverterFactory()
    override this.CanConvert(t: Type): bool = t.Name = typedefof<ProductTypeMode>.Name

    override this.CreateConverter(typeToConvert: Type, _options: JsonSerializerOptions): JsonConverter =
        let converterType = typedefof<ProductTypeModeValueConverter>
        Activator.CreateInstance(converterType) :?> JsonConverter

let options = JsonSerializerOptions()

options.Converters.Add(ProductTypeModeConverter())

options.Converters.Add
    (JsonFSharpConverter
        (JsonUnionEncoding.InternalTag
         ||| JsonUnionEncoding.UnwrapRecordCases
         ||| JsonUnionEncoding.UnwrapOption,
         unionTagName = "type",
         unionTagCaseInsensitive = true))

let parseLocationsResponse (response: string) =
    JsonSerializer.Deserialize<Response<LocationsResponse>>(response, options).result

let parseProfileResponse (response: string) =
    Some(JsonSerializer.Deserialize<Profile>(response, options))

let parseJourneysResponse (response: string) =
    JsonSerializer.Deserialize<Response<JourneysResponse>>(response, options).result

let parseTrip (response: string) =
    JsonSerializer.Deserialize<Response<Trip>>(response, options).result
