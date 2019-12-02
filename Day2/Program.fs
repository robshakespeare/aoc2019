// Learn more about F# at http://fsharp.org

open System
open System.IO
open System.Linq

[<EntryPoint>]
let main argv =
    let intCodes =
        File.ReadAllText("input.txt")
            .Split(',')
            .Select(fun s -> int(s))
            .ToArray()

    Array.set intCodes 1 12
    Array.set intCodes 2 2

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

    let result = processIntCodes intCodes
    
    printfn "Solution: %s" (string(result))

    0 // return an integer exit code
