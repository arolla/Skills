namespace Skills.Domain

open System

type EventDate = private EventDate of DateTime

module EventDate =

    let create date = EventDate date

    let value eventDate =
        let (EventDate value) = eventDate
        value
