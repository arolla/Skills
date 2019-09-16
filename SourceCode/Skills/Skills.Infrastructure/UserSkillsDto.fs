namespace Skills.Infrastructure

open Skills.Domain.Types
open Dto
open Skills.Domain

module UserSkillsDto =

    let toDomain (dto:UserSkillsDto) = 
        let toDomain user evaluations : UserEvaluations =
            {
                user = user
                evaluations = evaluations
            }
        let userResult = User.create dto.user.name |> Result.mapError List.singleton

        let evaluationsResults = 
            dto.evaluations
            |> Array.map EvaluationDto.toDomain
            |> Array.fold (fun acc res ->
                match acc, res with
                | Ok evals, Ok(eval)        -> Ok (eval :: evals)
                | Ok _, Error(error)        -> Error([error])
                | Error errors, Ok _        -> Error errors
                | Error errors, Error error -> Error(error::errors)
            ) (Ok([]))

        match userResult with
        | Error error -> Error error
        | Ok user ->
        match evaluationsResults with
        | Error errors -> Error errors
        | Ok evaluations ->
        toDomain user evaluations |> Ok


    let fromDomain (userSkills:UserEvaluations) : UserSkillsDto =
        let evaluations = userSkills.evaluations |> List.map EvaluationDto.fromDomain |> Array.ofList
        {
            user = {name = UserName.value userSkills.user.name}
            evaluations = evaluations
        }