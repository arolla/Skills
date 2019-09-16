namespace Skills.Infrastructure

open System
open Skills.Domain
open Skills.Domain.Event
open Newtonsoft.Json
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

        let serializedUserSkill = JsonConvert.SerializeObject(userSkill)

        {
            date = EventDate.value domainEvent.date
            data = serializedUserSkill
            eventType = typeof<EvaluationAdded>.Name
        }

    let addEvent saveEvent enqueue evaluationAddedDto =
        async {
            let! r = saveEvent evaluationAddedDto
            match r with
            | Error _ as error -> return error
            | Ok _ ->
            return! enqueue evaluationAddedDto
        }