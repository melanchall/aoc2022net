// Rope Bridge: https://adventofcode.com/2022/day/9

using Aoc2022Net.Utilities;
using System.Text.RegularExpressions;

namespace Aoc2022Net.Days
{
    internal sealed class Day9 : Day
    {
        private record Point(int X, int Y);

        private static readonly Dictionary<string, Point> Directions = new()
        {
            ["L"] = new(-1, 0),
            ["R"] = new(1, 0),
            ["U"] = new(0, 1),
            ["D"] = new(0, -1)
        };

        public override object SolvePart1() => Solve(2);

        public override object SolvePart2() => Solve(10);

        private int Solve(int knotsCount)
        {
            var moves = from line in InputData.GetInputLines()
                        let match = Regex.Match(line, @"(\w) (\d+)")
                        from _ in Enumerable.Range(0, match.GetInt32Group(2))
                        select match.GetStringGroup(1);

            var knots = Enumerable.Range(0, knotsCount).Select(_ => new Point(0, 0)).ToArray();
            var visitedPoints = new HashSet<Point> { new(0, 0) };

            foreach (var move in moves)
            {
                var offset = Directions[move];

                knots[0] = new Point(knots[0].X + offset.X, knots[0].Y + offset.Y);

                for (var i = 1; i < knots.Length; i++)
                {
                    var previousKnotXMove = knots[i - 1].X - knots[i].X;
                    var previousKnotYMove = knots[i - 1].Y - knots[i].Y;
                    if (Math.Abs(previousKnotXMove) <= 1 && Math.Abs(previousKnotYMove) <= 1)
                        continue;

                    knots[i] = new(
                        knots[i].X + (previousKnotXMove + Math.Sign(previousKnotXMove)) / 2,
                        knots[i].Y + (previousKnotYMove + Math.Sign(previousKnotYMove)) / 2);
                }

                visitedPoints.Add(knots.Last());
            }

            return visitedPoints.Count;
        }
    }
}
