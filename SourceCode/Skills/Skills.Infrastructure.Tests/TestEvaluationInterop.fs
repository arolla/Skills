namespace Skills.Infrastructure.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open Skills.Infrastructure.EvaluationInterop
open Skills.Infrastructure

[<TestClass>]
type TestEvaluationInterop () =
    [<TestMethod>]
    [<ExpectedException(typeof<ArgumentException>)>]
    member this.``Given an empty connection string When I call AddEvaluationAddedEventAsync with this argument Then I get an exception``() =
        let connectionString = ""
        let _ = AddEvaluationAddedEventAsync connectionString Unchecked.defaultof<_>
        Assert.Fail("Should have throw an exception")


    [<TestMethod>]
    [<ExpectedException(typeof<ArgumentException>)>]
    member this.``Given a null connection string When I call AddEvaluationAddedEventAsync with this argument Then I get an exception``() =
        let connectionString = Unchecked.defaultof<_>
        let _ = AddEvaluationAddedEventAsync connectionString Unchecked.defaultof<_>
        Assert.Fail("Should have throw an exception")


    [<TestMethod>]
    [<ExpectedException(typeof<ArgumentNullException>)>]
    member this.``Given a null user evaluation When I call AddEvaluationAddedEventAsync with this argument Then I get an exception``() =
        let connectionString = "MyValidConnectionString"
        let _ = AddEvaluationAddedEventAsync connectionString Unchecked.defaultof<_>
        Assert.Fail("Should have throw an exception")


    [<TestMethod>]
    [<ExpectedException(typeof<ArgumentNullException>)>]
    member this.``Given a user evaluation with a null user property When I call AddEvaluationAddedEventAsync with this argument Then I get an exception``() =
        let connectionString = "MyValidConnectionString"
        let event : DatedUserEvaluationDto = {
            user = Unchecked.defaultof<_>
            evaluation = {skill = "skill"; date = DateTime.Now; level = 0}
            date = DateTime.Now
        }
        let _ = AddEvaluationAddedEventAsync connectionString event
        Assert.Fail("Should have throw an exception")


    [<TestMethod>]
    [<ExpectedException(typeof<ArgumentNullException>)>]
    member this.``Given a user evaluation with a null evaluation property When I call AddEvaluationAddedEventAsync with this argument Then I get an exception``() =
        let connectionString = "MyValidConnectionString"
        let event : DatedUserEvaluationDto = {
            user = {name = "username"}
            evaluation = Unchecked.defaultof<_>
            date = DateTime.Now
        }
        let _ = AddEvaluationAddedEventAsync connectionString event
        Assert.Fail("Should have throw an exception")


    [<TestMethod>]
    [<ExpectedException(typeof<ArgumentException>)>]
    member this.``Given an empty connection string When I call AddEvaluationAsync with this argument Then I get an exception``() =
        let connectionString = ""
        let _ = AddEvaluationAsync connectionString Unchecked.defaultof<_>
        Assert.Fail("Should have throw an exception")


    [<TestMethod>]
    [<ExpectedException(typeof<ArgumentException>)>]
    member this.``Given a null connection string When I call AddEvaluationAsync with this argument Then I get an exception``() =
        let connectionString = Unchecked.defaultof<_>
        let _ = AddEvaluationAsync connectionString Unchecked.defaultof<_>
        Assert.Fail("Should have throw an exception")


    [<TestMethod>]
    [<ExpectedException(typeof<ArgumentNullException>)>]
    member this.``Given a null event When I call AddEvaluationAsync with this argument Then I get an exception``() =
        let connectionString = "myValidConnectionString"
        let _ = AddEvaluationAsync connectionString Unchecked.defaultof<_>
        Assert.Fail("Should have throw an exception")


    [<TestMethod>]
    [<ExpectedException(typeof<ArgumentException>)>]
    member this.``Given an event with a null data property When I call AddEvaluationAsync with this argument Then I get an exception``() =
        let connectionString = "myValidConnectionString"
        let event = {
            data = Unchecked.defaultof<_>
            eventType = "eventType"
            date = DateTime.Now
        }
        let _ = AddEvaluationAsync connectionString event
        Assert.Fail("Should have throw an exception")


    [<TestMethod>]
    [<ExpectedException(typeof<ArgumentException>)>]
    member this.``Given an event with a null event type property When I call AddEvaluationAsync with this argument Then I get an exception``() =
        let connectionString = "myValidConnectionString"
        let event = {
            data = "data"
            eventType = Unchecked.defaultof<_>
            date = DateTime.Now
        }
        let _ = AddEvaluationAsync connectionString event
        Assert.Fail("Should have throw an exception")
