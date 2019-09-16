namespace Skills.Domain

open System
open Types

module Event =
    type EventDate = EventDate of DateTime

    type EvaluationAdded = {    
        date: EventDate
        user:User
        evaluation:Evaluation
    }
