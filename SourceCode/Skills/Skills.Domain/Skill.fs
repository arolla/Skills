namespace Skills.Domain

open System

type Skill = Skill of string 

module Skill = 

    let create skill = 
        if String.IsNullOrWhiteSpace(skill) then
            sprintf "Skill is not valid (%s)" skill |> Error
        else
            Skill skill |> Ok