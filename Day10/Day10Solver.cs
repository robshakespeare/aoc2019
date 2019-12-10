using System;
using System.Drawing;
using Common;

namespace Day10
{
    public class Day10Solver : SolverReadAllLines
    {
        public override long? SolvePart1(string[] input)
        {
            return base.SolvePart1(input);
        }

        public override long? SolvePart2(string[] input)
        {
            return base.SolvePart2(input);
        }

        //public double CalculateAngleBetweenVectors(PointF v1, PointF v2)
        //{
        //    // angle(v1, v2) = acos( (v1x * v2x + v1y * v2y) / (sqrt(v1x^2+v1y^2) * sqrt(v2x^2+v2y^2)) )

        //    return (v1.X * v2.X + v1.Y * v2.Y);

        //    return Math.Acos(
        //        (v1.X * v2.X + v1.Y * v2.Y) /
        //        (Math.Sqrt(Math.Pow(v1.X, 2) + Math.Pow(v1.Y, 2)) * Math.Sqrt(Math.Pow(v2.X, 2) + Math.Pow(v2.Y, 2))));
        //}

        //public PointF Normalise(PointF v)
        //{
        //    return v / 2.5f;
        //}

        ////public PointF NormalizeVector(PointF v)
        ////{
        ////    var length = CalculateLengthOfVector(v);
        ////    return new PointF(v.X / length, v.Y / length);
        ////}
    }
}
