namespace Skills.Infrastructure.Tests

open Skills.Infrastructure.Dto
open Microsoft.VisualStudio.TestTools.UnitTesting
open Skills.Infrastructure.UserSkillsInterop
open System

module Tests =
    let connectionString = "UseDevelopmentStorage=true"

    [<TestClass>]
    type TestUserSkillsInterop () =
        //[<TestMethod>]
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
            } |> Async.RunSynchronously

         //[<TestMethod>]
         member this.``Given an existing user When I read its skills Then I get a user skills with its evaluations``() =
             let user = {
                 name = "Tom"
             }

             async{
                let! result = ReadUserSkillsAsync connectionString user |> Async.AwaitTask
                Assert.IsTrue(result.evaluations.Length > 0)
             } |> Async.RunSynchronously       

         //[<TestMethod>]
         member this.``Given a null When I try go get user skills Then I get a null``() =
             let user = {
                 name = null
             }

             async{
                let! result = ReadUserSkillsAsync connectionString user |> Async.AwaitTask
                Assert.AreEqual(null, result)
             } |> Async.RunSynchronously


        [<TestMethod>]
        [<ExpectedException(typeof<ArgumentException>)>]
        member this.``Given an empty connection string When I call ReadUserSkillsAsync with this argument Then I get an exception``() =
            let connectionString = ""
            let _ = ReadUserSkillsAsync connectionString Unchecked.defaultof<_>
            Assert.Fail("Should have throw an exception")


        [<TestMethod>]
        [<ExpectedException(typeof<ArgumentNullException>)>]
        member this.``Given a null userDto When I call ReadUserSkillsAsync with this argument Then I get an exception``() =
            let connectionString = "myValidConnectionString"
            let _ = ReadUserSkillsAsync connectionString Unchecked.defaultof<_>
            Assert.Fail("Should have throw an exception")
