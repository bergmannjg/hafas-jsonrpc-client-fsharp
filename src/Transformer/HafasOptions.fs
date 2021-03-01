module HafasOptions

//
// Hafas domain specfic transformations
//

let prelude = """/// generated by ts2fable and transformer
module rec Hafas

open System

// type Promise<'T> = Async<'T>
type Promise<'T> =
    abstract _catch: onrejected:option<obj -> 'T> -> Promise<'T>
    abstract _then: onfulfilled:option<'T -> 'TResult> * onrejected:option<obj -> 'TResult> -> Promise<'TResult>

type U2<'a, 'b> =
    | Case1 of 'a
    | Case2 of 'b

type U3<'a, 'b, 'c> =
    | Case1 of 'a
    | Case2 of 'b
    | Case3 of 'c

type IndexMap<'s, 'b when 's: comparison>(defaultValue: 'b) =
    let mutable map: Map<'s, 'b> = Map.empty

    member __.Item
        with get (s: 's) =
            match map.TryFind s with
            | Some v -> v
            | None -> defaultValue
        and set s b =
            map <- map.Add(s, b)
            ()

    member __.Keys =
        map |> Seq.map (fun kv -> kv.Key) |> Seq.toArray
"""

let postlude = """
"""

let transformType str =
    if str = "ReadonlyArray" then "array"
    else if str = "ResizeArray" then "array"
    else str

let escapeIdent str =
    if str = "type" then "``" + str + "``"
    else if str = "default" then "``" + str + "``"
    else if str = "when" then "``" + str + "``"
    else if str = "public" then "``" + str + "``"
    else if str = "to" then "``" + str + "``"
    else str

let excludeTypes = [| "ReadonlyArray"; "IExports" |]

let transformTypeVals =
    [| "transferTime", "int option"
       "transfers", "int option"
       "delay", "int option"
       "departureDelay", "int option"
       "arrivalDelay", "int option"
       "transfers", "int option"
       "transfers", "int option"
       "results", "int option"
       "bitmasks", "array<int>"
       "``type``", "string option" |]

let transformTypeDefns =
    [| "ScheduledDays", "IndexMap<string, bool>"
       "Products", "IndexMap<string, bool>"
       "Facilities", "IndexMap<string, string>"
       "Ids", "IndexMap<string, string>" |]

let transformsType (name: string) (arr: (string * string) array) =
    let index =
        Array.tryFindIndex (fun (s, _) -> s = name) arr

    if (index.IsSome) then
        let (_, transform) = arr.[index.Value]
        Some transform
    else
        None

let transformsTypeVal (name: string) = transformsType name transformTypeVals

let transformsTypeDefn (name: string) = transformsType name transformTypeDefns

let excludesType (name: string) =
    Array.exists (fun s -> s = name) excludeTypes

let options: Transformer.TransformerOptions =
    { prelude = Some prelude
      postlude = Some postlude
      escapeIdent = escapeIdent
      transformType = transformType
      excludesType = excludesType
      transformsTypeVal = transformsTypeVal
      transformsTypeDefn = transformsTypeDefn }
