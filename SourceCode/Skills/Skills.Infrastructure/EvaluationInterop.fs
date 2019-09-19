namespace Skills.Infrastructure

open UserSkillEvaluation
open UserSkillsRepo
open EventStore
open EventRepo
open EventSender
open Microsoft.Extensions.Logging

module EvaluationInterop =

    let AddEvaluationAsync connectionString (logger:ILogger) event =
        if System.String.IsNullOrWhiteSpace(connectionString) then invalidArg "connectionString" "Must not be null, empty or whitespace"
        if isNull(box logger) then nullArg "logger"
        if isNull(box event) then nullArg "event"
        if System.String.IsNullOrWhiteSpace(event.data) then invalidArg "event.data" "Must not be null, empty or whitespace"
        if System.String.IsNullOrWhiteSpace(event.eventType) then invalidArg "event.eventType" "Must not be null, empty or whitespace"

        
        let readSkills = readUserSkills connectionString
        let saveSkills = saveUsersSkills connectionString

        async{
            match! addEvaluation readSkills saveSkills event with
            | Ok _      -> 
                let message = sprintf "Event processed successfully !%A%A" System.Environment.NewLine event
                logger.LogInformation(message)
            | Error (Skills.Domain.UserSkillEvaluation.AddEvaluationError.EvaluationAlreadyExists evaluation) -> 
                let message = sprintf "This evaluation already exists: %A" evaluation
                logger.LogError(message)
            | Error (Skills.Domain.UserSkillEvaluation.AddEvaluationError.SaveException exn) -> 
                let message = sprintf "Issue on event %A%A%A" event System.Environment.NewLine exn
                logger.LogError(message)
            | Error (Skills.Domain.UserSkillEvaluation.AddEvaluationError.ReadUserSkillsErrors errors) -> 
                let message = sprintf "Issue on event %A%A%A" event System.Environment.NewLine
                let sep = sprintf "%A" System.Environment.NewLine 
                let message = errors |> String.concat sep |> message
                logger.LogError(message)

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
