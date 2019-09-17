namespace Skills.Infrastructure

open System
open Skills.Infrastructure.Dto
open Skills.Domain.Types
open Skills.Domain
open Skills.Domain.Result

type DatedUserEvaluationDto = {
    date: DateTime
    user: UserDto
    evaluation: EvaluationDto
}

module DatedUserEvaluationDto =

    let toDomainEvent userEvaluation =

        let result = result{
            let! user = UserDto.toDomain userEvaluation.user
            let! evaluation = EvaluationDto.toDomain userEvaluation.evaluation
            return EvaluationAdded.create { user = user; evaluation = evaluation } userEvaluation.date
        }

         match result with 
         | Error err -> failwith err
         | Ok evaluationAdded -> evaluationAdded