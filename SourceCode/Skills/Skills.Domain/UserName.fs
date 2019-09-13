namespace Skills.Domain

open System
open Types

module UserName =
    
    let create name =
        if String.IsNullOrWhiteSpace(name) then sprintf "Name is invalid (%s)" name |> Error
        else
        UserName name |> Ok

    let value userName =
        let (UserName name) = userName
        name
