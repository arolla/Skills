namespace Skills.Infrastructure.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open Skills.Infrastructure
open Skills.Infrastructure.EventSender

[<TestClass>]
type TestEventSender () =
    //[<TestMethod>]
    member this.``Given an evaluation dto when I send the event then it should be enqueued``() =
        let now = DateTime.Now
        let eventToEnqueue:EvaluationAddedDto = {
            date = now
            data = """{"evaluation":{"date":"2019-08-30T00:00:00","level":4,"skill":"poterie"},"user":{"name":"Machin"}}"""
            eventType = "EvaluationAdded"
        }

        let connectionString = ""
        let result = sendEvent connectionString eventToEnqueue |> Async.RunSynchronously
        match result with
        | Error error -> Assert.Fail(error.Message)
        | Ok _ -> Assert.IsTrue(true, "Test pass ! \o/")
    