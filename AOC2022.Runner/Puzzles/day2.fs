module AOC2022.Runner.Puzzles.Day3

open System
open Microsoft.FSharp.Collections

let runPuzzle_3_1 (input: string) =
    input.Split("\n")
    |> Seq.map (fun x ->
        let first = x[.. x.Length / 2 - 1]
        let second = x[x.Length / 2 ..]
        Seq.allPairs first second |> Seq.find (fun (a, b) -> a = b) |> fst)
    |> Seq.map (fun x ->
        if Char.IsUpper(x) then
            x, (x |> int64) - 38L
        else
            x, (x |> int64) - 96L)
    |> Seq.sumBy snd


let runPuzzle_3_2 (input: string) =
    input.Split("\n")
    |> Seq.chunkBySize 3
    |> Seq.map (fun x ->
        Seq.allPairs x[0] x[1]
        |> Seq.allPairs x[2]
        |> Seq.find (fun (a, (b, c)) -> a = b && b = c) |> fst
    )

    |> Seq.sumBy (fun x ->
        if Char.IsUpper(x) then
            (x |> int64) - 38L
        else
            (x |> int64) - 96L)
