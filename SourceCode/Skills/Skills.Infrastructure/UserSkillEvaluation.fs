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
        
    let deserializeUserSkills jsonContent =
        JsonConvert.DeserializeObject<UserSkillsDto>(jsonContent)

    let addEvaluation readSkills saveSkills user evaluation =
        readSkills()
        |> deserializeSkills
        |> convertDtoSkills
        |> findSkills user
        |> addEvaluation evaluation
        |> List.singleton
        |> convertSkills
        |> saveSkills
        ()

    let getUserSkillsTable connectionString =
        let storageAccount = CloudStorageAccount.Parse(connectionString)
        let tableClient = storageAccount.CreateCloudTableClient()
        let table = tableClient.GetTableReference("userskills")
        table

    let getConnectionString() =
        String.Empty
    
    let saveUsersSkills connectionString (userSkills : UserSkillsDto) =
        let table = connectionString |> getUserSkillsTable 
        let jsonUserSkills = serializeSkills userSkills
        let insertOperation = TableOperation.InsertOrReplace(new UserSkillsEntity(userSkills.user.name, jsonUserSkills))
        let userSkillsResult = async {
            let! result = Async.AwaitTask (table.ExecuteAsync(insertOperation))
            return result.Result
        }
        let result = userSkillsResult |> Async.RunSynchronously        
        ()

    let readUsersSkills connectionString userName : UserSkillsDto =
        let table = connectionString |> getUserSkillsTable 
        let selectOperation = TableOperation.Retrieve<UserSkillsEntity>("1", userName)
        let userSkillsResult = async {
            let! result = Async.AwaitTask (table.ExecuteAsync(selectOperation))
            let jsonUserSkills = (result.Result :?> UserSkillsEntity).userSkills
            return deserializeUserSkills jsonUserSkills
        }
        let result = userSkillsResult |> Async.RunSynchronously
        result