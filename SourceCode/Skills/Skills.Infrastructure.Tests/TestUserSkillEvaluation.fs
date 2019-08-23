namespace Skills.Infrastructure.Tests

open System
open Skills.Domain.UserSkillEvaluation
open Skills.Infrastructure.UserSkillEvaluation
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type TestClass () =

    
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
    member this.``Given many users skills When I would save them Then they are serialized in a json file``() =
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
