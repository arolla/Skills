namespace Skills.Infrastructure

open Skills.Domain
open Skills.Infrastructure.Dto
open Skills.Domain.Types

module UserSkillDto =

    let toDomain (dto:UserSkillDto) : Result<UserEvaluation, string> =
        let userResult = User.create dto.user.name
        match userResult with 
        | Error message -> Error message
        | Ok user -> 
        let evaluationResult = 
            Evaluation.create
                dto.evaluation.skill 
                dto.evaluation.level
                dto.evaluation.date
        match evaluationResult with
        | Error message -> Error message
        | Ok evaluation ->
        let userSkill:UserEvaluation = {user = user; evaluation = evaluation}
        userSkill |> Ok
