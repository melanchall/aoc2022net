namespace Aoc2022Net.Utilities
{
    internal static class GridUtilities
    {
        public static IEnumerable<(int X, int Y)> GetAllCoordinates<T>(this T[,] grid) =>
            DataProvider.GetGridCoordinates(grid.GetLength(0), grid.GetLength(1));

        public static T GetGridSymbolType<T>(this char symbol) where T : Enum =>  ReflectionUtilities
            .GetAttributedEnumValues<T, GridSymbolAttribute>()
            .First(v => v.Attribute.Symbol == symbol)
            .Value;

        public static (T[,] NewGrid, int Width, int Height) AddMargin<T>(this T[,] grid, int margin, T fillValue)
        {
            var newGrid = new T[grid.GetLength(0) + margin * 2, grid.GetLength(1) + margin * 2];

            foreach (var (x, y) in newGrid.GetAllCoordinates())
            {
                newGrid[x, y] = fillValue;
            }

            foreach (var (x, y) in grid.GetAllCoordinates())
            {
                newGrid[x + margin, y + margin] = grid[x, y];
            }

            return (newGrid, newGrid.GetLength(0), newGrid.GetLength(1));
        }
    }
}
