namespace Skills.Infrastructure

open System
open Skills.Infrastructure.UserSkillEvaluation
open Microsoft.WindowsAzure.Storage.Table
open Microsoft.WindowsAzure.Storage
open Skills.Infrastructure.Dto
open System.Threading.Tasks

module UserSkillsRepo =

    let partitionKey = "1"
    let userSkillsTable = "userskills"

    type UserSkillsEntity (userName, userSkills : string) =
        inherit TableEntity(partitionKey, userName)
        new() = UserSkillsEntity(null, null)
        member val userSkills : string = userSkills with get, set

    let getUserSkillsTable connectionString =
        let storageAccount = CloudStorageAccount.Parse(connectionString)
        let tableClient = storageAccount.CreateCloudTableClient()
        let table = tableClient.GetTableReference(userSkillsTable)
        table

    let saveUsersSkills connectionString (userSkills : UserSkillsDto) =
        let table = connectionString |> getUserSkillsTable 
        let jsonUserSkills = serializeSkills userSkills
        let insertOperation = TableOperation.InsertOrReplace(new UserSkillsEntity(userSkills.user.name, jsonUserSkills))
        async {
            try
                let! _ = Async.AwaitTask (table.ExecuteAsync(insertOperation))
                return Ok ()
            with
            | exn -> return Error exn
        }

    let read (executeOperation:Task<TableResult>) =
        async {
            let! result = Async.AwaitTask (executeOperation)
            let option = result.Result |> Option.ofObj 
            match option with 
            | None -> return None
            | Some(userSkill) -> 
                let jsonUserSkills = (userSkill :?> UserSkillsEntity).userSkills
                return (deserializeUserSkills jsonUserSkills) |> Some
        }

    let readUsersSkills connectionString userName : Async<UserSkillsDto option> =
        let table = connectionString |> getUserSkillsTable 
        let selectOperation = TableOperation.Retrieve<UserSkillsEntity>(partitionKey, userName)
        let executeOperation = table.ExecuteAsync(selectOperation)
        read executeOperation
