using Aoc2022Net.Utilities;
using System.Text;
using System.Text.RegularExpressions;

namespace Aoc2022Net.Days
{
    internal sealed class Day13 : Day
    {
        private record Point(int X, int Y);
        private record FoldRule(bool ByX, int Value);

        public override object SolvePart1()
        {
            var (points, foldRules) = GetInputData();
            return Fold(points, foldRules.First()).Distinct().Count();
        }

        public override object SolvePart2()
        {
            var (points, foldRules) = GetInputData();

            foreach (var f in foldRules)
            {
                points = Fold(points, f).Distinct().ToList();
            }

            var width = points.Select(p => p.X).Max() + 1;
            var height = points.Select(p => p.Y).Max() + 1;

            var grid = new bool[width, height];
            points.ForEach(p => grid[p.X, p.Y] = true);

            var stringBuilder = new StringBuilder();

            for (var y = 0; y < height; y++)
            {
                stringBuilder.AppendLine();

                for (var x = 0; x < width; x++)
                {
                    stringBuilder.Append(grid[x, y] ? "#" : ".");
                }
            }

            return stringBuilder.ToString().Trim();
        }

        private (List<Point> Points, ICollection<FoldRule> FoldRules) GetInputData()
        {
            var lines = InputData.GetInputLines(true);

            var points = new List<Point>();
            var foldRules = new List<FoldRule>();

            foreach (var line in lines)
            {
                var match = Regex.Match(line, @"fold along (\w)\=(\d+)");
                if (match.Success)
                    foldRules.Add(new FoldRule(match.Groups[1].Value == "x", match.GetInt32Group(2)));
                else
                {
                    match = Regex.Match(line, @"(\d+),(\d+)");
                    points.Add(new Point(match.GetInt32Group(1), match.GetInt32Group(2)));
                }
            }

            return (points, foldRules);
        }

        private static IEnumerable<Point> Fold(IEnumerable<Point> points, FoldRule rule) => rule.ByX
            ? points
                .Where(p => p.X < rule.Value)
                .Concat(points.Where(p => p.X > rule.Value).Select(p => new Point(rule.Value - (p.X - rule.Value), p.Y)))
            : points
                .Where(p => p.Y < rule.Value)
                .Concat(points.Where(p => p.Y > rule.Value).Select(p => new Point(p.X, rule.Value - (p.Y - rule.Value))));
    }
}
