using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Common.IntCodes;

namespace Day13
{
    public class Day13Part2Solver
    {
        ////private const int FirstPossibleMove = -1;
        ////private const int LastPossibleMove = 1;

        private static readonly IReadOnlyCollection<int> PossibleMoves = new ReadOnlyCollection<int>(new[] {0, -1, 1});

        private readonly IntCodeComputer intCodeComputer = new IntCodeComputer();
        private readonly string inputProgram;

        public Day13Part2Solver(string inputProgram)
        {
            this.inputProgram = inputProgram;
        }

        public long? Solve() => ProcessFrame(intCodeComputer.Parse(inputProgram));

        ////var previousFrames = new Stack<IntCodeState>(); ////(IntCodeState intCodeState, int lastAttemptedInput, long score)>();
        ////var lastAttemptedInput = -2;
        ////var score = 0L;
        ////previousFrames.Push(intCodeState.Clone());
        ////return Process(intCodeState, FirstPossibleMove - 1, previousFrames);

        private long? ProcessFrame(IntCodeState intCodeState)
        {
            intCodeComputer.EvaluateUntilHaltedOrInputNeeded(intCodeState);

            // If we halted, we must have either won or lost!
            if (intCodeState.Halted)
            {
                return GetScore(intCodeState);
            }

            // We didn't halt, so we must need input
            foreach (var move in PossibleMoves)
            {
                var childIntCodeState = intCodeState.Clone();
                intCodeComputer.EvaluateNextInstruction(childIntCodeState, () => move);

                var childScore = ProcessFrame(childIntCodeState);
                if (childScore != null && childScore > 0)
                {
                    return childScore; // game has finished, we have a score, we won!
                }
            }

            return null; // No routes down this node give us a successful score.
        }


        private static long? GetScore(IntCodeState intCodeState)
        {
            var outputs = intCodeState.Outputs;
            if (outputs[outputs.Count - 3] == -1 && outputs[outputs.Count - 2] == 0)
            {
                return outputs[outputs.Count - 1];
            }
            throw new InvalidOperationException("No score was provided as last output");
        }

        ////private long ProcessFrame(
        ////    IntCodeState intCodeState,
        ////    Stack<(IntCodeState intCodeState, int lastAttemptedInput, long score)> previousFrames,
        ////    int lastAttemptedInput,
        ////    long score)
        ////{
        ////    var outputs = new List<long>();

        ////    foreach (var move in PossibleMoves)
        ////    {
                
        ////    }

        ////    while (lastAttemptedInput <= 1 && intCodeComputer.EvaluateNextInstruction(
        ////        intCodeState,
        ////        () =>
        ////        {
        ////            lastAttemptedInput++;
        ////            previousFrames.Push((intCodeState.Clone(), lastAttemptedInput, score));
        ////            return lastAttemptedInput;
        ////        },
        ////        output =>
        ////        {
        ////            outputs.Add(output);

        ////            if (outputs.Count == 3)
        ////            {
        ////                if (outputs[0] == -1 && outputs[1] == 0)
        ////                {
        ////                    score = outputs[2];
        ////                }

        ////                outputs.Clear();
        ////            }
        ////        }))
        ////    {
        ////    }

        ////    // if score == 0, backtrack loop!

        ////    return -1;
        ////}



        ////private long? Process(IntCodeState intCodeState, int currentMove, Stack<IntCodeState> previousFrames)
        ////{
        ////    var unsolvable = false;

        ////    while (!unsolvable)
        ////    {
        ////        ////previousFrames.Push(intCodeState.Clone());

        ////        ////if (nextMove > LastPossibleMove)
        ////        ////{
        ////        ////    // ????
        ////        ////    return null;
        ////        ////}

        ////        intCodeComputer.EvaluateUntilHaltedOrInputNeeded(intCodeState);

        ////        if (intCodeState.Halted)
        ////        {
        ////            long score = GetScore(intCodeState);

        ////            if (score > 0)
        ////            {
        ////                // if we have score, we finished!
        ////                return score;
        ////            }

        ////            intCodeState = previousFrames.Pop();
        ////        }

        ////        // If we have halted, but did not get a score, then that means we lost, so try next input, or jump up if there is no next input
        ////        // Or if we have not halted, that means we need a next input, so try next input, or jump up if there is no next input

        ////        var nextMove = currentMove + 1;

        ////        if (nextMove <= LastPossibleMove)
        ////        {
        ////            previousFrames.Push(intCodeState.Clone());
        ////            intCodeComputer.EvaluateNextInstruction(intCodeState, () => nextMove);
        ////            //return DoIt(intCodeState, nextMove, previousFrames);
        ////        }
        ////        else if (previousFrames.Count == 0)
        ////        {
        ////            unsolvable = true; // Unsolvable!
        ////        }
        ////        else
        ////        {
        ////            intCodeState = previousFrames.Pop();
        ////            currentMove = FirstPossibleMove - 1;
        ////        }

        ////        // try the next input, if no inputs left, jump up to previous frame


        ////        //////foreach (var move in PossibleMoves)
        ////        //////{
        ////        ////    //RunUntilHaltedOrNextInputGiven(intCodeState, nextMove, previousFrames);



        ////        // try the next input, if no inputs left, jump up to previous frame
        ////        //}

        ////        // no inputs left, go back to previous frame, and try next input there
        ////    }

        ////    return null; // Unsolvable!
        ////}

        ////private void RunUntilHaltedOrNextInputGiven(IntCodeState intCodeState, int move, Stack<IntCodeState> previousFrames)
        ////{
        ////    // Every time we provide a new input, that is essentially a point we can go back, and try next input

        ////    var inputProvided = false;
        ////    while (intCodeState.ReadNextInstruction().OpCode !inputProvided && intCodeComputer.EvaluateNextInstruction(
        ////               intCodeState,
        ////               () =>
        ////               {
        ////                   previousFrames.Push(intCodeState.Clone());
        ////                   inputProvided = true;
        ////                   return move;
        ////               }))
        ////    {
        ////    }
        ////}

        ////void DisplayComputerOutput(long value)
        ////{
        ////    outputs.Add(value);

        ////    if (outputs.Count == 3)
        ////    {
        ////        if (outputs[0] == -1 && outputs[1] == 0)
        ////        {
        ////            score = outputs[2];
        ////            Console.SetCursorPosition(game.Right + 2, 1);
        ////            Console.Write($"Score: {score.ToString().PadRight(20)}");
        ////        }
        ////        else
        ////        {
        ////            game.Update(outputs);
        ////        }

        ////        outputs.Clear();
        ////    }
        ////}
    }
}
