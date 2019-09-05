namespace Skills.Infrastructure.Tests

open System
open Skills.Infrastructure.UserSkillEvaluation
open Skills.Infrastructure.UserSkillsRepo
open Microsoft.VisualStudio.TestTools.UnitTesting
open System.Threading.Tasks
open Microsoft.WindowsAzure.Storage.Table

[<TestClass>]
type TestUserSkillsRepo () =
    //[<TestMethod>]
    member this.``Given saved user skills When I read user skills Then I get the saved ones``() =
        let tomName = "Tom"
        let userSkillsToSave = {
            user = {
                    name = tomName
            }
            evaluations = [|
                {
                    skill = "csharp"
                    date = DateTime(2019, 08,23)
                    level = 3
                }
            |]
        }

        let connectionString = ""

        let usersSkills = readUsersSkills connectionString tomName
        async {
            match! usersSkills with
            | None -> Assert.Fail("Should not be null...")
            | Some us ->
            Assert.AreEqual(tomName, us.user.name)
        } |> Async.RunSynchronously

    [<TestMethod>]
    member this.``Given no userSkill exists When I read skills Then no skill found``() =
        let noSkillsOperation = 
            Task<TableResult>.FromResult(TableResult())
        async {
            let! result = read noSkillsOperation
            Assert.AreEqual(Option.None, result)
        } |> Async.RunSynchronously 