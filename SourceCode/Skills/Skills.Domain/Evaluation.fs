namespace Skills.Domain

open System
open Skill
open Level

type EvaluationDate = EvaluationDate  of DateTime

type Evaluation = {
    skill : Skill
    date : EvaluationDate
    level : Level
}

module Evaluation =

    let create skill level date = 
        let skillResult = Skill.create skill
        match skillResult with
        | Error message -> Error message
        | Ok skill -> 
        let levelResult = Level.create level
        match levelResult with
        | Error message -> Error message
        | Ok level ->
        {skill = skill; level = level; date = EvaluationDate date} |> Ok
