namespace Skills.Infrastructure

open Skills.Domain.UserSkillEvaluation
open Skills.Infrastructure.Dto

module UserDto =

    let toDomain (dto:UserDto) : Result<User, string> =
        User.create dto.name