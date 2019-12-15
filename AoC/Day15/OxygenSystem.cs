using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Day15
{
    public class OxygenSystem
    {
        private readonly Vector oxygenSystemPosition;
        private readonly Vector renderOffset;
        private readonly Dictionary<Vector, GridState> gridStates;
        private readonly bool disablePlotting;
        private readonly HashSet<Vector> oxygenLocations = new HashSet<Vector>();
        private readonly int numberOfLocationsToFill;

        public OxygenSystem(
            Vector oxygenSystemPosition,
            Vector renderOffset,
            Dictionary<Vector, GridState> gridStates,
            bool disablePlotting)
        {
            this.oxygenSystemPosition = oxygenSystemPosition;
            this.renderOffset = renderOffset;
            this.gridStates = gridStates;
            this.disablePlotting = disablePlotting;
            numberOfLocationsToFill = this.gridStates.Values.Count(state => state == GridState.Explored);
            
            AddOxygenLocation(oxygenSystemPosition);
        }

        private void AddOxygenLocation(Vector location)
        {
            Render(location, 'O');
            oxygenLocations.Add(location);
        }

        /// <returns>Number of iterations to fill with oxygen</returns>
        public long FillAndSolve()
        {
            var iterationsToFillWithOxygen = 0L;

            var edges = new[] { oxygenSystemPosition };

            // spread the oxygen, recording the number of iterative spreads to fill the valid parts of the grid
            while (oxygenLocations.Count < numberOfLocationsToFill)
            {
                edges = GetNextEdges(edges);
                foreach (var edge in edges)
                {
                    AddOxygenLocation(edge);
                }

                iterationsToFillWithOxygen++;
            }

            return iterationsToFillWithOxygen;
        }

        /// <summary>
        /// Move north/east/south/west, to any explorable space, which doesn't yet have oxygen in it.
        /// </summary>
        private Vector[] GetNextEdges(IEnumerable<Vector> edges) =>
            edges
                .SelectMany(edge => new[]
                {
                    edge + MovementCommand.North.MovementVector,
                    edge + MovementCommand.South.MovementVector,
                    edge + MovementCommand.East.MovementVector,
                    edge + MovementCommand.West.MovementVector
                })
                .Where(nextPos => gridStates[nextPos] == GridState.Explored)
                .Where(nextPos => !oxygenLocations.Contains(nextPos))
                .ToArray();

        private void Render(Vector location, char chr)
        {
            if (disablePlotting)
            {
                return;
            }

            var pos = location + renderOffset;
            Console.SetCursorPosition(pos.X, pos.Y);
            Console.Write(chr);
        }
    }
}
