namespace Skills.Infrastructure.Tests

open System
open Skills.Domain.UserSkillEvaluation
open Skills.Domain
open Skills.Infrastructure.UserSkillEvaluation
open Microsoft.VisualStudio.TestTools.UnitTesting
open Skills.Infrastructure
open Skills.Infrastructure.Dto
open Helpers

[<TestClass>]
type TestUserSkillEvaluation () =
    [<TestMethod>]
    member this.``Given many users skills When I would convert them DTO Then they are serializable``() =
        let usersSkills : UserSkills = 
            {
                user = {
                    name = Helpers.userName "Tom"
                }
                evaluations = [
                    {
                        skill = Skill "csharp"
                        date = EvaluationDate(DateTime(2019, 08,23))
                        level = level 3
                    }
                ]
            }

        let convertedSkills = convertSkills usersSkills

        let expectedConvertedSkills : UserSkillsDto = 
           {
               user = {
                    name = "Tom"
               }
               evaluations = [|
                   {
                       skill = "csharp"
                       date = DateTime(2019, 08,23)
                       level = 3
                   }
               |]
           }

        Assert.AreEqual(expectedConvertedSkills, convertedSkills)

    [<TestMethod>]
    member this.``Given serialized user skills When I would convert them to domain entities Then I get domain user skills``() =

        let userSkillsDto : UserSkillsDto = 
            {
                user = {
                    name = "Tom"
                }
                evaluations = [|
                    {
                        skill = "csharp"
                        date = DateTime(2019, 08,23)
                        level = 3
                    }
                |]
            }

        let convertedSkills = convertDtoSkills userSkillsDto

        let expectedUserSkills : Result<UserSkills, string list> = 
            let userSkills:UserSkills = {
                user = {
                    name = Helpers.userName "Tom"
                }
                evaluations = [
                    {
                        skill = Skill "csharp"
                        date = EvaluationDate(DateTime(2019, 08,23))
                        level = level 3
                    }
                ]
            }
            userSkills |> Ok

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
                evaluations = [|
                    {
                        skill = "csharp"
                        date = DateTime(2019, 08,23)
                        level = 3
                    }
                |]
            }
            {
                user = jack
                evaluations = [|
                    {
                        skill = "fsharp"
                        date = DateTime(2019, 08,23)
                        level = 3
                    }
                |]
            }
        ]

        let jsonContent = serializeSkills usersSkills

        let expectedJson = """[{"user":{"name":"Tom"},"evaluations":[{"skill":"csharp","date":"2019-08-23T00:00:00","level":3}]},{"user":{"name":"Jack"},"evaluations":[{"skill":"fsharp","date":"2019-08-23T00:00:00","level":3}]}]"""
        Assert.AreEqual(expectedJson, jsonContent)

    
    [<TestMethod>]
       member this.``Given EvaluationAdded dto event When I add a new evaluation Then this lastest is added to the user skills``() =
           let event = {
               date = DateTime(2019, 09, 09)
               eventType = "EvaluationAddedDto"
               data = """{"evaluation":{"date":"2019-08-30T00:00:00","level":3,"skill":"csharp"},"user":{"name":"Jack"}}""" // UserSkillDto
           }       
           let jackName = "Jack"
           let evaluation:Evaluation = {
               skill = Skill "csharp"
               date = EvaluationDate(DateTime(2019, 08, 30))
               level = level 3
           }
           let expectedUserSkills : UserSkills = {
               user = {
                   name = Helpers.userName jackName
               }
               evaluations = [
                   evaluation
               ]
           }
           let saveUserSkills userSkills = async{
               Assert.AreEqual(expectedUserSkills, userSkills)
               return Ok ()
           }
           let readUserSkills user = async{
               let userSkills : UserSkills = {
                   user = user
                   evaluations = List.empty
               }
               return Ok userSkills
           }
           
           addEvaluation readUserSkills saveUserSkills event |> Async.RunSynchronously
