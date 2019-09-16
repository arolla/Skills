namespace Skills.Domain


module Types =

    type User = {
        name : UserName
    }

    type UserEvaluation = {
        user : User
        evaluation : Evaluation
    }

    type UserEvaluations = {
        user : User
        evaluations : Evaluation list
    }
