﻿namespace Skills.Infrastructure

open Microsoft.WindowsAzure.Storage
open Skills.Infrastructure.EventRepo
open Microsoft.WindowsAzure.Storage.Queue

module EventSender =

    let eventQueue = "skillseventqueue"

    let getEventQueue connectionString =
        let storageAccount = CloudStorageAccount.Parse(connectionString)
        let queueClient = storageAccount.CreateCloudQueueClient()
        queueClient.GetQueueReference(eventQueue)

    let sendEvent connectionString evaluationAddedDto =
        let queue = connectionString |> getEventQueue
        let jsonEvent = 
            evaluationAddedDto
            |> Json.serialize
        
        let cloudQueueMessage = new CloudQueueMessage(jsonEvent)
        
        async {
            try
                do! Async.AwaitTask (queue.AddMessageAsync(cloudQueueMessage))
                return Ok()
            with
            | ex -> return Error ex
        }


