namespace Skills.Infrastructure

open System
open Skills.Domain.Event
open Skills.Domain.UserSkillEvaluation
open Newtonsoft.Json

module EventStore =
    
    type EvaluationAddedDto = {
        date: DateTime
        data: string
        eventType: string
    }

    let convertToDto (eventDomain:EvaluationAdded) =
        
        let ({skill = Skill skill; level = Level level; date = EvaluationDate date}) = eventDomain.evaluation

        let userSkill = {|
            user = {|name = eventDomain.user.name|}
            evaluation = {| skill = skill; level = level; date = date |}
        |}

        let serializedUserSkill = JsonConvert.SerializeObject(userSkill)
        let (EventDate date) = eventDomain.date

        {
            date = date
            data = serializedUserSkill
            eventType = typeof<EvaluationAdded>.Name
        }

            
