// Learn more about F# at http://fsharp.org
module Day2

open System
open System.IO
open System.Linq

type Solver = class
    static member loadIntCodes noun verb =
        let intCodes =
            File.ReadAllText("input.txt")
                .Split(',')
                .Select(fun s -> int(s))
                .ToArray()
        Array.set intCodes 1 noun
        Array.set intCodes 2 verb

        intCodes

    static member processIntCodes (intCodes : int[]) (index : int) =
        if (intCodes.[index] <> 99) then
            let opcode = intCodes.[index]
            let leftIndex = intCodes.[index + 1]
            let rightIndex = intCodes.[index + 2]
            let storageIndex = intCodes.[index + 3]

            let left = intCodes.[leftIndex]
            let right = intCodes.[rightIndex]

            let result =
                match opcode with
                | 1 -> left + right
                | 2 -> left * right
                | _ -> raise (InvalidOperationException("Invalid opcode: " + string(opcode)))

            Array.set intCodes storageIndex result

            Solver.processIntCodes intCodes (index + 4)
        else
            intCodes.[0]

    static member findSolution () =
        seq {
                for noun in 0 .. 99 do
                    for verb in 0 .. 99 ->
                        (Solver.processIntCodes (Solver.loadIntCodes noun verb) 0, noun, verb)
            }
            |> Seq.find (fun (result, noun, verb) -> result = 19690720)
            |> fun (result, noun, verb) -> 100 * noun + verb
end

[<EntryPoint>]
let main _ =
    let solution = Solver.findSolution ()
    printfn "Solution: %s" (string(solution))

    0 // return integer exit code indicating success
