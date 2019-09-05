namespace Skills.Domain

open System

module UserSkillEvaluation =
    
    type Level = Level of int   
    
    type Skill = Skill of string 
    
    type EvaluationDate = EvaluationDate  of DateTime
        
    type Evaluation = {
        skill : Skill
        date : EvaluationDate
        level : Level
    }
    
    type UserName = UserName of string

    type User = {
        name : UserName
    }
    
    type UserSkills = {
        user : User
        evaluations : Evaluation list
    }

    module UserName =
        
        let create name =
            if String.IsNullOrWhiteSpace(name) then sprintf "Name is invalid (%s)" name |> Error
            else
            UserName name |> Ok

        let value userName =
            let (UserName name) = userName
            name

    module User =
        let create name =
            UserName.create name
            |> Result.map (fun userName -> {name = userName})
            
    
    let addEvaluation evaluation userSkills = 
        {
            userSkills with evaluations = evaluation :: userSkills.evaluations 
        }

    let findSkills user (usersSkills:UserSkills list) =
        let foundSkills = List.tryFind (fun userSkill -> userSkill.user = user) usersSkills
        match foundSkills with 
        | None -> { user = user; evaluations = []}
        | Some userSkills -> userSkills
    