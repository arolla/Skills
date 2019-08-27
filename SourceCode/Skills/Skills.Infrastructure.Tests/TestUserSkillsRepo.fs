namespace Skills.Infrastructure.Tests

open System
open Skills.Infrastructure.UserSkillEvaluation
open Skills.Infrastructure.UserSkillsRepo
open Microsoft.VisualStudio.TestTools.UnitTesting

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

        let connectionString = getConnectionString()
        saveUsersSkills connectionString userSkillsToSave |> ignore
        let usersSkills = readUsersSkills connectionString tomName

        Assert.AreEqual(tomName, usersSkills.user.name)
        ()