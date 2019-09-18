namespace Skills.Infrastructure

open Skills.Infrastructure.Dto
open Skills.Domain

module UserDto =

    let toDomain (dto:UserDto) =
        User.create dto.name