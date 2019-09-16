namespace Skills.Domain

open System

type Skill = private Skill of string 

module Skill = 

    let create skill = 
        if String.IsNullOrWhiteSpace(skill) then
            sprintf "Skill is not valid (%s)" skill |> Error
        else
            Skill skill |> Ok

    let value skill =
        let (Skill value) = skill
        value