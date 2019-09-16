namespace Skills.Domain

open Types

module Event =

    type EvaluationAdded = {    
        date: EventDate
        user:User
        evaluation:Evaluation
    }
