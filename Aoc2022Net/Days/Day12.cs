namespace Aoc2022Net.Days
{
    internal sealed class Day12 : Day
    {
        public override object SolvePart1() => CountFullPaths(false);

        public override object SolvePart2() => CountFullPaths(true);

        private int CountFullPaths(bool can)
        {
            const string startCaveName = "start";
            const string endCaveName = "end";

            var map = InputData.GetInputLines()
                .Select(line => line.Split('-'))
                .SelectMany(parts => new[]
                {
                    (From: parts[0], To: parts[1]),
                    (From: parts[1], To: parts[0])
                })
                .Where(connection => connection.From != endCaveName && connection.To != startCaveName)
                .GroupBy(connection => connection.From)
                .ToDictionary(
                    connectionsGroup => connectionsGroup.Key,
                    connectionsGroup => connectionsGroup.Select(connection => connection.To).ToArray());

            return FindPaths(startCaveName, map, new List<string>(), can, false).Count(p => p.Last() == endCaveName);
        }

        private static string[][] FindPaths(
            string fromCave,
            IDictionary<string, string[]> map,
            List<string> currentPath,
            bool canSingleSmallCaveBeVisitedTwice,
            bool pathContainsSmallCaveVisitedTwice)
        {
            if (!char.IsUpper(fromCave[0]) && currentPath.Contains(fromCave))
            {
                if (canSingleSmallCaveBeVisitedTwice && !pathContainsSmallCaveVisitedTwice)
                    pathContainsSmallCaveVisitedTwice = true;
                else
                    return Array.Empty<string[]>();
            }

            currentPath.Add(fromCave);
            if (!map.ContainsKey(fromCave))
                return new[] { new[] { fromCave } };

            IEnumerable<string[]> GetPathsFromCave(string toCave) =>
                FindPaths(toCave, map, currentPath.ToList(), canSingleSmallCaveBeVisitedTwice, pathContainsSmallCaveVisitedTwice)
                    .Select(path => new[] { fromCave }.Concat(path).ToArray());

            return map[fromCave].SelectMany(GetPathsFromCave).ToArray();
        }
    }
}
