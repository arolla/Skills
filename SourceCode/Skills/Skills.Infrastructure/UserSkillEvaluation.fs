namespace Skills.Infrastructure

open Skills.Domain.Types
open Skills.Domain.UserSkillEvaluation
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
                |> Result.arrayOfResultToResultList

            let userSkills : UserEvaluations = {
                user = user
                evaluations = evaluations
            }
            return userSkills
        }

    
    let addEvaluation readUserSkills saveUserSkills (event : EvaluationAddedDto) =

        let exceptionToAddEvaluationError = SaveException >> Error
        let stringToAddEvaluationError = exn >> exceptionToAddEvaluationError


        let results = result {
            let! userSkill = 
                Json.deserialize<UserSkillDto> event.data 
                |> Result.mapError exceptionToAddEvaluationError
            let! evaluation = 
                Evaluation.create 
                    userSkill.evaluation.skill 
                    userSkill.evaluation.level
                    userSkill.evaluation.date
                |> Result.mapError stringToAddEvaluationError
            let! user = 
                User.create userSkill.user.name
                |> Result.mapError stringToAddEvaluationError

            return evaluation, user
        }
        match results with
        | Error err -> async{return err}
        | Ok (evaluation, user) ->
            addEvaluation
                readUserSkills
                saveUserSkills
                user
                evaluation