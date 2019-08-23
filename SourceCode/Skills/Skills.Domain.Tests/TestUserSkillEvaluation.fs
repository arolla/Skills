namespace Skills.Domain.Tests

open Skills.Domain.UserSkillEvaluation
open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type TestUserSkillEvaluation () =

    [<TestMethod>]
    member this.``Given an evaluation and a user When Add evaluation to the user given Then return user skills with the evaluation added `` () =
            
        let evaluation = {
            skill = Skill "fsharp"
            date = EvaluationDate(DateTime(2019, 11, 02))
            level = Level 3
        }
        let user = {
            name = "Jack"
        }

        let userSkills = addEvaluation evaluation user
        Assert.AreEqual(userSkills.user, user)
        let exists =
            userSkills.evaluations
            |> List.contains evaluation
        //let exists = List.contains evaluation userSkills.evaluations 
        Assert.IsTrue(exists)
