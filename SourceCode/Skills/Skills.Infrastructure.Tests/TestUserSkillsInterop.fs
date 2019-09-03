namespace Skills.Infrastructure.Tests

open Skills.Infrastructure.UserSkillEvaluation
open Microsoft.VisualStudio.TestTools.UnitTesting
open Skills.Infrastructure.UserSkillsInterop

[<TestClass>]
type TestUserSkillsInterop () =
    [<TestMethod>]
    member this.``Given a user that doesn't exist When I read its skills Then I get a user skills with no evaluation``() =
        let user = {
            name = "azerzdgf"
        }
        
        let expected = {
            user = {
                name = user.name
            }
            evaluations = [||]
        }

        let connectionString = ""

        let result = ReadUserSkills connectionString user

        Assert.AreEqual(expected, result)


    [<TestMethod>]
     member this.``Given an existing user When I read its skills Then I get a user skills with its evaluations``() =
         let user = {
             name = "Tom"
         }
     

         let connectionString = ""

         let result = ReadUserSkills connectionString user

         Assert.IsTrue(result.evaluations.Length > 0)
       