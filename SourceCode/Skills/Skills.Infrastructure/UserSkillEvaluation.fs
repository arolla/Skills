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
        let (UserName name) = userSkills.user.name
        let toUserDto (user: User) =
            {name = name}
        let toEvaluationsDto ({skill = Skill skill; date = EvaluationDate date; level = Level level}: Evaluation) =
            {
                skill = skill
                date = date
                level = level
            }
        
        {
            user = toUserDto userSkills.user
            evaluations = userSkills.evaluations |> Array.ofList |> Array.map toEvaluationsDto 
        }

    let convertDtoSkills (userSkills: UserSkillsDto): UserSkills =
        let fromUserDto (user: UserDto) : User =
            {name = UserName user.name}

        let fromEvaluationsDto ({skill = skill; date = date; level = level}: EvaluationDto) : Evaluation =
            {
                skill = Skill skill
                date = EvaluationDate date
                level = Level level
            }
           
        {
            user = fromUserDto userSkills.user
            evaluations = Array.map fromEvaluationsDto userSkills.evaluations |> List.ofArray
        }

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