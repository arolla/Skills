namespace Skills.Infrastructure

open System
open Skills.Infrastructure.Dto
open Skills.Domain.Types
open Skills.Domain

type DatedUserEvaluationDto = {
    date: DateTime
    user: UserDto
    evaluation: EvaluationDto
}

module DatedUserEvaluationDto =

    let toDomainEvent userEvaluation =
        let user = UserDto.toDomain userEvaluation.user
        let evaluation = EvaluationDto.toDomain userEvaluation.evaluation
    
        match user with
        | Error err -> failwith err 
        | Ok user ->
        match evaluation with
        | Error err -> failwith err 
        | Ok evaluation ->
        let userSkill:UserEvaluation = {
            user = user
            evaluation = evaluation
        }

        EvaluationAdded.create userSkill userEvaluation.date
    