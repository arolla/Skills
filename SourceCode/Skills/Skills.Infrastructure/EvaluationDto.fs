namespace Skills.Infrastructure

open Skills.Domain
open Skills.Infrastructure.Dto

module EvaluationDto =
        

    let toDomain (dto:EvaluationDto) =
        Evaluation.create dto.skill dto.level dto.date

    let fromDomain (evaluation:Evaluation) =
        {
            skill = Skill.value evaluation.skill
            level = Level.value evaluation.level
            date = EvaluationDate.value evaluation.date
        }