﻿namespace Skills.Infrastructure

open System
open Microsoft.WindowsAzure.Storage.Table
open Microsoft.WindowsAzure.Storage

module EventRepo =

    let partitionKey = "1"
    let eventStoreTable = "eventstore"

    type EventEntity (data : string) =
        inherit TableEntity(partitionKey, Guid.NewGuid().ToString())
        new() = EventEntity(null)
        member val Data : string = data with get, set

    
    let getEventStoreTable connectionString =
        let storageAccount = CloudStorageAccount.Parse(connectionString)
        let tableClient = storageAccount.CreateCloudTableClient()
        let table = tableClient.GetTableReference(eventStoreTable)
        table

    let saveEvent connectionString evaluationAddedDto =
        let table = connectionString |> getEventStoreTable
        let jsonEvent = 
            evaluationAddedDto
            |> Json.serialize
        let insertOperation = TableOperation.InsertOrReplace(new EventEntity(jsonEvent))
        async {
            try
                let! _ = Async.AwaitTask (table.ExecuteAsync(insertOperation))
                return Ok()
            with
            | exn -> return Error exn
        }

