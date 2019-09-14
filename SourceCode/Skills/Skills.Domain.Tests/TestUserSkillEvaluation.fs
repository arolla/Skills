namespace Skills.Domain.Tests

open Skills.Domain.UserSkillEvaluation
open Skills.Domain
open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type TestUserSkillEvaluation () =

    let userName name =
        match UserName.create name with
        | Error _ -> "Unable to create a username" |> failwith
        | Ok username -> username

    let level level =
        match Level.create level with
        | Error _ -> "Unable to create a level" |> failwith
        | Ok level -> level
  
    let skill skill =
        match Skill.create skill with
        | Error _ -> "Unable to create a skill" |> failwith
        | Ok skill -> skill  

    [<TestMethod>]
    member this.``Given an evaluation and a user When Add evaluation to the user given Then return user skills with the evaluation added `` () =
            
        let evaluation = {
            skill = skill "fsharp"
            date = DateTime(2019, 11, 02) |> EvaluationDate.create 
            level = level 3
        }
        let user = {
            name = userName "Jack"
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
            name = userName "Jack"
        }

        let usersSkills = []

        let jackSkills = findSkills jack usersSkills

        Assert.AreEqual(jack.name, jackSkills.user.name)
        Assert.AreEqual(0, jackSkills.evaluations.Length)
                
    [<TestMethod>]
    member this.``Given skills of my user When I would find my user skills Then I get the existing user skills``() =
        let jack = {
            name = userName "Jack"
        }

        let usersSkills = [
            {
                user = jack
                evaluations = [
                    {
                        skill = skill "fsharp"
                        date = DateTime(2019, 08, 23) |> EvaluationDate.create
                        level = level 3
                    }
                ]
            }
        ]

        let jackSkills = findSkills jack usersSkills

        Assert.AreEqual(usersSkills.Head, jackSkills)
        
    [<TestMethod>]
    member this.``Given skills of other user When I would find my user skills Then I get no skills``() =
        let jack = {
            name = userName "Jack"
        }

        let tom = {
            name = userName "Tom"
        }

        let usersSkills = [
            {
                user = tom
                evaluations = [
                    {
                        skill = skill "fsharp"
                        date = DateTime(2019, 08,23) |> EvaluationDate.create
                        level = level 3
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
            name = userName "Jack"
        }
        
        let jackSkills = {
            user = jack
            evaluations = [
                {
                    skill = skill "fsharp"
                    date = DateTime(2019, 08,23) |> EvaluationDate.create
                    level = level 3
                }
            ]
        }

        let tom = {
            name = userName "Tom"
        }

        let usersSkills = [
            {
                user = tom
                evaluations = [
                    {
                        skill = skill "fsharp"
                        date = DateTime(2019, 08,23) |> EvaluationDate.create
                        level = level 3
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
            name = userName jackName
        }
        let evaluation:Evaluation = {
            skill = skill "csharp"
            date = DateTime(2019, 08,23) |> EvaluationDate.create
            level = level 3
        }
        let expectedUserSkills : UserSkills = {
                user = {
                    name = userName jackName
                }
                evaluations = [
                    evaluation
                ]
        }
        let readSkills jack = async {
                return Ok {
                    user = jack
                    evaluations = List.empty
                }       
            }
        let saveSkills skills = async {
                Assert.AreEqual(expectedUserSkills, skills)
                return Ok ()
            }
       
        addEvaluation readSkills saveSkills jack evaluation |> ignore

    [<TestMethod>]
    member this.``Given an error skills read When I would add the evaluation to the user skills Then Error should be returned``() =
        let jack:User = {
            name = userName "Jack"
        }
        let evaluation:Evaluation = {
            skill = skill "csharp"
            date = DateTime(2019, 08,23) |> EvaluationDate.create
            level = level 3
        }
        let readSkills jack = async{
                return
                    sprintf "Problem when reading skills of %A" jack
                    |> List.singleton
                    |> Error
            }
   
        let saveSkills _ =
            async{return Ok ()}
   
        async{
            match! addEvaluation readSkills saveSkills jack evaluation with 
            | Error (AddEvaluationError.ReadUserSkillsErrors messages) -> 
                let expectedMessage = 
                    sprintf 
                        "Problem when reading skills of %A"
                        jack
                let result = messages |> List.exists (fun m -> m = expectedMessage)
                Assert.IsTrue(result)
            | _ -> Assert.Fail "Should not add an evaluation to the user"
        } |> Async.RunSynchronously


    [<TestMethod>]
    member this.``Given an error on skills save When I would add the evaluation to the user skills Then Error should be returned``() =
        let jack:User = {
            name = userName "Jack"
        }
        let evaluation:Evaluation = {
            skill = skill "csharp"
            date = DateTime(2019, 08,23) |> EvaluationDate.create
            level = level 3
        }
        let expectedExceptionMessage = "My amazing expected exception message"
        let readSkills jack = async {
                return Ok {
                    user = jack
                    evaluations = List.empty
                }       
            }
   
        let saveSkills _ =
            async{return expectedExceptionMessage |> exn |> Error}
   
        async{
                match! addEvaluation readSkills saveSkills jack evaluation with 
                | Error (AddEvaluationError.SaveException exn) -> 
                    Assert.AreEqual(expectedExceptionMessage, exn.Message)
                | _ -> Assert.Fail "Should not add an evaluation to the user"
        } |> Async.RunSynchronously


       [<TestMethod>]
       member this.``Given an error skills save When I would add the evaluation to the user skills Then Error should be returned``() =
           let jack:User = {
               name = userName "Jack"
           }
           let evaluation:Evaluation = {
               skill = skill "csharp"
               date = DateTime(2019, 08,23) |> EvaluationDate.create
               level = level 3
           }
           let readSkills jack = async{
                   return Ok {
                       user = jack
                       evaluations = List.empty
                   }
                }
           let saveSkills userSkill = async{
                   return
                       sprintf "Problem when saving user skills of %A" userSkill
                       |> exn
                       |> Error
               }
           let expectedUserSkillsToSave = {
               user = jack
               evaluations = [evaluation]
           }

           async{
               let! result = addEvaluation readSkills saveSkills jack evaluation
               match result with 
               | Error message -> 
                   let expectedMessage = 
                       sprintf 
                           "Problem when saving user skills of %A"
                           expectedUserSkillsToSave
                   Assert.AreEqual(expectedMessage, message)
               | Ok _ -> Assert.Fail "Should not add an evaluation to the user"

           }