namespace Skills.Domain

open Types
open Skills.Domain.Result
     
module User =
    let create name =
        result {
            let! userName = UserName.create name
            return {name = userName}
        }