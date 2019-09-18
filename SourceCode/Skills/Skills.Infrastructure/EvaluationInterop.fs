namespace Skills.Infrastructure

open UserSkillEvaluation
open UserSkillsRepo
open EventStore
open EventRepo
open EventSender

module EvaluationInterop =

    let AddEvaluationAsync connectionString event =
        if System.String.IsNullOrWhiteSpace(connectionString) then invalidArg "connectionString" "Must not be null, empty or whitespace"
        if isNull(box event) then nullArg "event"
        if System.String.IsNullOrWhiteSpace(event.data) then invalidArg "event.data" "Must not be null, empty or whitespace"
        if System.String.IsNullOrWhiteSpace(event.eventType) then invalidArg "event.eventType" "Must not be null, empty or whitespace"

        
        let readSkills = readUserSkills connectionString
        let saveSkills = saveUsersSkills connectionString

        async{
            match! addEvaluation readSkills saveSkills event with
            | Ok _      -> ()
            | Error (Skills.Domain.UserSkillEvaluation.AddEvaluationError.EvaluationAlreadyExists evaluation) -> 
                sprintf "This evaluation already exists: %A" evaluation |> exn |> raise
            | Error (Skills.Domain.UserSkillEvaluation.AddEvaluationError.SaveException exn) -> raise exn
            | Error (Skills.Domain.UserSkillEvaluation.AddEvaluationError.ReadUserSkillsErrors errors) -> 
                errors |> String.concat ", " |> exn |> raise
        }
        |> Async.StartImmediateAsTask :> System.Threading.Tasks.Task


    let AddEvaluationAddedEventAsync connectionString (userEvaluation:DatedUserEvaluationDto) =
        if System.String.IsNullOrWhiteSpace(connectionString) then invalidArg "connectionString" "Must not be null, empty or whitespace"
        if isNull(box userEvaluation) then nullArg "userEvaluation"
        if isNull(box userEvaluation.evaluation) then nullArg "userEvaluation.evaluation"
        if isNull(box userEvaluation.user) then nullArg "userEvaluation.user"

        let saveEvent = saveEvent connectionString
        let enqueue = sendEvent connectionString
            
        async{
            match! addEvent saveEvent enqueue userEvaluation with
            | Ok ()     -> ()
            | Error exn -> raise exn
        }
        |> Async.StartImmediateAsTask :> System.Threading.Tasks.Task
