namespace Skills.Infrastructure.Tests

open Skills.Infrastructure.UserSkillEvaluation
open Microsoft.VisualStudio.TestTools.UnitTesting
open Skills.Infrastructure.UserSkillsInterop

module Tests =
    let connectionString = ""

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

            async{
                let! result = ReadUserSkillsAsync connectionString user |> Async.AwaitTask
                Assert.AreEqual(expected, result)
            }       

         [<TestMethod>]
         member this.``Given an existing user When I read its skills Then I get a user skills with its evaluations``() =
             let user = {
                 name = "Tom"
             }

             async{
                let! result = ReadUserSkillsAsync connectionString user |> Async.AwaitTask
                Assert.IsTrue(result.evaluations.Length > 0)
             }       
