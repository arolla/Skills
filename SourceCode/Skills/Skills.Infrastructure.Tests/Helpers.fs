namespace Skills.Infrastructure.Tests

open Skills.Domain
open Skills.Infrastructure

module Helpers =
    let userName name =
        match UserName.create name with
        | Error _ -> "Unable to create a username" |> failwith
        | Ok username -> username

    let level level =
        match Level.create level with
        | Error _ -> "Unable to create a level" |> failwith
        | Ok level -> level

    let skill skill =
        match Skill.create skill with
        | Error _ -> "Unable to create a skill" |> failwith
        | Ok skill -> skill

    let userSkill dto =
        match UserSkillDto.toDomain dto with
        | Error _ -> "Unable to create a userSkill" |> failwith
        | Ok userSkill -> userSkill
