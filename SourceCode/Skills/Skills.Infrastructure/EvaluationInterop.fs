namespace Skills.Infrastructure

open UserSkillEvaluation
open UserSkillsRepo
open EventStore
open EventRepo
open EventSender
open Newtonsoft.Json
open Skills.Infrastructure.Dto

module EvaluationInterop =
    

    let GetUserSkillFromEvent event =
        JsonConvert.DeserializeObject<UserSkillDto>(event.data)        

    let AddEvaluationAsync connectionString event =
        let readSkills = readUserSkills connectionString
        let saveSkills = saveUsersSkills connectionString

        async{
            match! addEvaluation readSkills saveSkills event with
            | Ok _      -> ()
            | Error (Skills.Domain.UserSkillEvaluation.AddEvaluationError.SaveException exn) -> raise exn
            | Error (Skills.Domain.UserSkillEvaluation.AddEvaluationError.ReadUserSkillsErrors errors) -> 
                errors |> String.concat ", " |> exn |> raise
        }
        |> Async.StartImmediateAsTask :> System.Threading.Tasks.Task

    let AddEvaluationAddedEventAsync connectionString (userEvaluation:DatedUserEvaluationDto) =

        let saveEvent = saveEvent connectionString
        let enqueue = sendEvent connectionString
            
        async{
            match! addEvent saveEvent enqueue userEvaluation with
            | Ok ()     -> ()
            | Error exn -> raise exn
        }
        |> Async.StartImmediateAsTask :> System.Threading.Tasks.Task
