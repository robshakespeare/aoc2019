module Day2.Tests

open NUnit.Framework

[<Test>]
let Day2Test () =
    // ACT
    let solution = Solver.findSolution ()

    // ASSERT
    Assert.AreEqual(5696, solution)
