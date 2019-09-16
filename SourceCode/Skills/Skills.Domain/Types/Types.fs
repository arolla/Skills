namespace Skills.Domain


module Types =

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
