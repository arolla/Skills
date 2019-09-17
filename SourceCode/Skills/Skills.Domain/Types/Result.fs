namespace Skills.Domain

module Result =

    let retn x = Ok x
    let bind f x =
        match x with
        | Error e -> Error e
        | Ok x -> f x

    type ResultBuilder() =
        member this.Return x = retn x
        member this.Bind(x, f) = bind f x

    let result = new ResultBuilder()