namespace Skills.Infrastructure

open UserSkillEvaluation
open UserSkillsRepo
open EventStore
open EventRepo
open EventSender
open System
open Newtonsoft.Json
open Skills.Infrastructure.Dto

module EvaluationInterop =
    
    type UserEvalutationDto = {
        date: DateTime
        User: UserDto
        Evaluation: EvaluationDto
    }

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

    let AddEvaluationAddedEventAsync connectionString (userEvaluation:UserEvalutationDto) =
        let saveEvent = saveEvent connectionString
        let enqueue = sendEvent connectionString
        let userSkill = {
            user = userEvaluation.User
            evaluation = userEvaluation.Evaluation
        }
        let data = JsonConvert.SerializeObject(userSkill)
        let evaluationAddedEvent:EvaluationAddedDto = {
            date = userEvaluation.date
            data = data
            eventType = "EvaluationAdded"
        }
        async{
            match! addEvent saveEvent enqueue evaluationAddedEvent with
            | Ok ()     -> ()
            | Error exn -> raise exn
        }
        |> Async.StartImmediateAsTask :> System.Threading.Tasks.Task
        
