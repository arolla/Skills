namespace Skills.Infrastructure

open Skills.Domain.Types
open Skills.Domain.UserSkillEvaluation
open Newtonsoft.Json
open Skills.Domain
open Skills.Infrastructure.Dto
open Skills.Domain.Result

module UserSkillEvaluation =

    module User =
        let fromDto (dto:UserDto) =
            User.create dto.name
    
    let convertSkills (userSkills: UserEvaluations) : UserSkillsDto =
        let toUserDto (user: User) =
            {name = UserName.value user.name}
        let toEvaluationsDto ({skill = skill; date =  date; level = level}: Evaluation) =
            {
                skill = Skill.value skill
                date = EvaluationDate.value date
                level = Level.value level
            }
        
        {
            user = toUserDto userSkills.user
            evaluations = userSkills.evaluations |> Array.ofList |> Array.map toEvaluationsDto 
        }

    let convertDtoSkills (userSkills: UserSkillsDto) : Result<UserEvaluations, string list> =
        
        result{
            let! user = UserDto.toDomain userSkills.user |> Result.mapError (fun error -> [error])
            let! evaluations = 
                userSkills.evaluations 
                |> Array.map EvaluationDto.toDomain
                |> Array.fold (fun acc res ->
                    match acc, res with
                    | Ok evals, Ok(eval)        -> Ok (eval :: evals)
                    | Ok _, Error(error)        -> Error([error])
                    | Error errors, Ok _        -> Error errors
                    | Error errors, Error error -> Error(error::errors)
                ) (Ok([]))

            let userSkills : UserEvaluations = {
                user = user
                evaluations = evaluations
            }
            return userSkills
        }


    let serializeSkills usersSkills =
        JsonConvert.SerializeObject(usersSkills)

    let deserializeUserSkills jsonContent =
        JsonConvert.DeserializeObject<UserSkillsDto>(jsonContent)
    
    let addEvaluation readUserSkills saveUserSkills (event : EvaluationAddedDto) =

        let toAddEvaluationError = exn >> SaveException >> Error
        let toAsyncAddEvaluationError err =
            async {return toAddEvaluationError err}

        let userSkill = JsonConvert.DeserializeObject<UserSkillDto> event.data

        let results = result {
            let! evaluation = 
                Evaluation.create 
                    userSkill.evaluation.skill 
                    userSkill.evaluation.level
                    userSkill.evaluation.date
            let! user = User.create userSkill.user.name
            return evaluation, user
        }
        match results with
        | Error err -> toAsyncAddEvaluationError err
        | Ok (evaluation, user) ->
            addEvaluation
                readUserSkills
                saveUserSkills
                user
                evaluation