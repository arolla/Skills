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
        evaluations : EvaluationDto list
    }

    let convertSkills (userSkills: UserSkills list) : (UserSkillsDto list) =
        let toUserDto (user: User) =
            {name = user.name}

        let toEvaluationsDto ({skill = Skill skill; date = EvaluationDate date; level = Level level}: Evaluation) =
            {
                skill = skill
                date = date
                level = level
            }
        
        userSkills
        |> List.map (fun domainSkills -> 
            {
                user = toUserDto domainSkills.user
                evaluations = List.map toEvaluationsDto domainSkills.evaluations
            }
        )

    let convertDtoSkills (userSkills: UserSkillsDto list): (UserSkills list) =
        let fromUserDto (user: UserDto):(User) =
            {name = user.name}

        let fromEvaluationsDto ({skill = skill; date = date; level = level}: EvaluationDto):(Evaluation) =
            {
                skill = Skill skill
                date = EvaluationDate date
                level = Level level
            }
            
        userSkills
        |> List.map (fun domainSkills -> 
            {
                user = fromUserDto domainSkills.user
                evaluations = List.map fromEvaluationsDto domainSkills.evaluations
            }
        )

        


    let serializeSkills usersSkills =
        JsonConvert.SerializeObject(usersSkills)

    let deserializeSkills jsonContent =
        JsonConvert.DeserializeObject<UserSkillsDto list>(jsonContent)

    let addEvaluation readSkills saveSkills user evaluation =
        let jsonSkills = readSkills()
        let foundUserSkills = deserializeSkills jsonSkills
        let convertedUserSkills = convertDtoSkills foundUserSkills
        let userSkills = findSkills user convertedUserSkills
        let updatedUserSkills = addEvaluation evaluation userSkills
        let updatedUserSkillsDto = convertSkills [updatedUserSkills]
        saveSkills updatedUserSkillsDto
        ()
