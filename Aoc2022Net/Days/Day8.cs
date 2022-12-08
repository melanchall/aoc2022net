// Treetop Tree House: https://adventofcode.com/2022/day/8

namespace Aoc2022Net.Days
{
    internal sealed class Day8 : Day
    {
        public override object SolvePart1()
        {
            var (grid, width, height) = InputData.GetInputInt32Grid(0, 0);

            var result = width * 2 + (height - 2) * 2;

            for (var x = 1; x < width - 1; x++)
            {
                for (var y = 1; y < height - 1; y++)
                {
                    var current = grid[x, y];

                    if (GetLeftXs(x).All(xOffset => IsLeftXValid(current, grid, x, y, xOffset)) ||
                        GetRightXs(x, width).All(newX => IsRightXValid(current, grid, newX, y)) ||
                        GetTopYs(y).All(yOffset => IsTopYValid(current, grid, x, y, yOffset)) ||
                        GetBottomYs(y, height).All(newY => IsBottomYValid(current, grid, x, newY)))
                        result++;
                }
            }

            return result;
        }

        public override object SolvePart2()
        {
            var (grid, width, height) = InputData.GetInputInt32Grid(0, 0);

            var result = 0;

            int Count(IEnumerable<int> values, Func<int, bool> predicate)
            {
                var count = 0;

                foreach (var c in values)
                {
                    count++;
                    if (!predicate(c))
                        break;
                }

                return count;
            }

            for (var x = 1; x < width - 1; x++)
            {
                for (var y = 1; y < height - 1; y++)
                {
                    var current = grid[x, y];
                    var score =
                        Count(GetLeftXs(x), xOffset => IsLeftXValid(current, grid, x, y, xOffset)) *
                        Count(GetRightXs(x, width), newX => IsRightXValid(current, grid, newX, y)) *
                        Count(GetTopYs(y), yOffset => IsTopYValid(current, grid, x, y, yOffset)) *
                        Count(GetBottomYs(y, height), newY => IsBottomYValid(current, grid, x, newY));
                    result = Math.Max(result, score);
                }
            }

            return result;
        }

        private IEnumerable<int> GetLeftXs(int x) =>
            Enumerable.Range(0, x);
        private IEnumerable<int> GetRightXs(int x, int width) =>
            Enumerable.Range(x + 1, width - x - 1);
        private IEnumerable<int> GetTopYs(int y) =>
            Enumerable.Range(0, y);
        private IEnumerable<int> GetBottomYs(int y, int height) =>
            Enumerable.Range(y + 1, height - y - 1);

        private bool IsLeftXValid(int current, int[,] grid, int x, int y, int offset) =>
            grid[x - offset - 1, y] < current;
        private bool IsRightXValid(int current, int[,] grid, int x, int y) =>
            grid[x, y] < current;
        private bool IsTopYValid(int current, int[,] grid, int x, int y, int offset) =>
            grid[x, y - offset - 1] < current;
        private bool IsBottomYValid(int current, int[,] grid, int x, int y) =>
            grid[x, y] < current;
    }
}
