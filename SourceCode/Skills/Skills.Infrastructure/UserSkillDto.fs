namespace Skills.Infrastructure

open Skills.Domain
open Skills.Infrastructure.Dto
open Skills.Domain.Types
open Skills.Domain.Result

module UserSkillDto =

    let toDomain (dto:UserSkillDto) : Result<UserEvaluation, string> =
        result{
            let! user = User.create dto.user.name
            let! evaluation = 
                Evaluation.create
                    dto.evaluation.skill 
                    dto.evaluation.level
                    dto.evaluation.date
            return {user = user; evaluation = evaluation}
        }