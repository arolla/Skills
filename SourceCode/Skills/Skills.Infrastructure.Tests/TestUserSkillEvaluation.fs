namespace Skills.Infrastructure.Tests

open System
open Skills.Domain
open Skills.Infrastructure.UserSkillEvaluation
open Microsoft.VisualStudio.TestTools.UnitTesting
open Skills.Infrastructure
open Skills.Infrastructure.Dto
open Helpers
open Skills.Domain.Types

[<TestClass>]
type TestUserSkillEvaluation () =
    [<TestMethod>]
    member this.``Given many users skills When I would convert them DTO Then they are serializable``() =
        let usersSkills : UserEvaluations = 
            {
                user = {
                    name = Helpers.userName "Tom"
                }
                evaluations = [
                    {
                        skill = skill "csharp"
                        date = DateTime(2019, 08,23) |> EvaluationDate.create
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

        let expectedUserSkills : Result<UserEvaluations, string list> = 
            let userSkills:UserEvaluations = {
                user = {
                    name = Helpers.userName "Tom"
                }
                evaluations = [
                    {
                        skill = skill "csharp"
                        date = DateTime(2019, 08,23) |> EvaluationDate.create
                        level = level 3
                    }
                ]
            }
            userSkills |> Ok

        Assert.AreEqual(expectedUserSkills, convertedSkills)

    
    [<TestMethod>]
       member this.``Given EvaluationAdded dto event When I add a new evaluation Then this lastest is added to the user skills``() =
           let event = {
               date = DateTime(2019, 09, 09)
               eventType = "EvaluationAddedDto"
               data = """{"evaluation":{"date":"2019-08-30T00:00:00","level":3,"skill":"csharp"},"user":{"name":"Jack"}}""" // UserSkillDto
           }       
           let jackName = "Jack"
           let evaluation:Evaluation = {
               skill = skill "csharp"
               date = DateTime(2019, 08, 30) |> EvaluationDate.create
               level = level 3
           }
           let expectedUserSkills : UserEvaluations = {
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
               let userSkills : UserEvaluations = {
                   user = user
                   evaluations = List.empty
               }
               return Ok userSkills
           }
           
           addEvaluation readUserSkills saveUserSkills event |> Async.RunSynchronously
