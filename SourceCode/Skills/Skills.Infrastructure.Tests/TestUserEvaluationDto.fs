namespace Skills.Infrastructure.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open Skills.Infrastructure
open Skills.Domain

[<TestClass>]
type TestUserEvaluationDto () =
    [<TestMethod>]
    member this.``Given a valid user evaluation dto When I convert it to domain type Then I get a valid domain user evaluation``() =
        
        let date = System.DateTime.Now
        let user : Skills.Infrastructure.Dto.UserDto = {name = "myName"}
        let evaluation: Skills.Infrastructure.Dto.EvaluationDto = {
            skill = "skill"
            level = 5
            date = date
        }
        let userSkill = Helpers.userSkill {
            user = user
            evaluation = evaluation
        }
        let expected = EvaluationAdded.create userSkill date

        let userEvaluationDto : DatedUserEvaluationDto = {
            user = user
            evaluation = evaluation
            date = date
        }
        let result = DatedUserEvaluationDto.toDomainEvent userEvaluationDto
        Assert.AreEqual(expected, result)