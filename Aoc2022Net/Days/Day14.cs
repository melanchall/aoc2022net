// Regolith Reservoir: https://adventofcode.com/2022/day/14

namespace Aoc2022Net.Days
{
    internal sealed class Day14 : Day
    {
        private enum TileType
        {
            Air = 0,
            Sand,
            Rock
        }

        private enum MoveType
        {
            MoveFurther,
            FallIntoAbyss,
            TryAnotherMove
        }

        public override object SolvePart1() => Solve(GetGrid(), 0);

        public override object SolvePart2()
        {
            var grid = GetGrid();
            var width = grid.GetLength(0);
            var height = grid.GetLength(1);

            var xOffset = 500;
            var newGrid = new TileType[width + xOffset * 2, height + 2];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    newGrid[x + xOffset, y] = grid[x, y];
                }
            }

            for (var x = 0; x < newGrid.GetLength(0); x++)
            {
                newGrid[x, height + 1] = TileType.Rock;
            }

            return Solve(newGrid, xOffset);
        }

        private int Solve(TileType[,] grid, int xOffset)
        {
            const int sandSourceX = 500;
            const int sandSourceY = 0;

            var maxX = grid.GetLength(0) - 1 + xOffset;
            var maxY = grid.GetLength(1) - 1;

            var result = 0;

            while (true)
            {
                var sandPosition = (X: sandSourceX + xOffset, Y: sandSourceY);

                // No room for sand moving
                if (grid[sandPosition.X, sandPosition.Y + 1] != TileType.Air &&
                    grid[sandPosition.X - 1, sandPosition.Y + 1] != TileType.Air &&
                    grid[sandPosition.X + 1, sandPosition.Y + 1] != TileType.Air)
                    return result + 1 /* count sand source tile too */;

                while (true)
                {
                    if (MoveDown(ref sandPosition, grid, maxY) == MoveType.FallIntoAbyss)
                        return result;

                    switch (MoveLeftDown(ref sandPosition, grid))
                    {
                        case MoveType.FallIntoAbyss:
                            return result;
                        case MoveType.MoveFurther:
                            continue;
                    }

                    switch (MoveRightDown(ref sandPosition, grid, maxX))
                    {
                        case MoveType.FallIntoAbyss:
                            return result;
                        case MoveType.MoveFurther:
                            continue;
                    }

                    // Sand comes to rest
                    result++;
                    grid[sandPosition.X, sandPosition.Y] = TileType.Sand;
                    break;
                }
            }
        }

        private MoveType MoveDown(ref (int X, int Y) sandPosition, TileType[,] grid, int maxY)
        {
            for (var y = sandPosition.Y + 1; ; y++)
            {
                if (y > maxY)
                    return MoveType.FallIntoAbyss;

                if (grid[sandPosition.X, y] != TileType.Air)
                {
                    grid[sandPosition.X, sandPosition.Y] = TileType.Air;
                    sandPosition = (sandPosition.X, y - 1);
                    return MoveType.TryAnotherMove;
                }
            }
        }

        private MoveType MoveLeftDown(ref (int X, int Y) sandPosition, TileType[,] grid)
        {
            if (sandPosition.X - 1 < 0)
                return MoveType.FallIntoAbyss;

            if (grid[sandPosition.X - 1, sandPosition.Y + 1] == TileType.Air)
            {
                sandPosition = (sandPosition.X - 1, sandPosition.Y + 1);
                return MoveType.MoveFurther;
            }

            return MoveType.TryAnotherMove;
        }

        private MoveType MoveRightDown(ref (int X, int Y) sandPosition, TileType[,] grid, int maxX)
        {
            if (sandPosition.X + 1 > maxX)
                return MoveType.FallIntoAbyss;

            if (grid[sandPosition.X + 1, sandPosition.Y + 1] == TileType.Air)
            {
                sandPosition = (sandPosition.X + 1, sandPosition.Y + 1);
                return MoveType.MoveFurther;
            }

            return MoveType.TryAnotherMove;
        }

        private TileType[,] GetGrid()
        {
            var instructions = InputData
                .GetInputLines()
                .Select(line => line
                    .Split(" -> ")
                    .Select(p =>
                    {
                        var pp = p.Split(",");
                        return (X: int.Parse(pp[0]), Y: int.Parse(pp[1]));
                    })
                    .ToArray())
                .ToArray();

            var maxX = instructions.SelectMany(i => i.Select(p => p.X)).Max();
            var maxY = instructions.SelectMany(i => i.Select(p => p.Y)).Max();

            var grid = new TileType[maxX + 1, maxY + 1];

            foreach (var instruction in instructions)
            {
                for (var coordinateIndex = 1; coordinateIndex < instruction.Length; coordinateIndex++)
                {
                    var xStep = Math.Sign(instruction[coordinateIndex].X - instruction[coordinateIndex - 1].X);
                    var yStep = Math.Sign(instruction[coordinateIndex].Y - instruction[coordinateIndex - 1].Y);

                    for (var x = instruction[coordinateIndex - 1].X; ; x += xStep)
                    {
                        for (var y = instruction[coordinateIndex - 1].Y; ; y += yStep)
                        {
                            grid[x, y] = TileType.Rock;
                            if (y == instruction[coordinateIndex].Y)
                                break;
                        }

                        if (x == instruction[coordinateIndex].X)
                            break;
                    }
                }
            }

            return grid;
        }
    }
}
