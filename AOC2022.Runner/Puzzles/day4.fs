module AOC2022.Runner.Puzzles.Day4


let runPuzzle_4_1 (input: string) =
    input.Split('\n', ',', '-')
    |> Seq.map int
    |> Seq.chunkBySize 4
    |> Seq.sumBy (fun r ->
        match r with
        | [| a1; a2; b1; b2 |] when a1 >= b1 && a2 <= b2 -> 1
        | [| a1; a2; b1; b2 |] when b1 >= a1 && b2 <= a2 -> 1
        | x when x.Length = 4 -> 0
        | _ -> failwith "should not be anything other than size 4")

let runPuzzle_4_2 (input: string) =
    input.Split('\n', ',', '-')
    |> Seq.map int
    |> Seq.chunkBySize 4
    |> Seq.sumBy (fun r ->
        match r with
        | [| a1; _; b1; b2 |] when a1 >= b1 && a1 <= b2 -> 1
        | [| _; a2; b1; b2 |] when a2 >= b1 && a2 <= b2 -> 1
        | [| a1; a2; b1; _ |] when b1 >= a1 && b1 <= a2 -> 1
        | [| a1; a2; _; b2 |] when b2 >= a1 && b2 <= a2 -> 1
        | x when x.Length = 4 -> 0
        | _ -> failwith "should not be anything other than size 4")
