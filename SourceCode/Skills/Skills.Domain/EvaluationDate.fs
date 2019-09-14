namespace Skills.Domain

open System


type EvaluationDate = private EvaluationDate of DateTime

module EvaluationDate =

    let create date =
        EvaluationDate date

    let value evaluationDate =
        let (EvaluationDate value) = evaluationDate
        value
