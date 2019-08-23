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

        let userSkills = {
            user = user
            evaluations = []
        }

        let modifiedUserSkills = addEvaluation evaluation userSkills
        Assert.AreNotSame(modifiedUserSkills, userSkills)
        Assert.AreEqual(modifiedUserSkills.user, user)
        let exists =
            modifiedUserSkills.evaluations
            |> List.contains evaluation
        //let exists = List.contains evaluation userSkills.evaluations 
        Assert.IsTrue(exists)

    [<TestMethod>]
    member this.``Given no user skills When I would find the user skills Then I get no evaluation for this user``() =
        let jack = {
            name = "Jack"
        }

        let usersSkills = []

        let jackSkills = findSkills jack usersSkills

        Assert.AreEqual(jack.name, jackSkills.user.name)
        Assert.AreEqual(0, jackSkills.evaluations.Length)

        