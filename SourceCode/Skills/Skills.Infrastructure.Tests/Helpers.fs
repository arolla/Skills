namespace Skills.Infrastructure.Tests

open Skills.Domain.UserSkillEvaluation
open Skills.Domain

module Helpers =
    let userName name =
        match UserName.create name with
        | Error _ -> "Unable to create a username" |> failwith
        | Ok username -> username

    let level level =
        match Level.create level with
        | Error _ -> "Unable to create a level" |> failwith
        | Ok level -> level

