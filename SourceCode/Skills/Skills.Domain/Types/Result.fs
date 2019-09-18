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

    let arrayOfResultToResultList (array: Result<'a, 'b> []) =
        let folder acc res =
            match acc, res with
            | Ok evals, Ok(eval)        -> Ok (eval :: evals)
            | Ok _, Error(error)        -> Error([error])
            | Error errors, Ok _        -> Error errors
            | Error errors, Error error -> Error(error::errors)

        Array.fold folder (Ok([])) array