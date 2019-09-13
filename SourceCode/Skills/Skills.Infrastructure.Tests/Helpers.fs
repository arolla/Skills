namespace Skills.Infrastructure.Tests

open Skills.Domain.UserSkillEvaluation

module Helpers =
    let userName name =
        match UserName.create name with
        | Error _ -> "Unable to create a username" |> failwith
        | Ok username -> username


