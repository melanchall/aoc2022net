using Aoc2022Net.Utilities;
using System.Text.RegularExpressions;

namespace Aoc2022Net.Days
{
    internal sealed class Day5 : Day
    {
        public override object SolvePart1() => CountDangerousPoints(false);

        public override object SolvePart2() => CountDangerousPoints(true);

        private int CountDangerousPoints(bool countDiagonal)
        {
            var lines = (from line in InputData.GetInputLines()
                         let match = Regex.Match(line, @"(\d+),(\d+) -> (\d+),(\d+)")
                         select new
                         {
                             X1 = match.GetInt32Group(1),
                             Y1 = match.GetInt32Group(2),
                             X2 = match.GetInt32Group(3),
                             Y2 = match.GetInt32Group(4)
                         }).ToArray();

            IEnumerable<int> GetCoordinateValues(int from, int to)
            {
                var step = Math.Sign(to - from);
                return Enumerable.Range(0, Math.Abs(to - from) + 1).Select(i => from + i * step);
            }

            var points = new List<(int X, int Y)>();

            foreach (var line in lines)
            {
                if (line.X1 == line.X2)
                    points.AddRange(
                        GetCoordinateValues(line.Y1, line.Y2).Select(y => (line.X1, y)));
                else if (line.Y1 == line.Y2)
                    points.AddRange(
                        GetCoordinateValues(line.X1, line.X2).Select(x => (x, line.Y1)));
                else if (countDiagonal)
                    points.AddRange(
                        GetCoordinateValues(line.X1, line.X2).Zip(GetCoordinateValues(line.Y1, line.Y2)));
            }

            return points.GroupBy(point => point).Count(pointsGroup => pointsGroup.Count() >= 2);
        }
    }
}
