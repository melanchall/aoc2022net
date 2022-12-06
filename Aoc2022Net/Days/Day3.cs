// Rucksack Reorganization: https://adventofcode.com/2022/day/3

namespace Aoc2022Net.Days
{
    internal sealed class Day3 : Day
    {
        public override object SolvePart1() => InputData
            .GetInputLines()
            .Sum(rucksack => CalculateTotalPriority(rucksack[..(rucksack.Length / 2)], rucksack[(rucksack.Length / 2)..]));

        public override object SolvePart2() => InputData
            .GetInputLines()
            .Chunk(3)
            .Sum(CalculateTotalPriority);

        private static int CalculateTotalPriority(params string[] chunks)
        {
            var commonItem = chunks.Aggregate((result, part) => new string(result.Intersect(part).ToArray())).First();
            return char.IsLower(commonItem)
                ? (commonItem - 'a') + 1
                : (commonItem - 'A') + 27;
        }
    }
}
