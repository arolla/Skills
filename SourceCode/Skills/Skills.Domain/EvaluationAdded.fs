namespace Skills.Domain

open Types

type EvaluationAdded = {    
    date: EventDate
    user: User
    evaluation: Evaluation
}

module EvaluationAdded =


    let create (userSkill:UserEvaluation) date =
        {
            date = EventDate.create date
            user = userSkill.user
            evaluation = userSkill.evaluation
        }
