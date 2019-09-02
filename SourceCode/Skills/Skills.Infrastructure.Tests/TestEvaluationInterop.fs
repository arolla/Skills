namespace Skills.Infrastructure.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open Skills.Infrastructure.EventStore
open Skills.Infrastructure.EvaluationInterop

[<TestClass>]
type TestEvaluationInterop () =
    [<TestMethod>]
    member this.``Given an evaluation added dto event when I ``() =
        let now = DateTime.Now
        let event:EvaluationAddedDto = {
            date = now
            data = """{"evaluation":{"date":"2019-08-30T00:00:00","level":4,"skill":"poterie"},"user":{"name":"Machin"}}"""
            eventType = "EvaluationAdded"
        }

        let userSkill = GetUserSkillFromEvent event 
        Assert.AreEqual("Machin", userSkill.user.name)
        Assert.AreEqual(DateTime(2019,08,30), userSkill.evaluation.date)
        Assert.AreEqual(4, userSkill.evaluation.level)
        Assert.AreEqual("poterie", userSkill.evaluation.skill)

    