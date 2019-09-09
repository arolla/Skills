namespace Skills.Infrastructure

open UserSkillEvaluation
open UserSkillsRepo
open EventStore
open EventRepo
open EventSender
open System
open Newtonsoft.Json

module EvaluationInterop =

    
    type UserEvalutationDto = {
        date: DateTime
        User: UserDto
        Evaluation: EvaluationDto
    }

    let GetUserSkillFromEvent event =
        JsonConvert.DeserializeObject<UserSkillDto>(event.data)        

    let AddEvaluationAsync connectionString event =
        let user = GetUserSkillFromEvent event 
        let readSkills = readUsersSkills connectionString
        let saveSkills = saveUsersSkills connectionString

        async{
            match! addEvaluation_ToDelete readSkills saveSkills user.user user.evaluation with
            | Ok _      -> ()
            | Error exn -> raise exn
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
        
