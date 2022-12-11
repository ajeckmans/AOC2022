namespace AOC2022.Runner.AOC

open System
open System.IO
open FsHttp

module AOC =
    let getSampleInput day puzzle =
        let fileName = $"./content/{day}_{puzzle}_sample"
        task { return! (File.ReadAllTextAsync fileName) }

    let downloadInput day puzzle session =
        task {
            let fileName = $"./content/{day}_{puzzle}"

            if (File.Exists fileName) then
                return! (File.ReadAllTextAsync fileName)
            else
                let! response =
                    http {
                        GET $"https://adventofcode.com/2022/day/{day}/input"
                        Cookie "session" session
                        CacheControl "no-cache"
                    }
                    |> Request.sendTAsync

                let! content = response.content.ReadAsStringAsync()

                let sanitized = content.TrimEnd('\n')

                Directory.CreateDirectory("content") |> ignore
                do! File.WriteAllTextAsync(fileName, sanitized)

                return sanitized
        }

    let submitInput day puzzle input session =
        task {
            let! response =
                http {
                    POST $"https://adventofcode.com/2022/day/{day}/answer"
                    CacheControl "no-cache"
                    Cookie "session" session
                    body
                    formUrlEncoded [ "level", puzzle |> string; "answer", input |> string ]
                }
                |> Request.sendTAsync

            response.originalHttpResponseMessage.EnsureSuccessStatusCode() |> ignore
            let! responseContent = response.content.ReadAsStringAsync()

            if responseContent.Contains("That's the right answer!", StringComparison.OrdinalIgnoreCase) then
                return Ok "Answer is correct"
            else
                return Error responseContent
        }
