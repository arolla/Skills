namespace Skills.Infrastructure

open System

module Dto =
    
    type EvaluationDto = {
        skill : string
        date : DateTime
        level : int
    }
       
    type UserDto = {
        name : string
    }
    
    type UserSkillDto = {
        user : UserDto
        evaluation : EvaluationDto
    }
    
    type UserSkillsDto = {
        user : UserDto
        evaluations : EvaluationDto []
    }
    

