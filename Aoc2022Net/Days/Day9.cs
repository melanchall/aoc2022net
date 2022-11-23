namespace Aoc2022Net.Days
{
    internal sealed class Day9 : Day
    {
        private record Point(int X, int Y);
        private record DayInputData(Point[] LowPoints, int[,] Grid);

        public override object SolvePart1()
        {
            var (lowPoints, grid) = GetInputData();
            return lowPoints.Sum(p => grid[p.X, p.Y] + 1);
        }

        public override object SolvePart2()
        {
            var (lowPoints, grid) = GetInputData();
            var basinsSizes = lowPoints
                .Select(p => GetBasinPoints(p, null, grid).Distinct().Count())
                .OrderByDescending(size => size)
                .Take(3)
                .ToArray();

            return basinsSizes[0] * basinsSizes[1] * basinsSizes[2];
        }

        private static IEnumerable<Point> GetBasinPoints(Point point, int? previousValue, int[,] grid)
        {
            var currentValue = grid[point.X, point.Y];
            if (previousValue != null && (currentValue >= 9 || currentValue - previousValue < 1))
                return Enumerable.Empty<Point>();

            return new[] { point }
                .Concat(GetBasinPoints(new(point.X + 1, point.Y), currentValue, grid))
                .Concat(GetBasinPoints(new(point.X - 1, point.Y), currentValue, grid))
                .Concat(GetBasinPoints(new(point.X, point.Y + 1), currentValue, grid))
                .Concat(GetBasinPoints(new(point.X, point.Y - 1), currentValue, grid));
        }

        private DayInputData GetInputData()
        {
            var (grid, width, height) = InputData.GetInputInt32Grid(1, int.MaxValue);

            var lowPoints = new List<Point>();

            for (var x = 1; x <= width; x++)
            {
                for (var y = 1; y <= height; y++)
                {
                    if (grid[x, y] < grid[x - 1, y] &&
                        grid[x, y] < grid[x, y - 1] &&
                        grid[x, y] < grid[x + 1, y] &&
                        grid[x, y] < grid[x, y + 1])
                        lowPoints.Add(new(x, y));
                }
            }

            return new(lowPoints.ToArray(), grid);
        }
    }
}
