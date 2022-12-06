// Calorie Counting: https://adventofcode.com/2022/day/1

namespace Aoc2022Net.Days
{
    internal sealed class Day1 : Day
    {
        public override object SolvePart1() => GetElvesCalories()
            .Max();

        public override object SolvePart2() => GetElvesCalories()
            .OrderByDescending(_ => _)
            .Take(3)
            .Sum();

        private IEnumerable<int> GetElvesCalories() => InputData
            .GetInputLinesGroups()
            .Select(g => g.Sum(int.Parse));
    }
}
