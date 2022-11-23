namespace Aoc2022Net.Days
{
    internal sealed class Day15 : Day
    {
        private record Point(int X, int Y);

        private static readonly Point[] Offsets = { new(1, 0), new(-1, 0), new(0, 1), new(0, -1) };

        public override object SolvePart1() => FindLowestTotalRisk(1);

        public override object SolvePart2() => FindLowestTotalRisk(5);

        private int FindLowestTotalRisk(int tileMultiplier)
        {
            var (tile, tileWidth, tileHeight) = InputData.GetInputInt32Grid(0, 0);
            var grid = new int[tileWidth * tileMultiplier, tileHeight * tileMultiplier];

            for (var tileRow = 0; tileRow < tileMultiplier; tileRow++)
            {
                for (var tileColumn = 0; tileColumn < tileMultiplier; tileColumn++)
                {
                    for (var tileX = 0; tileX < tileWidth; tileX++)
                    {
                        for (var tileY = 0; tileY < tileHeight; tileY++)
                        {
                            var value = (tile[tileX, tileY] + tileRow + tileColumn) % 9;
                            if (value == 0)
                                value = 9;

                            grid[tileX + tileColumn * tileWidth, tileY + tileRow * tileHeight] = value;
                        }
                    }
                }
            }

            var path = FindPathWithLowestTotalRisk(grid);
            return CalculatePathRisk(path, grid) - grid[0, 0];
        }

        private static Point[] FindPathWithLowestTotalRisk(int[,] grid)
        {
            var gridWidth = grid.GetLength(0);
            var gridHeight = grid.GetLength(1);

            var start = new Point(0, 0);
            var end = new Point(gridWidth - 1, gridHeight - 1);

            var closedPoints = new HashSet<Point>();
            var openPaths = new PriorityQueue<Point[], int>();
            
            openPaths.Enqueue(new[] { start }, 0);

            while (openPaths.Count > 0)
            {
                var path = openPaths.Dequeue();
                
                var pathEnd = path.Last();
                if (closedPoints.Contains(pathEnd))
                    continue;

                if (pathEnd == end)
                    return path;

                closedPoints.Add(pathEnd);

                foreach (var offset in Offsets)
                {
                    var neighborPoint = new Point(pathEnd.X + offset.X, pathEnd.Y + offset.Y);
                    if (neighborPoint.X < 0 || neighborPoint.X >= gridWidth || neighborPoint.Y < 0 || neighborPoint.Y >= gridHeight)
                        continue;

                    openPaths.Enqueue(path.Concat(new[] { neighborPoint }).ToArray(), CalculatePathRisk(path, grid) + grid[neighborPoint.X, neighborPoint.Y]);
                }
            }

            throw new InvalidOperationException("No path");
        }

        private static int CalculatePathRisk(Point[] path, int[,] grid) =>
            path.Sum(point => grid[point.X, point.Y]);
    }
}
