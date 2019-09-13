namespace Skills.Infrastructure

open UserSkillsRepo
open UserSkillEvaluation
open Skills.Domain.UserSkillEvaluation
open System.Threading.Tasks
open Skills.Infrastructure.Dto

module UserSkillsInterop =

    let ReadUserSkillsAsync connectionString (userDto : UserDto) =
        if System.String.IsNullOrWhiteSpace(connectionString) then invalidArg "connectionString" "Must not be null, empty or whitespace"

        if isNull(box userDto) then nullArg "userDto"

        let readUserSkills (user : User) =
            let username = UserName.value user.name

            async {
                match! getUsersSkillsFromDb connectionString username with
                | Some userSkills   -> return userSkills
                | None              -> return {user = userDto; evaluations = [||]}
            } |> Async.StartImmediateAsTask

        match User.fromDto userDto with
        | Ok user -> readUserSkills user
        | Error _ -> Task.FromResult Unchecked.defaultof<UserSkillsDto>