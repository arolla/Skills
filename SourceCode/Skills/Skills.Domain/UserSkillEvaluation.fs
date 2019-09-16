namespace Skills.Domain

open Types

module UserSkillEvaluation =
                   
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