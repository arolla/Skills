namespace Skills.Domain

open System

module Types =
    type Skill = Skill of string
    
    type Level = Level of int

    type EvaluationDate = EvaluationDate  of DateTime

    type Evaluation = {
        skill : Skill
        date : EvaluationDate
        level : Level
    }

    type UserName = UserName of string

    type User = {
        name : UserName
    }

    type UserSkill = {
        user : User
        evaluation : Evaluation
    }
    
    type UserSkills = {
        user : User
        evaluations : Evaluation list
    }