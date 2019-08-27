namespace Skills.Infrastructure

open System
open Skills.Domain.UserSkillEvaluation
open Newtonsoft.Json
open Microsoft.WindowsAzure.Storage
open Microsoft.WindowsAzure.Storage.Table

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
    type UserSkillsEntity (userName, userSkills : string) =
        inherit TableEntity("1", userName)
        new() = UserSkillsEntity(null, null)
        member val userSkills : string = userSkills with get, set

    let convertSkills (userSkills: UserSkills) : (UserSkillsDto) =
        let toUserDto (user: User) =
            {name = user.name}
        let toEvaluationsDto ({skill = Skill skill; date = EvaluationDate date; level = Level level}: Evaluation) =
            {
                skill = skill
                date = date
                level = level
            }
        
        {
            user = toUserDto userSkills.user
            evaluations = List.map toEvaluationsDto userSkills.evaluations
        }

    let convertDtoSkills (userSkills: UserSkillsDto): (UserSkills) =
        let fromUserDto (user: UserDto):(User) =
            {name = user.name}

        let fromEvaluationsDto ({skill = skill; date = date; level = level}: EvaluationDto):(Evaluation) =
            {
                skill = Skill skill
                date = EvaluationDate date
                level = Level level
            }
           
        {
            user = fromUserDto userSkills.user
            evaluations = List.map fromEvaluationsDto userSkills.evaluations
        }

    let serializeSkills usersSkills =
        JsonConvert.SerializeObject(usersSkills)

    let deserializeUserSkills jsonContent =
        JsonConvert.DeserializeObject<UserSkillsDto>(jsonContent)

    let addEvaluation readSkills saveSkills (user:User) evaluation =
        readSkills user.name
        |> convertDtoSkills
        |> addEvaluation evaluation
        |> convertSkills
        |> saveSkills
        ()

    let getUserSkillsTable connectionString =
        let storageAccount = CloudStorageAccount.Parse(connectionString)
        let tableClient = storageAccount.CreateCloudTableClient()
        let table = tableClient.GetTableReference("userskills")
        table
