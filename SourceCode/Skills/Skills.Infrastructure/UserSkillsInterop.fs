namespace Skills.Infrastructure

open UserSkillsRepo
open UserSkillEvaluation
open Skills.Domain.UserSkillEvaluation
open System.Threading.Tasks

module UserSkillsInterop =

    let ReadUserSkillsAsync connectionString userDto =
        if System.String.IsNullOrWhiteSpace(connectionString) then invalidArg "connectionString" "Must not be null, empty or whitespace"
        if userDto = Unchecked.defaultof<UserDto> then nullArg "userDto"

        let readUserSkills user =
            let username = UserName.value user.name

            async {
                match! readUsersSkills connectionString username with
                | Some userSkills   -> return userSkills
                | None              -> return {user = userDto; evaluations = [||]}
            } |> Async.StartImmediateAsTask

        match User.fromDto userDto with
        | Ok user -> readUserSkills user
        | Error _ -> Task.FromResult Unchecked.defaultof<UserSkillsDto>