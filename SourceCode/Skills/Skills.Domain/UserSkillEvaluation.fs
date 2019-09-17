namespace Skills.Domain

open Types
open System

module UserSkillEvaluation =
                   
    let addEvaluationToUserSkills evaluation userSkills = 
        {
            userSkills with evaluations = evaluation :: userSkills.evaluations 
        }    

    type AddEvaluationError =
    | SaveException of except : exn
    | ReadUserSkillsErrors of errors : string list
    | EvaluationAlreadyExists of evaluation : Evaluation

    let addEvaluation readSkills saveSkills user evaluation =
        let userEvaluationPredicate eval =
            eval.skill = evaluation.skill && eval.date = evaluation.date
                
        async {
            match! readSkills user with
            | Ok userSkills -> 
                if userSkills.evaluations |> List.exists userEvaluationPredicate then 
                    return Error (EvaluationAlreadyExists evaluation)
                else
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