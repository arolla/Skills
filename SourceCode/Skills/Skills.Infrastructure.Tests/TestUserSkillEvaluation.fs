namespace Skills.Infrastructure.Tests

open System
open Skills.Domain.UserSkillEvaluation
open Skills.Infrastructure.UserSkillEvaluation
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type TestUserSkillEvaluation () =
    [<TestMethod>]
    member this.``Given many users skills When I would convert them DTO Then they are serializable``() =
        let usersSkills : UserSkills list = [
            {
                user = {
                    name = "Tom"
                }
                evaluations = [
                    {
                        skill = Skill "csharp"
                        date = EvaluationDate(DateTime(2019, 08,23))
                        level = Level 3
                    }
                ]
            }
            {
                user = {
                    name = "Jack"
                }
                evaluations = [
                    {
                        skill = Skill "fsharp"
                        date = EvaluationDate(DateTime(2019, 08,23))
                        level = Level 3
                    }
                ]
            }
        ]

        let convertedSkills = convertSkills usersSkills

        let expectedConvertedSkills : UserSkillsDto list = [
           {
               user = {
                    name = "Tom"
               }
               evaluations = [
                   {
                       skill = "csharp"
                       date = DateTime(2019, 08,23)
                       level = 3
                   }
               ]
           }
           {
               user = {
                    name = "Jack"
               }
               evaluations = [
                   {
                       skill = "fsharp"
                       date = DateTime(2019, 08,23)
                       level = 3
                   }
               ]
           }
        ]

        Assert.AreEqual(expectedConvertedSkills, convertedSkills)

    [<TestMethod>]
    member this.``Given serialized user skills When I would convert them to domain entities Then I get domain user skills``() =

        let userSkillsDto : UserSkillsDto list = [
            {
                user = {
                    name = "Tom"
                }
                evaluations = [
                    {
                        skill = "csharp"
                        date = DateTime(2019, 08,23)
                        level = 3
                    }
                ]
            }
            {
                user = {
                    name = "Jack"
                }
                evaluations = [
                    {
                        skill = "fsharp"
                        date = DateTime(2019, 08,23)
                        level = 3
                    }
                ]
            }
        ]

        let convertedSkills = convertDtoSkills userSkillsDto

        let expectedUserSkills : UserSkills list = [
            {
                user = {
                    name = "Tom"
                }
                evaluations = [
                    {
                        skill = Skill "csharp"
                        date = EvaluationDate(DateTime(2019, 08,23))
                        level = Level 3
                    }
                ]
            }
            {
                user = {
                    name = "Jack"
                }
                evaluations = [
                    {
                        skill = Skill "fsharp"
                        date = EvaluationDate(DateTime(2019, 08,23))
                        level = Level 3
                    }
                ]
            }
        ]

        Assert.AreEqual(expectedUserSkills, convertedSkills)

    [<TestMethod>]
    member this.``Given many users skills When I would save them Then they are serialized in a json content``() =
        let jack = {
            name = "Jack"
        }
        let tom = {
            name = "Tom"
        }
        let usersSkills = [
            {
                user = tom
                evaluations = [
                    {
                        skill = "csharp"
                        date = DateTime(2019, 08,23)
                        level = 3
                    }
                ]
            }
            {
                user = jack
                evaluations = [
                    {
                        skill = "fsharp"
                        date = DateTime(2019, 08,23)
                        level = 3
                    }
                ]
            }
        ]

        let jsonContent = serializeSkills usersSkills

        let expectedJson = @"[{""user"":{""name"":""Tom""},""evaluations"":[{""skill"":""csharp"",""date"":""2019-08-23T00:00:00"",""level"":3}]},{""user"":{""name"":""Jack""},""evaluations"":[{""skill"":""fsharp"",""date"":""2019-08-23T00:00:00"",""level"":3}]}]"
        Assert.AreEqual(expectedJson, jsonContent)

    [<TestMethod>]
    member this.``Given a json content When I would read its content Then I obtain user skills``() =
        let jsonContent = @"[{""user"":{""name"":""Tom""},""evaluations"":[{""skill"":""csharp"",""date"":""2019-08-23T00:00:00"",""level"":3}]},{""user"":{""name"":""Jack""},""evaluations"":[{""skill"":""fsharp"",""date"":""2019-08-23T00:00:00"",""level"":3}]}]"
        let usersSkills = deserializeSkills jsonContent 

        let expectedUserSkills = [
            {
                user = {
                    name = "Tom"
                }
                evaluations = [
                    {
                        skill = "csharp"
                        date = DateTime(2019, 08,23)
                        level = 3
                    }
                ]
            }
            {
                user = {
                    name = "Jack"
                }
                evaluations = [
                    {
                        skill = "fsharp"
                        date = DateTime(2019, 08,23)
                        level = 3
                    }
                ]
            }
        ]

        Assert.AreEqual(expectedUserSkills, usersSkills)

    [<TestMethod>]
    member this.``Given a user and an evaluation When I would add the evaluation to the user skills Then they are persisted``() =
        let jack:User = {
            name = "Jack"
        }
        let evaluation:Evaluation = {
            skill = Skill "csharp"
            date = EvaluationDate(DateTime(2019, 08,23))
            level = Level 3
        }

        let skills : UserSkills list = [
            {
                user = jack
                evaluations = [
                    evaluation
                ]
        }]

        let expected = convertSkills skills

        let readSkills () = "[]"
            

        let saveSkills skills =
            Assert.AreEqual(expected, skills)

        addEvaluation readSkills saveSkills jack evaluation |> ignore

    // [<TestMethod>]
    member this.``Given saved user skills When I read user skills Then I get the saved ones``() =
        let tomName = "Tom"
        let userSkillsToSave = {
            user = {
                    name = tomName
            }
            evaluations = [
                {
                    skill = "csharp"
                    date = DateTime(2019, 08,23)
                    level = 3
                }
            ]
        }

        let connectionString = getConnectionString()
        saveUsersSkills connectionString userSkillsToSave |> ignore
        let usersSkills = readUsersSkills connectionString tomName

        Assert.AreEqual(tomName, usersSkills.user.name)
        ()