namespace Skills.Infrastructure

open System
open Skills.Infrastructure.UserSkillEvaluation
open Microsoft.WindowsAzure.Storage.Table

module UserSkillsRepo =

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
