namespace Skills.Domain

type Level = private Level of int

module Level = 
    let MIN_LEVEL = 0
    let MAX_LEVEL = 5

    let create level = 
        if level < MIN_LEVEL || level > MAX_LEVEL then
            sprintf "Level should be between %i and %i (%i)" MIN_LEVEL MAX_LEVEL level |> Error
        else
            Level level |> Ok


    let value level =
        let (Level value) = level
        value
