module Program

[<EntryPoint>]
let main argv =
    match argv with
    | [| target; fromFile; toFile |] ->
        let options: Transformer.TransformerOptions =
            match target with
            | "Hafas" -> HafasOptions.options
            | "FsHafas" -> FsHafasOptions.options
            | "RailwayRoute" -> RailwayRouteOptions.options
            | _ -> failwith "unknown target"

        Transformer.transform fromFile toFile options
    | _ -> failwith "arguments expected: target fromFile toFile"
    0 // return an integer exit code
