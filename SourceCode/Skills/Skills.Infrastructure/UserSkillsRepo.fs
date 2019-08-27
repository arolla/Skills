namespace Skills.Infrastructure

open System
open Skills.Infrastructure.UserSkillEvaluation
open Microsoft.WindowsAzure.Storage.Table
open Microsoft.WindowsAzure.Storage

module UserSkillsRepo =

    type UserSkillsEntity (userName, userSkills : string) =
        inherit TableEntity("1", userName)
        new() = UserSkillsEntity(null, null)
        member val userSkills : string = userSkills with get, set

    let getConnectionString() =
        String.Empty
    
    let getUserSkillsTable connectionString =
        let storageAccount = CloudStorageAccount.Parse(connectionString)
        let tableClient = storageAccount.CreateCloudTableClient()
        let table = tableClient.GetTableReference("userskills")
        table

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
