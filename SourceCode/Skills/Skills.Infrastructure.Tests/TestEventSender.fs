﻿namespace Skills.Infrastructure.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open Skills.Infrastructure.EventStore
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
        sendEvent connectionString eventToEnqueue |> ignore
    