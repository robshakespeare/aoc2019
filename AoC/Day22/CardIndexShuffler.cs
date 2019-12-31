// rs-todo: fix and finish
// // using System;

// // namespace AoC.Day22
// // {
// //     public class CardIndexShuffler
// //     {
// //         private readonly int factoryOrderNumber;

// //         public CardIndexShuffler(in int factoryOrderNumber)
// //         {
// //             this.factoryOrderNumber = factoryOrderNumber;
// //         }

// //         /// <summary>
// //         /// Performs the whole shuffle process, just for the card number specified.
// //         /// The card number is zero based, so it is also the starting index.
// //         /// </summary>
// //         public int ShuffleIndex(int cardNumber, (InstructionType instruction, int operand)[] shuffleProcess)
// //         {
// //             var index = cardNumber;

// //             foreach (var (instruction, operand) in shuffleProcess)
// //             {
// //                 index = instruction switch
// //                     {
// //                     InstructionType.DealIntoNewStack => factoryOrderNumber - index - 1,
// //                     InstructionType.Cut => (index - operand + factoryOrderNumber) % factoryOrderNumber,
// //                     InstructionType.DealWithIncrement => (operand * index % factoryOrderNumber + factoryOrderNumber) % factoryOrderNumber,
// //                     _ => throw new InvalidOperationException("Unexpected instruction type: " + instruction)
// //                     };
// //             }

// //             return index;
// //         }
// //     }
// // }
