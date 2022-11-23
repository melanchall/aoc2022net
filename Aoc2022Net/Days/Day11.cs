using Aoc2022Net.Utilities;

namespace Aoc2022Net.Days
{
    internal sealed class Day11 : Day
    {
        public override object SolvePart1() =>
            CountFlashesAfterStepOrStepsForSyncronizedFlash(100);

        public override object SolvePart2() =>
            CountFlashesAfterStepOrStepsForSyncronizedFlash(int.MaxValue);

        private int CountFlashesAfterStepOrStepsForSyncronizedFlash(int maxSteps)
        {
            var (grid, width, height) = InputData.GetInputInt32Grid(1, -int.MaxValue);

            const int minValueToFlash = 10;
            const int valueAfterFlash = 0;

            var totalFlashesCount = 0;

            for (var step = 0; step < maxSteps; step++)
            {
                DataProvider
                    .GetGridCoordinates(width, height)
                    .ToList()
                    .ForEach(p => grid[p.X + 1, p.Y + 1]++);

                var flashedPoints = new List<(int X, int Y)>();
                var newFlashesCount = 0;

                while (grid.Cast<int>().Any(v => v >= minValueToFlash))
                {
                    var aboutFlash = DataProvider
                        .GetGridCoordinates(width, height)
                        .Where(xy => grid[xy.X + 1, xy.Y + 1] >= minValueToFlash)
                        .Select(xy => (X: xy.X + 1, Y: xy.Y + 1))
                        .ToArray();

                    foreach (var xy in aboutFlash.Where(p => !flashedPoints.Contains(p)))
                    {
                        flashedPoints.Add(xy);
                        grid[xy.X, xy.Y] = valueAfterFlash;
                        newFlashesCount++;

                        for (var x = xy.X - 1; x <= xy.X + 1; x++)
                        {
                            for (var y = xy.Y - 1; y <= xy.Y + 1; y++)
                            {
                                if ((x == xy.X && y == xy.Y) || flashedPoints.Contains((x, y)))
                                    continue;

                                grid[x, y]++;
                            }
                        }
                    }
                }

                if (newFlashesCount == width * height)
                    return step + 1;

                totalFlashesCount += flashedPoints.Count;
            }

            return totalFlashesCount;
        }
    }
}
