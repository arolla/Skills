namespace Skills.Domain

module UserSkills =
    
    let createDefault userName =
        {
            user = UserName.create userName
            evaluations = []
        }

