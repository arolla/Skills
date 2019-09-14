namespace Skills.Infrastructure

open Skills.Domain.UserSkillEvaluation
open Newtonsoft.Json
open Skills.Domain
open Skills.Infrastructure.Dto

module UserSkillEvaluation =

    module User =
        let fromDto (dto:UserDto) =
            User.create dto.name
    
    let convertSkills (userSkills: UserSkills) : UserSkillsDto =
        let toUserDto (user: User) =
            {name = UserName.value user.name}
        let toEvaluationsDto ({skill = skill; date = EvaluationDate date; level = level}: Evaluation) =
            {
                skill = Skill.value skill
                date = date
                level = Level.value level
            }
        
        {
            user = toUserDto userSkills.user
            evaluations = userSkills.evaluations |> Array.ofList |> Array.map toEvaluationsDto 
        }

    let convertDtoSkills (userSkills: UserSkillsDto) : Result<UserSkills, string list> =
            
        let userResult = UserDto.toDomain userSkills.user
        let evaluations = 
            userSkills.evaluations 
            |> Array.map EvaluationDto.toDomain
            |> Array.fold (fun acc res ->
                match acc, res with
                | Ok evals, Ok(eval)        -> Ok (eval :: evals)
                | Ok _, Error(error)        -> Error([error])
                | Error errors, Ok _        -> Error errors
                | Error errors, Error error -> Error(error::errors)
            ) (Ok([]))

        match userResult with
        | Error error -> Error [error]
        | Ok user ->
        match evaluations with
        | Error error -> Error error
        | Ok evaluations ->
            let userSkills : UserSkills = {
                user = user
                evaluations = evaluations
            }
            userSkills |> Ok


    let serializeSkills usersSkills =
        JsonConvert.SerializeObject(usersSkills)

    let deserializeUserSkills jsonContent =
        JsonConvert.DeserializeObject<UserSkillsDto>(jsonContent)

    
    let addEvaluation readUserSkills saveUserSkills (event : EvaluationAddedDto) =
        let toAsyncEvaluationError message =
            async { return message |> exn |> SaveException |> Error }

        let userSkill = JsonConvert.DeserializeObject<UserSkillDto> event.data
        let evaluationResult = 
            Evaluation.create 
                userSkill.evaluation.skill 
                userSkill.evaluation.level
                userSkill.evaluation.date
        match evaluationResult with
        | Error message -> toAsyncEvaluationError message
        | Ok evaluation ->
        let userResult = User.create userSkill.user.name
        match userResult with
        | Error message -> toAsyncEvaluationError message
        | Ok user ->
        UserSkillEvaluation.addEvaluation
            readUserSkills
            saveUserSkills
            user
            evaluation