namespace Skills.Infrastructure

open System
open Skills.Domain
open Skills.Domain.Event
open UserSkillEvaluation
open Newtonsoft.Json
open Skills.Infrastructure.Dto

type EvaluationAddedDto = {
    date: DateTime
    data: string
    eventType: string
}

module EventStore =
    
    type private SaveEvent = EvaluationAddedDto -> Async<Result<unit, exn>>
    type private Enqueue = EvaluationAddedDto -> Async<Result<unit, exn>>

    let convertToDto (domainEvent:EvaluationAdded) =
        
        let ({skill = Skill skill; level = Level level; date = EvaluationDate date} : Evaluation) = domainEvent.evaluation
        let (UserName name) = domainEvent.user.name
        let userSkill = {
            user = {name = name}
            evaluation = { skill = skill; level = level; date = date }
        }

        let serializedUserSkill = JsonConvert.SerializeObject(userSkill)
        let (EventDate date) = domainEvent.date

        {
            date = date
            data = serializedUserSkill
            eventType = typeof<EvaluationAdded>.Name
        }

    let addEvent (saveEvent:SaveEvent) (enqueue: Enqueue) evaluationAddedDto =
        async {
            let! r = saveEvent evaluationAddedDto
            match r with
            | Error _ as error -> return error
            | Ok _ ->
            return! enqueue evaluationAddedDto
        }