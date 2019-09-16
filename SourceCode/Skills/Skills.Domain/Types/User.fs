namespace Skills.Domain

open Types
     
module User =
    let create name =
        UserName.create name
        |> Result.map (fun userName -> {name = userName})