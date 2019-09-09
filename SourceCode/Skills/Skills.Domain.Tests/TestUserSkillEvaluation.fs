namespace Skills.Domain.Tests

open Skills.Domain.UserSkillEvaluation
open Skills.Domain
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
            name = UserName "Jack"
        }

        let userSkills = {
            user = user
            evaluations = []
        }

        let modifiedUserSkills = addEvaluationToUserSkills evaluation userSkills
        Assert.AreNotSame(modifiedUserSkills, userSkills)
        Assert.AreEqual(modifiedUserSkills.user, user)
        let exists =
            modifiedUserSkills.evaluations
            |> List.contains evaluation
        Assert.IsTrue(exists)

    [<TestMethod>]
    member this.``Given no user skills When I would find the user skills Then I get no evaluation for this user``() =
        let jack = {
            name = UserName "Jack"
        }

        let usersSkills = []

        let jackSkills = findSkills jack usersSkills

        Assert.AreEqual(jack.name, jackSkills.user.name)
        Assert.AreEqual(0, jackSkills.evaluations.Length)
                
    [<TestMethod>]
    member this.``Given skills of my user When I would find my user skills Then I get the existing user skills``() =
        let jack = {
            name = UserName "Jack"
        }

        let usersSkills = [
            {
                user = jack
                evaluations = [
                    {
                        skill = Skill "fsharp"
                        date = EvaluationDate(DateTime(2019, 08,23))
                        level = Level 3
                    }
                ]
            }
        ]

        let jackSkills = findSkills jack usersSkills

        Assert.AreEqual(usersSkills.Head, jackSkills)
        
    [<TestMethod>]
    member this.``Given skills of other user When I would find my user skills Then I get no skills``() =
        let jack = {
            name = UserName "Jack"
        }

        let tom = {
            name = UserName "Tom"
        }

        let usersSkills = [
            {
                user = tom
                evaluations = [
                    {
                        skill = Skill "fsharp"
                        date = EvaluationDate(DateTime(2019, 08,23))
                        level = Level 3
                    }
                ]
            }
        ]

        let jackSkills = findSkills jack usersSkills

        Assert.AreEqual(jack.name, jackSkills.user.name)
        Assert.AreEqual(0, jackSkills.evaluations.Length)

    [<TestMethod>]
    member this.``Given many users skills (including mine) When I would find my user skills Then I get the existing user skills``() =
        let jack = {
            name = UserName "Jack"
        }
        
        let jackSkills = {
            user = jack
            evaluations = [
                {
                    skill = Skill "fsharp"
                    date = EvaluationDate(DateTime(2019, 08,23))
                    level = Level 3
                }
            ]
        }

        let tom = {
            name = UserName "Tom"
        }

        let usersSkills = [
            {
                user = tom
                evaluations = [
                    {
                        skill = Skill "fsharp"
                        date = EvaluationDate(DateTime(2019, 08,23))
                        level = Level 3
                    }
                ]
            }
            jackSkills
        ]

        let foundJackSkills = findSkills jack usersSkills

        Assert.AreEqual(jackSkills, foundJackSkills)
    
    [<TestMethod>]
       member this.``Given a user and an evaluation When I would add the evaluation to the user skills Then they are persisted``() =
           let jackName = "Jack"
           let jack:User = {
               name = UserName jackName
           }
           let evaluation:Evaluation = {
               skill = Skill "csharp"
               date = EvaluationDate(DateTime(2019, 08,23))
               level = Level 3
           }
           let expectedUserSkills : UserSkills = {
                   user = {
                       name = UserName jackName
                   }
                   evaluations = [
                       evaluation
                   ]
           }
           let readSkills jack = 
               Ok {
                   user = jack
                   evaluations = List.empty
               }       
           let saveSkills skills = 
               Assert.AreEqual(expectedUserSkills, skills)
               Ok ()
       
           addEvaluation readSkills saveSkills jack evaluation |> ignore

       [<TestMethod>]
       member this.``Given an error skills read When I would add the evaluation to the user skills Then Error should be returned``() =
           let jack:User = {
               name = UserName "Jack"
           }
           let evaluation:Evaluation = {
               skill = Skill "csharp"
               date = EvaluationDate(DateTime(2019, 08,23))
               level = Level 3
           }
           let readSkills jack = 
               sprintf "Problem when reading skills of %A" jack
               |> Error
   
           let saveSkills _ =
               Ok ()
   
           let result = addEvaluation readSkills saveSkills jack evaluation
           match result with 
           | Error message -> 
               let expectedMessage = 
                   sprintf 
                       "Problem when reading skills of %A"
                       jack
               Assert.AreEqual(expectedMessage, message)
           | Ok _ -> Assert.Fail "Should not add an evaluation to the user"

       [<TestMethod>]
       member this.``Given an error skills save When I would add the evaluation to the user skills Then Error should be returned``() =
           let jack:User = {
               name = UserName "Jack"
           }
           let evaluation:Evaluation = {
               skill = Skill "csharp"
               date = EvaluationDate(DateTime(2019, 08,23))
               level = Level 3
           }
           let readSkills jack = 
               Ok {
                   user = jack
                   evaluations = List.empty
               }
           let saveSkills userSkill =
               sprintf "Problem when saving user skills of %A" userSkill
               |> Error
           let expectedUserSkillsToSave = {
               user = jack
               evaluations = [evaluation]
           }

           let result = addEvaluation readSkills saveSkills jack evaluation
           match result with 
           | Error message -> 
               let expectedMessage = 
                   sprintf 
                       "Problem when saving user skills of %A"
                       expectedUserSkillsToSave
               Assert.AreEqual(expectedMessage, message)
           | Ok _ -> Assert.Fail "Should not add an evaluation to the user"