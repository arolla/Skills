namespace Skills.Infrastructure

open Skills.Domain.Types
open Dto
open Skills.Domain
open Skills.Domain.Result

module UserSkillsDto =

    let toDomain (dto:UserSkillsDto) = 
        let toDomain user evaluations : UserEvaluations =
            {
                user = user
                evaluations = evaluations
            }

        result{
            let! user = User.create dto.user.name |> Result.mapError List.singleton

            let! evaluations = 
                dto.evaluations
                |> Array.map EvaluationDto.toDomain
                |> Result.arrayOfResultToResultList

            return toDomain user evaluations
        }

    let fromDomain (userSkills:UserEvaluations) : UserSkillsDto =
        let evaluations = userSkills.evaluations |> List.map EvaluationDto.fromDomain |> Array.ofList
        {
            user = {name = UserName.value userSkills.user.name}
            evaluations = evaluations
        }