namespace Skills.Infrastructure

open System
open Skills.Domain.Event
open UserSkillEvaluation
open Skills.Domain.UserSkillEvaluation
open Newtonsoft.Json

module EventStore =
    
    type EvaluationAddedDto = {
        date: DateTime
        data: string
        eventType: string
    }
    type UserSkillDto = {
        user : UserDto
        evaluation : EvaluationDto
    }

    let convertToDto (domainEvent:EvaluationAdded) =
        
        let ({skill = Skill skill; level = Level level; date = EvaluationDate date} : Evaluation) = domainEvent.evaluation

        let userSkill = {
            user = {name = domainEvent.user.name}
            evaluation = { skill = skill; level = level; date = date }
        }

        let serializedUserSkill = JsonConvert.SerializeObject(userSkill)
        let (EventDate date) = domainEvent.date

        {
            date = date
            data = serializedUserSkill
            eventType = typeof<EvaluationAdded>.Name
        }

    let addEvent save enqueue evaluationAddedDto =
        evaluationAddedDto
        |> save
        evaluationAddedDto
        |> enqueue
