﻿namespace Skills.Infrastructure

open System
open Skills.Domain
open Skills.Infrastructure.Dto

type EvaluationAddedDto = {
    date: DateTime
    data: string
    eventType: string
}

module EventStore =
    
    let convertToDto (domainEvent:EvaluationAdded) =
        
        let ({skill = skill; level = level; date = date} : Evaluation) = domainEvent.evaluation
        let userSkill = {
            user = {name = UserName.value domainEvent.user.name}
            evaluation = { skill = Skill.value skill; level = Level.value level; date = EvaluationDate.value date }
        }


        {
            date = EventDate.value domainEvent.date
            data = Json.serialize userSkill
            eventType = typeof<EvaluationAdded>.Name
        }

    let addEvent saveEvent enqueue userEvaluation =
        async {
            let evaluationAddedDto =
                userEvaluation
                |> DatedUserEvaluationDto.toDomainEvent 
                |> convertToDto

            let! r = saveEvent evaluationAddedDto
            match r with
            | Error _ as error -> return error
            | Ok _ ->
            return! enqueue evaluationAddedDto
        }