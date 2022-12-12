// Hill Climbing Algorithm: https://adventofcode.com/2022/day/12

using Aoc2022Net.Utilities;

namespace Aoc2022Net.Days
{
    internal sealed class Day12 : Day
    {
        private const char StartChar = 'S';
        private const char EndChar = 'E';
        private const char MinHeightChar = 'a';
        private const char MaxHeightChar = 'z';

        public override object SolvePart1() => GetShortestPathLength(StartChar);

        public override object SolvePart2() => GetShortestPathLength(StartChar, MinHeightChar);

        private int GetShortestPathLength(params char[] startPoints)
        {
            var (grid, width, height) = InputData.GetInputCharGrid();

            var coordinates = grid.GetAllCoordinates();
            var heightsGrid = new int[width, height];
            
            foreach (var c in coordinates)
            {
                var heightChar = grid[c.X, c.Y];
                heightsGrid[c.X, c.Y] = (heightChar == StartChar
                    ? MinHeightChar
                    : (heightChar == EndChar
                        ? MaxHeightChar
                        : heightChar)) - MinHeightChar;
            }

            var endPoint = coordinates.First(c => grid[c.X, c.Y] == EndChar);
            return coordinates
                .Where(c => startPoints.Contains(grid[c.X, c.Y]))
                .Select(p => GetShortestPathLength(p, endPoint, heightsGrid))
                .Min();
        }

        private int GetShortestPathLength((int X, int Y) sPoint, (int X, int Y) ePoint, int[,] heightsGrid)
        {
            var stepsQueue = new Queue<(int X, int Y, int Steps)>();
            stepsQueue.Enqueue((sPoint.X, sPoint.Y, 0));

            var visitedPoints = new List<(int X, int Y)> { sPoint };
            var width = heightsGrid.GetLength(0);
            var height = heightsGrid.GetLength(1);

            bool IsValidNextPoint((int X, int Y) nextPoint, (int X, int Y) currentPoint) =>
                !visitedPoints.Contains(nextPoint) &&
                nextPoint.X >= 0 &&
                nextPoint.X < width &&
                nextPoint.Y >= 0 &&
                nextPoint.Y < height &&
                (heightsGrid[nextPoint.X, nextPoint.Y] - heightsGrid[currentPoint.X, currentPoint.Y]) <= 1;

            while (stepsQueue.Any())
            {
                var currentPointSteps = stepsQueue.Dequeue();
                if (currentPointSteps.X == ePoint.X && currentPointSteps.Y == ePoint.Y)
                    return currentPointSteps.Steps;

                var nextPoints = new (int X, int Y)[]
                    {
                        (currentPointSteps.X - 1, currentPointSteps.Y),
                        (currentPointSteps.X + 1, currentPointSteps.Y),
                        (currentPointSteps.X, currentPointSteps.Y - 1),
                        (currentPointSteps.X, currentPointSteps.Y + 1),
                    }
                    .Where(p => IsValidNextPoint(p, (currentPointSteps.X, currentPointSteps.Y)));

                foreach (var p in nextPoints)
                {
                    visitedPoints.Add((p.X, p.Y));
                    stepsQueue.Enqueue((p.X, p.Y, currentPointSteps.Steps + 1));
                }
            }

            return int.MaxValue;
        }
    }
}
