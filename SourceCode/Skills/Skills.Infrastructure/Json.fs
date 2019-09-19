namespace Skills.Infrastructure

open Newtonsoft.Json

module Json =

    let serialize o =
        JsonConvert.SerializeObject o

    let deserialize<'o> s =
        try
            JsonConvert.DeserializeObject<'o>(s) |> Ok
        with
        | ex -> Error ex