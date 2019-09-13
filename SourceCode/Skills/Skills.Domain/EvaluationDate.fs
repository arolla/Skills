namespace Skills.Domain

open System


type EvaluationDate = EvaluationDate  of DateTime

module EvaluationDate =

    let value evaluationDate =
        let (EvaluationDate value) = evaluationDate
        value
