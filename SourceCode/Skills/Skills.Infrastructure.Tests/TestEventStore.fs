namespace Skills.Infrastructure.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open Skills.Domain.Event
open Skills.Infrastructure.EventStore

[<TestClass>]
type TestEventStore () =
    [<TestMethod>]
    member this.``Given an evaluation added event When I add it to the store Then it should be saved``() =
        let now = DateTime.Now

        let eventToSave:EvaluationAddedDto = {
            date = now
            data = """{"evaluation":{"date":"2019-08-30T00:00:00","level":4,"skill":"poterie"},"user":{"name":"Machin"}}"""
            eventType = "EvaluationAdded"
        }

        let save event =
            Assert.AreEqual(eventToSave, event)

        addEvent save eventToSave


    [<TestMethod>]
    member this.``Given an evaluation added domain event When I convert it to Dto event Then the dto should contains the serialized domain event``() =
        let now = DateTime.Now
        let evaluationAddedEvent:EvaluationAdded = {
            date = EventDate(now)
            user = {name = "Machin"}
            evaluation = {
                skill = Skill "poterie"
                level = Level 4
                date = EvaluationDate(DateTime(2019, 08, 30))
            }
        }

        let expectedEvent:EvaluationAddedDto = {
            date = now
            data = """{"evaluation":{"date":"2019-08-30T00:00:00","level":4,"skill":"poterie"},"user":{"name":"Machin"}}"""
            eventType = "EvaluationAdded"
        }

        let result = convertToDto evaluationAddedEvent
        Assert.AreEqual(expectedEvent, result)