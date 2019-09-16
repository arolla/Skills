namespace Skills.Infrastructure.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open Skills.Domain.Event
open Skills.Infrastructure
open Skills.Infrastructure.EventStore
open Skills.Domain
open Skills.Domain.EventDate
open Helpers

[<TestClass>]
type TestEventStore () =
    [<TestMethod>]
    member this.``Given an evaluation added event When I add it to the store Then it should be saved``() =
        let now = DateTime.Now

        let eventToSave:EvaluationAddedDto = {
            date = now
            data = """{"user":{"name":"Machin"},"evaluation":{"skill":"poterie","date":"2019-08-30T00:00:00","level":4}}"""
            eventType = "EvaluationAdded"
        }

        let save event = async{
            Assert.AreEqual(eventToSave, event)
            return Ok()
        }
        let enqueue _ = async{return Ok()}
    
        addEvent save enqueue eventToSave

    [<TestMethod>]
    member this.``Given an evaluation added domain event When I convert it to Dto event Then the dto should contains the serialized domain event``() =
        let now = DateTime.Now
        let evaluationAddedEvent:EvaluationAdded = {
            date = EventDate.create now
            user = {name = Helpers.userName "Machin"}
            evaluation = {
                skill = skill "poterie"
                level = level 4
                date = DateTime(2019, 08, 30) |> EvaluationDate.create
            }
        }

        let expectedEvent:EvaluationAddedDto = {
            date = now
            data = """{"user":{"name":"Machin"},"evaluation":{"skill":"poterie","date":"2019-08-30T00:00:00","level":4}}"""
            eventType = "EvaluationAdded"
        }

        let result = convertToDto evaluationAddedEvent
        Assert.AreEqual(expectedEvent, result)
   
    [<TestMethod>]
    member this.``Given an evaluation added event When I add it to the store Then it should be enqueued``() =
        let now = DateTime.Now

        let eventToSave:EvaluationAddedDto = {
            date = now
            data = """{"user":{"name":"Machin"},"evaluation":{"skill":"poterie","date":"2019-08-30T00:00:00","level":4}}"""
            eventType = "EvaluationAdded"
        }

        let save e = async{return Ok ()}

        let enqueue event = async{
            Assert.AreEqual(eventToSave, event)
            return Ok()
        }

        addEvent save enqueue eventToSave
