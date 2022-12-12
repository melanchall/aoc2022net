using System;
using System.Linq;
using Aoc2022Net.Utilities;

namespace Aoc2022Net
{
    public sealed class InputData
    {
        private readonly string _input;

        public InputData(string input, bool trim) => _input = (trim ? input.Trim() : input);

        public string GetInputText() => _input;

        public string[] GetInputLines(bool skipEmpty = false, bool trim = true) => _input
            .Split(
                Environment.NewLine,
                trim ? StringSplitOptions.TrimEntries : StringSplitOptions.None)
            .Where(line => !skipEmpty || !string.IsNullOrWhiteSpace(line))
            .ToArray();

        public int[] GetInputInt32NumbersFromLines() => GetInputLines()
            .Select(int.Parse)
            .ToArray();

        public int[] GetCommaSeparatedInt32Numbers() => _input
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(int.Parse)
            .ToArray();

        public long[] GetInputInt64NumbersFromLines() => GetInputLines()
            .Select(long.Parse)
            .ToArray();

        public (int[,] Grid, int Width, int Height) GetInputInt32Grid(int margin, int fillValue) =>
            GetInputGrid(margin, fillValue, c => int.Parse(c.ToString()));

        public (char[,] Grid, int Width, int Height) GetInputCharGrid(int margin, char fillValue) =>
            GetInputGrid(margin, fillValue, c => c);

        public (char[,] Grid, int Width, int Height) GetInputCharGrid() =>
            GetInputGrid(0, ' ', c => c);

        public (T[,] Grid, int Width, int Height) GetInputGrid<T>(int margin, T fillValue, Func<char, T> convert)
        {
            var lines = GetInputLines();

            var width = lines[0].Length;
            var height = lines.Length;

            var gridWidth = width + margin * 2;
            var gridHeight = height + margin * 2;
            var grid = new T[gridWidth, gridHeight];

            void ForEachCoordinate(int width, int height, Action<(int X, int Y)> action) =>
                DataProvider.GetGridCoordinates(width, height).ToList().ForEach(action);

            ForEachCoordinate(gridWidth, gridHeight, p => grid[p.X, p.Y] = fillValue);
            ForEachCoordinate(width, height, p => grid[p.X + margin, p.Y + margin] = convert(lines[p.Y][p.X]));

            return (grid, width, height);
        }

        public (T[,] Grid, int Width, int Height) GetInputGrid<T>() where T : Enum => GetGrid<T>(GetInputLines());

        public string[] GetInputGroupsAsJoinedStrings() =>
            string.Join(" ", GetInputLines().Select(l => string.IsNullOrWhiteSpace(l) ? "$$" : l))
                .Split("$$", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        public string[][] GetInputLinesGroups() =>
            string.Join("%%", GetInputLines().Select(l => string.IsNullOrWhiteSpace(l) ? "$$" : l))
                .Split("$$", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(g => g.Split("%%", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                .ToArray();

        public static (T[,] Grid, int Width, int Height) GetGrid<T>(string[] lines)
            where T : Enum
        {
            var rules = ReflectionUtilities
                .GetAttributedEnumValues<T, GridSymbolAttribute>()
                .ToDictionary(v => v.Attribute.Symbol, v => v.Value);

            var width = lines.First().Length;
            var height = lines.Length;

            var grid = new T[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var symbol = lines[y][x];
                    if (!rules.TryGetValue(symbol, out var value))
                        throw new InvalidOperationException($"Failed to build a grid due to no rule for '{symbol}' symbol.");

                    grid[x, y] = value;
                }
            }

            return (grid, width, height);
        }
    }
}
