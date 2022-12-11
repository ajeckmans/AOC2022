open System
open System.Reflection
open AOC2022.Runner
open AOC2022.Runner.AOC
open Argu

type RunArgs =
    | [<Mandatory; MainCommand>] Run of day: int * puzzle: int
    | Submit
    | [<Inherit>] Session of session: string
    | UseSampleInput

    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Run _ -> "Specify day and puzzle"
            | Submit -> "Whether to submit the results or not"
            | UseSampleInput -> "Whether to use the sample input"
            | Session _ -> "The AOC session"


and DownloadArgs =
    | [<Mandatory; MainCommand>] Download of day: int * puzzle: int
    | [<Inherit>] Session of session: string

    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Download _ -> "Specify day and puzzle"
            | Session _ -> "The AOC session"

and Arguments =
    | [<CliPrefix(CliPrefix.None)>] Run of ParseResults<RunArgs>
    | [<CliPrefix(CliPrefix.None)>] Download of ParseResults<DownloadArgs>
    | Session of session: string

    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Session _ -> "specify AOC session"
            | Run _ -> "Runs a specific puzzle, downloading input and optionally submitting"
            | Download _ -> "Downloads input for a specific puzzle"

[<EntryPoint>]
let main args =
    let reader = EnvironmentVariableConfigurationReader() :> IConfigurationReader
    let parser = ArgumentParser.Create<Arguments>(programName = "AOC2022.Runner.exe")

    let res =
        parser.Parse(args, configurationReader = reader, ignoreMissing = false, ignoreUnrecognized = true)

    let run day puzzle input =
        let func =
            Assembly.GetEntryAssembly().GetTypes()
            |> Seq.collect (fun t -> t.GetMethods())
            |> Seq.find (fun t -> t.Name.Equals($"runPuzzle_{day}_{puzzle}", StringComparison.OrdinalIgnoreCase))

        (if func.IsGenericMethod then
             func.MakeGenericMethod([| typedefof<string> |])
         else
             func)
            .Invoke(null, [| input |])

    match res.TryGetSubCommand() with
    | Some (Run args) ->
        let day, puzzle = args.GetResult RunArgs.Run
        let session = args.GetResult RunArgs.Session
        let submit = args.Contains RunArgs.Submit
        let useSampleInput = args.Contains RunArgs.UseSampleInput

        (task {
            let! input =
                if useSampleInput then
                    AOC.getSampleInput day puzzle
                else
                    AOC.downloadInput day puzzle session

            let result = run day puzzle input

            if submit then
                let! response = (AOC.submitInput day puzzle result session)
return
    match response with
    | Result.Ok s -> box $"Correct answer {result} \n submitted to AOC.\n Response was {s}"
    | Result.Error s -> box $"Incorrect answer {result} \n submitted to AOC.\n Response was {s}"
            else
                return result
        })
            .GetAwaiter()
            .GetResult()
        |> printf "%A"
    | Some (Download args) ->
        let day, puzzle = args.GetResult DownloadArgs.Download
        let session = args.GetResult DownloadArgs.Session

        (AOC.downloadInput day puzzle session).GetAwaiter().GetResult() |> printf "%A"
    | None -> printf $"%s{parser.PrintUsage()}"
    | _ -> failwith "argument not added"

    1
