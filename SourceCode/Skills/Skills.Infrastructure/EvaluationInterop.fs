﻿namespace Skills.Infrastructure

open UserSkillEvaluation
open UserSkillsRepo
open EventStore
open EventRepo
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

    let AddEvaluation connectionString (user:UserSkillDto) =
        let readSkills = readUsersSkills connectionString
        let saveSkills = saveUsersSkills connectionString
        addEvaluation readSkills saveSkills user.user user.evaluation

    let AddEvaluationAddedEvent connectionString (userEvaluation:UserEvalutationDto) =
        let saveEvent = saveEvent connectionString
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
        addEvent saveEvent evaluationAddedEvent
        