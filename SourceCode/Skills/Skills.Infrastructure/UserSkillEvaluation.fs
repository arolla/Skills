namespace Skills.Infrastructure

open System
open Skills.Domain.UserSkillEvaluation
open Newtonsoft.Json

module UserSkillEvaluation =
    
    type EvaluationDto = {
        skill : string
        date : DateTime
        level : int
    }
    
    type UserDto = {
        name : string
    }
    
    type UserSkillsDto = {
        user : UserDto
        evaluations : EvaluationDto []
    }

    type private ReadSkills = string -> Async<UserSkillsDto option>
    type private SaveSkills = UserSkillsDto -> Async<Result<unit, exn>>


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


    let addEvaluation (readSkills:ReadSkills) (saveSkills:SaveSkills) (user:UserDto) (evaluation:EvaluationDto) =
        let domainEvaluation : Evaluation = {
            skill = Skill(evaluation.skill)
            date = EvaluationDate(evaluation.date)
            level = Level(evaluation.level)
        }

        async{
            let! userSkills = readSkills user.name 
            return!
                match userSkills with
                | None -> 
                    {
                        user = { name = user.name }
                        evaluations = [||] // empty array
                    }
                | Some(userSkill) -> userSkill
                |> convertDtoSkills 
                |> addEvaluation domainEvaluation
                |> convertSkills
                |> saveSkills
        }
