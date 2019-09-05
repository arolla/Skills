namespace Skills.Infrastructure

open UserSkillsRepo
open UserSkillEvaluation

module UserSkillsInterop =

    let ReadUserSkillsAsync connectionString user =
        async {
            match! readUsersSkills connectionString user.name with
            | None              -> return {user = user; evaluations = [||]}
            | Some userSkills   -> return userSkills
        } |> Async.StartImmediateAsTask