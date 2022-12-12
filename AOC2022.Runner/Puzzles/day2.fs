module AOC2022.Runner.Puzzles.Day2

open System


type Outcome =
    | Win
    | Loss
    | Draw

let runRoundLine scoring (line: string) =
    if String.IsNullOrEmpty line then
        0
    else
        let x = line.Split(" ")
        scoring x[0] x[1]

let runPuzzle_2_1 (input: string) =
    let getScore x y =
        match y with
        | "X" ->
            match x with
            | "A" -> Draw, 1
            | "B" -> Loss, 1
            | "C" -> Win, 1
            | _ -> failwith "invalid hand"
        | "Y" ->
            match x with
            | "A" -> Win, 2
            | "B" -> Draw, 2
            | "C" -> Loss, 2
            | _ -> failwith "invalid hand"
        | "Z" ->
            match x with
            | "A" -> Loss, 3
            | "B" -> Win, 3
            | "C" -> Draw, 3
            | _ -> failwith "invalid hand"
        | _ -> failwith "invalid hand"

    let runRound opponent self =
        match getScore opponent self with
        | Loss, s -> s
        | Win, s -> s + 6
        | Draw, s -> s + 3

    input.Split("\n") |> Seq.map (runRoundLine runRound) |> Seq.sum

let runPuzzle_2_2 (input: string) =
    let getScore opponent result =
        let opponentScore =
            match opponent with
            | "A" -> 1
            | "B" -> 2
            | "C" -> 3
            | _ -> failwith "invalid hand"

        match result with
        | "X" -> (if opponentScore = 1 then 3 else opponentScore - 1)
        | "Y" -> opponentScore + 3
        | "Z" -> (if opponentScore = 3 then 1 else opponentScore + 1) + 6
        | _ -> failwith "invalid hand"

    input.Split("\n") |> Seq.map (runRoundLine getScore) |> Seq.sum
