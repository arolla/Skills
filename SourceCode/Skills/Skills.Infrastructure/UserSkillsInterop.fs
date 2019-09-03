namespace Skills.Infrastructure

open UserSkillsRepo
open UserSkillEvaluation

module UserSkillsInterop =

    let ReadUserSkills connectionString user =
        match readUsersSkills connectionString user.name with
        | None -> {user = user; evaluations = [||]}
        | Some userSkills -> userSkills