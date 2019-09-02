namespace Skills.Infrastructure

open UserSkillEvaluation
open UserSkillsRepo
open EventStore
open EventRepo
open EventSender
open System
open Newtonsoft.Json

module EvaluationInterop =

    type UserSkillDto = {
        user : UserDto
        evaluation : EvaluationDto
    }
    type UserEvalutationDto = {
        date: DateTime
        User: UserDto
        Evaluation: EvaluationDto
    }

    let GetUserSkillFromEvent event =
        JsonConvert.DeserializeObject<UserSkillDto>(event.data)        

    let AddEvaluation connectionString event =
        let user = GetUserSkillFromEvent event 
        let readSkills = readUsersSkills connectionString
        let saveSkills = saveUsersSkills connectionString
        addEvaluation readSkills saveSkills user.user user.evaluation

    let AddEvaluationAddedEvent connectionString (userEvaluation:UserEvalutationDto) =
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
        addEvent saveEvent enqueue evaluationAddedEvent
        