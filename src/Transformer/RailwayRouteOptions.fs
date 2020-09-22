module RailwayRouteOptions

//
// RailwayRoute domain specfic transformations
//

let prelude = """/// generated by ts2fable and transformer
module rec RailwayRoute

open System

"""

let transformType str =
    if str = "ReadonlyArray" then "array"
    else if str = "ResizeArray" then "array"
    else if str = "U2<float,string>" then "float"
    else str

let escapeIdent str =
    if str = "type" then "``" + str + "``"
    else if str = "to" then "``" + str + "``"
    else if str = "default" then "``" + str + "``"
    else if str = "when" then "``" + str + "``"
    else if str = "public" then "``" + str + "``"
    else str

let excludeTypes = [| "ReadonlyArray"; "IExports" |]

let transformTypeVals =
    [| "STRECKE_NR", "int option"
       "railwayRouteNr", "int option" |]

let transformTypeDefns = [||]

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
      escapeIdent = escapeIdent
      transformType = transformType
      excludesType = excludesType
      transformsTypeVal = transformsTypeVal
      transformsTypeDefn = transformsTypeDefn }
