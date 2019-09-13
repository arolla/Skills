namespace Skills.Infrastructure.Tests

open System
open Skills.Domain.UserSkillEvaluation
open Skills.Domain
open Skills.Infrastructure.Dto
open Microsoft.VisualStudio.TestTools.UnitTesting
open Skills.Infrastructure.UserSkillDto
open Helpers

[<TestClass>]
type TestUserSkillDto () =
    [<TestMethod>]
    member this.``Given a user skill dto with empty name when I convert it to domain type then I get an error``() =
        let userName = ""
        let userSkillDto = {
            user = { name = userName }
            evaluation = {
                skill = "fsharp"
                date = DateTime(2019, 09, 09)
                level = 3
            }
        }
        let userSkillResult = toDomain userSkillDto
           
        match userSkillResult with
        | Ok(userskill) -> 
            let errorMessage = sprintf "Conversion to domain should fail : %A" userskill
            Assert.Fail(errorMessage)
        | _ -> 
            Assert.IsTrue(true)

    [<TestMethod>]
    member this.``Given a user skill dto with empty skill when I convert it to domain type then I get an error``() =
        let userName = "Jack"
        let userSkillDto = {
            user = { name = userName }
            evaluation = {
                skill = ""
                date = DateTime(2019, 09, 09)
                level = 3
            }
        }
        let userSkillResult = toDomain userSkillDto
           
        match userSkillResult with
        | Ok(userskill) -> 
            let errorMessage = sprintf "Conversion to domain should fail : %A" userskill
            Assert.Fail(errorMessage)
        | _ -> 
            Assert.IsTrue(true)

    [<TestMethod>]
    member this.``Given a user skill dto with level with negative value when I convert it to domain type then I get an error``() =
        let userName = "Jack"
        let userSkillDto = {
            user = { name = userName }
            evaluation = {
                skill = "java"
                date = DateTime(2019, 09, 09)
                level = -1
            }
        }
        let userSkillResult = toDomain userSkillDto
              
        match userSkillResult with
        | Ok(userskill) -> 
            let errorMessage = sprintf "Conversion to domain should fail : %A" userskill
            Assert.Fail(errorMessage)
        | _ -> 
            Assert.IsTrue(true)

    [<TestMethod>]
    member this.``Given a user skill dto with level greater than five when I convert it to domain type then I get an error``() =
        let userName = "Jack"
        let userSkillDto = {
            user = { name = userName }
            evaluation = {
                skill = "java"
                date = DateTime(2019, 09, 09)
                level = 6
            }
        }
        let userSkillResult = toDomain userSkillDto
          
        match userSkillResult with
        | Ok(userskill) -> 
            let errorMessage = sprintf "Conversion to domain should fail : %A" userskill
            Assert.Fail(errorMessage)
        | _ -> 
            Assert.IsTrue(true)

    [<TestMethod>]
    member this.``Given a user skill dto with valid skill level and date When I convert it to domain type then I get an evaluation``() =
        let userName = "Jack"
        let userSkillDto = {
            user = { name = userName }
            evaluation = {
                skill = "java"
                date = DateTime(2019, 09, 09)
                level = 4
            }
        }
        let userSkillResult = toDomain userSkillDto
      
        match userSkillResult with
        | Error(userskill) -> 
            let errorMessage = sprintf "Conversion to domain should succeed : %A" userskill
            Assert.Fail(errorMessage)
        | Ok _ -> 
            Assert.IsTrue(true)

    [<TestMethod>]
    member this.``Given a user skill dto when I convert it to domain type then I get the UserSkill domain type``() =
        let userName = "Jack"
        let userSkillDto = {
            user = { name = userName }
            evaluation = {
                skill = "fsharp"
                date = DateTime(2019, 09, 09)
                level = 3
            }
        }
        let userSkillResult = toDomain userSkillDto
        let expectedUserSkill : UserSkill = {
            user = {name = Helpers.userName userName }
            evaluation = {
                skill = Skill "fsharp"
                date = EvaluationDate( DateTime(2019, 09, 09))
                level = level 3
            }
        }

        match userSkillResult with
        | Ok(userskill) -> Assert.AreEqual(expectedUserSkill, userskill)
        | _ -> 
            let errorMessage = sprintf "Conversion to domain failed, expected %A" expectedUserSkill
            Assert.Fail(errorMessage)
        ()
