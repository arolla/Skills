namespace Skills.Infrastructure

open Skills.Infrastructure.Dto
open Skills.Domain
open Skills.Domain.Types

module UserDto =

    let toDomain (dto:UserDto) : Result<User, string> =
        User.create dto.name