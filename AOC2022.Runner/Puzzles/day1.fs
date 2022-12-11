module AOC2022.Runner.Puzzles.Day1

open System

let sumsByIndex (input: string) =
    input.Split("\n")
    |> Seq.fold
        (fun (newElf, counter) line ->
            if String.IsNullOrEmpty(line) then
                (true, counter)
            else
                let calories = line |> int

                match newElf, counter with
                | false, first :: remainder -> (false, (first + calories) :: remainder)
                | true, counter -> (false, calories :: counter)
                | _, [] -> (false, [ calories ]))
        (false, [])
    |> snd
    |> Seq.mapi (fun i a -> i, a)
    
let runPuzzle_1_1 (input: string) =
    sumsByIndex input 
    |> Seq.maxBy snd
    
let runPuzzle_1_2 (input: string) =
    sumsByIndex input
    |> Seq.sortByDescending snd
    |> Seq.take 3
    |> Seq.sumBy snd
