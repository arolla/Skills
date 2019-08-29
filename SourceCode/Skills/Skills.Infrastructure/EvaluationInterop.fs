namespace Skills.Infrastructure

open UserSkillEvaluation
open UserSkillsRepo

module EvaluationInterop =

    type UserSkillDto = {
        user : UserDto
        evaluation : EvaluationDto
    }

    let AddEvaluation connectionString (user:UserSkillDto) =
        let readSkills = readUsersSkills connectionString
        let saveSkills = saveUsersSkills connectionString
        addEvaluation readSkills saveSkills user.user user.evaluation
        