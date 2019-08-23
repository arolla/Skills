namespace Skills.Domain

open System

module UserSkillEvaluation =
    
    type Level = Level of int   
    
    type Skill = Skill of string 
    
    type EvaluationDate = EvaluationDate  of DateTime
    
    type Evaluation = {
        skill : Skill
        date : EvaluationDate
        level : Level
    }
    
    type User = {
        name : string
    }
    
    type UserSkills = {
        user : User
        evaluations : Evaluation list
    }
    
    let addEvaluation evaluation userSkills = 
        {
            userSkills with evaluations = evaluation :: userSkills.evaluations 
        }
