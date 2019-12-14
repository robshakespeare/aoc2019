using System;
using System.Collections.Generic;

namespace AoC.Day11
{
    /// <remarks>
    /// The IntCode program will serve as the brain of the robot.
    ///
    /// The program uses input instructions to access the robot's camera, provide:
    ///     0 if the robot is over a black panel, or
    ///     1 if the robot is over a white panel.
    ///
    /// Then, the program will output two values:
    /// 1) First, it will output a value indicating the color to paint the panel the robot is over:
    ///     0 means to paint the panel black, and
    ///     1 means to paint the panel white.
    /// 2) Second, it will output a value indicating the direction the robot should turn: 0 means it should turn left 90 degrees, and 1 means it should turn right 90 degrees.
    /// 
    /// Note: still using coordinate system where positive X goes right, and positive Y goes down.
    ///
    /// All of the panels start black.
    /// </remarks>
    public class Robot
    {
        public const int Black = 0;
        public const int White = 1;
        public const int DefaultColor = Black;

        public Vector Heading { get; private set; } = new Vector(0, -1); // The robot starts facing up.

        public Vector Location { get; set; } = new Vector(0, 0);

        public Dictionary<Vector, long> PaintedGrid { get; } = new Dictionary<Vector, long>();

        private Action<long> nextCommand;

        public Robot()
        {
            nextCommand = ProcessPaintCommand;
        }

        public long GetPanelColor(Vector location) => PaintedGrid.TryGetValue(location, out var color) ? color : DefaultColor;

        public long GetCurrentPanelColor() => GetPanelColor(Location);

        public void ProcessNextCommand(long command) => nextCommand(command);

        /// <summary>
        /// Paint the panel the robot is over: 0 means to paint the panel black, and 1 means to paint the panel white.
        /// </summary>
        public void ProcessPaintCommand(long paintColor)
        {
            if (paintColor != Black && paintColor != White)
            {
                throw new InvalidOperationException("Invalid paint color: " + paintColor);
            }

            PaintedGrid[Location] = paintColor;

            // Next command should always be a Move command
            nextCommand = ProcessMoveCommand;
        }

        /// <summary>
        /// Takes a value indicating the direction the robot should turn: 0 means it should turn left 90 degrees, and 1 means it should turn right 90 degrees.
        /// After the robot turns, it should always move forward exactly one panel.
        /// </summary>
        public void ProcessMoveCommand(long moveCommand)
        {
            // Perform turn
            // 0 means it should turn left 90 degrees, and 1 means it should turn right 90 degrees.
            switch (moveCommand)
            {
                case 0:
                    // Turn left
                    Heading = Heading switch
                        {
                        _ when Heading.Equals(new Vector(0, -1)) => new Vector(-1, 0),
                        _ when Heading.Equals(new Vector(-1, 0)) => new Vector(0, 1),
                        _ when Heading.Equals(new Vector(0, 1)) => new Vector(1, 0),
                        _ when Heading.Equals(new Vector(1, 0)) => new Vector(0, -1),
                        _ => throw new InvalidOperationException("Invalid heading while trying to turn left: " + Heading)
                        };
                    break;
                case 1:
                    // Turn right
                    Heading = Heading switch
                        {
                        _ when Heading.Equals(new Vector(0, -1)) => new Vector(1, 0),
                        _ when Heading.Equals(new Vector(1, 0)) => new Vector(0, 1),
                        _ when Heading.Equals(new Vector(0, 1)) => new Vector(-1, 0),
                        _ when Heading.Equals(new Vector(-1, 0)) => new Vector(0, -1),
                        _ => throw new InvalidOperationException("Invalid heading while trying to turn right: " + Heading)
                        };
                    break;
                default:
                    throw new InvalidOperationException("Invalid move command: " + moveCommand);
            }

            // After the robot turns, it should always move forward exactly one panel
            Location += Heading;

            // Next command should always be a Paint command
            nextCommand = ProcessPaintCommand;
        }
    }
}
