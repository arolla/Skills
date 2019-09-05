namespace Skills.Infrastructure.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open Skills.Infrastructure.EventStore
open Skills.Infrastructure.EventRepo

[<TestClass>]
type TestEventRepo () =
    //[<TestMethod>]
    member this.``Given an evaluation added dto event When I save it Then it should persist``() =
        let now = DateTime.Now
        let eventToStore:EvaluationAddedDto = {
            date = now
            data = """{"evaluation":{"date":"2019-08-30T00:00:00","level":4,"skill":"poterie"},"user":{"name":"Machin"}}"""
            eventType = "EvaluationAdded"
        }

        let connectionString = ""
        saveEvent connectionString eventToStore |> Async.RunSynchronously
    