using System.Collections.Generic;

namespace AoC.Day18
{
    public class Explorer
    {
        private Vector position;
        private readonly Part1Result part1Result;

        private readonly Grid grid;
        private readonly Dictionary<Vector, int> gridSteps = new Dictionary<Vector, int>(); // KEY is grid location, VALUE is the number of steps from origins
        private readonly Stack<Vector> gridTrail = new Stack<Vector>(); // So we can track backwards, first item out is always our CURRENT position
        private readonly Dictionary<Vector, Queue<MovementCommand>> gridAvailableCommands = new Dictionary<Vector, Queue<MovementCommand>>();
        private readonly HashSet<Vector> gridLocationsWithAvailableCommands = new HashSet<Vector>();
        private readonly HashSet<Vector> gridVisited = new HashSet<Vector>();

        private readonly List<(char key, int numberOfSteps, Vector location)> keysFound = new List<(char key, int numberOfSteps, Vector location)>();

        public IEnumerable<(char key, int numberOfSteps, Vector location)> KeysFound => keysFound;

        public Vector InitialPosition { get; }
        public int InitialNumberOfSteps { get; }

        /// <remarks>
        /// Note that the `numberOfSteps` if the number of steps to reach the current position.
        /// </remarks>
        public Explorer(Grid grid, Vector position, int numberOfSteps, Part1Result part1Result)
        {
            this.grid = grid;
            this.position = position;
            this.part1Result = part1Result;
            InitialPosition = position;
            InitialNumberOfSteps = numberOfSteps;

            VisitGridLocationForFirstTime(position, numberOfSteps); // Note: our initial number of steps is just what it was to reach our current position
            gridTrail.Push(position);
        }

        private void VisitGridLocationForFirstTime(Vector location, int newStepNumber)
        {
            gridVisited.Add(location);
            gridSteps.Add(location, newStepNumber);
            gridAvailableCommands.Add(location, BuildAvailableMovementCommands());
            gridLocationsWithAvailableCommands.Add(location);
        }

        private static Queue<MovementCommand> BuildAvailableMovementCommands() => new Queue<MovementCommand>(MovementCommand.PossibleCommands);

        private bool IsVisited(Vector location) => gridVisited.Contains(location);

        public void Explore()
        {
            while (gridLocationsWithAvailableCommands.Count > 0)
            {
                var nextMovement = GetNextMovementVector();

                if (nextMovement != null)
                {
                    PerformNextMovement(nextMovement.Value);
                }
            }
        }

        private Vector? GetNextMovementVector()
        {
            // Decide next move:

            // If next move takes us to an unexplored area, then try go there.
            // If next move takes us to a wall/door/explored area, don't go there.

            var availableCommands = gridAvailableCommands[position];

            if (availableCommands.Count > 0)
            {
                var nextCommand = availableCommands.Dequeue();
                if (availableCommands.Count == 0)
                {
                    gridLocationsWithAvailableCommands.Remove(position);
                }

                var nextPosition = position + nextCommand.MovementVector;
                var isValidMove = grid.IsAvailableLocation(nextPosition) && !IsVisited(nextPosition);

                return isValidMove ? nextCommand.MovementVector : GetNextMovementVector();
            }

            // If we run out of possible moves, then track back one, and repeat
            var currentPosition = gridTrail.Pop();

            if (currentPosition.Equals(InitialPosition) && gridTrail.Count == 0)
            {
                return null;
            }

            var nextBackTrackPosition = gridTrail.Pop();
            var movementBack = nextBackTrackPosition - currentPosition;
            return movementBack;
        }

        private void PerformNextMovement(Vector nextMovement)
        {
            var newStepNumber = gridSteps[position] + 1;
            position += nextMovement;

            gridTrail.Push(position);

            // Record our number of steps if we haven't been here yet
            if (!gridSteps.ContainsKey(position))
            {
                VisitGridLocationForFirstTime(position, newStepNumber);
            }

            // If our new position is over a key, pick it up, but then treat as a dead end, and back track
            //   > i.e. record the number of steps to get to that key
            //   > when we find a key, STORE that key, number of steps and location, and then treat as a dead-end, and back track
            //   > we back track here, because our purposes specifically here is to find all the next available keys
            //   > our recursive element to the solution will deal with findings all the next keys from each of the last endpoints
            if (grid.IsKey(position, out var key))
            {
                keysFound.Add((key, newStepNumber, position)); // store key, number of steps and location

                gridAvailableCommands[position].Clear(); // force a back-track
                gridLocationsWithAvailableCommands.Remove(position);
            }

            // don't bother continuing if we're over known shortest distance!
            if (newStepNumber > part1Result.MinNumberOfStepsToCollectAllKeys)
            {
                gridAvailableCommands[position].Clear(); // force a back-track
                gridLocationsWithAvailableCommands.Remove(position);
            }
        }
    }
}
