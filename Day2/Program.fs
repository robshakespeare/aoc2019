// Learn more about F# at http://fsharp.org

open System
open System.IO
open System.Linq

[<EntryPoint>]
let main argv =
    let loadIntCodes noun verb =
        let intCodes =
            File.ReadAllText("input.txt")
                .Split(',')
                .Select(fun s -> int(s))
                .ToArray()
        Array.set intCodes 1 noun
        Array.set intCodes 2 verb
        
        intCodes

    let processIntCodes (intCodes : int[]) =
        let mutable index = 0
        while intCodes.[index] <> 99 do
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

            index <- index + 4
    
        intCodes.[0]

    for noun in 0 .. 99 do
        for verb in 0 .. 99 do
            let result = processIntCodes (loadIntCodes noun verb)

            if result = 19690720 then
                let solution = 100 * noun + verb
                printfn "Solution: %s" (string(solution))

                exit 0 // return integer exit code indicating success

    1 // return integer exit code indicating failure
