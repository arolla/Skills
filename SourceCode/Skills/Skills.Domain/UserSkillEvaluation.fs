namespace Skills.Domain

module UserSkillEvaluation =
    
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

 

    module User =
        let create name =
            UserName.create name
            |> Result.map (fun userName -> {name = userName})
            
    
    let addEvaluationToUserSkills evaluation userSkills = 
        {
            userSkills with evaluations = evaluation :: userSkills.evaluations 
        }

    let findSkills user (usersSkills:UserSkills list) =
        let foundSkills = List.tryFind (fun userSkill -> userSkill.user = user) usersSkills
        match foundSkills with 
        | None -> { user = user; evaluations = []}
        | Some userSkills -> userSkills
    

    type AddEvaluationError =
    | SaveException of except: exn
    | ReadUserSkillsErrors of errors: string list

    let addEvaluation readSkills saveSkills user evaluation =
        async {
            match! readSkills user with
            | Ok userSkills -> 
                return! async {
                    let! result = 
                        addEvaluationToUserSkills evaluation userSkills 
                        |> saveSkills
                    return result 
                    |> Result.mapError (fun err -> SaveException err)
                }
            | Error errors -> 
                return errors |> ReadUserSkillsErrors |> Error
        }