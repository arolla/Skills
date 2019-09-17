namespace Skills.Domain

type Evaluation = {
    skill : Skill
    date : EvaluationDate
    level : Level
}

open Result

module Evaluation =

    let create skill level date = 
        result{
            let! skill = Skill.create skill
            let! level = Level.create level
            let date = EvaluationDate.create date
            return {skill = skill; level = level; date = date}        
        }
