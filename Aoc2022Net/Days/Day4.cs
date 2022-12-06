// Camp Cleanup: https://adventofcode.com/2022/day/4

using Aoc2022Net.Utilities;
using System.Text.RegularExpressions;

namespace Aoc2022Net.Days
{
    internal sealed class Day4 : Day
    {
        public override object SolvePart1() => GetAssignmentsPairs()
            .Count(r => r.First.Contains(r.Second) || r.Second.Contains(r.First));

        public override object SolvePart2() => GetAssignmentsPairs()
            .Count(r => r.First.Intersects(r.Second));

        private IEnumerable<(Range First, Range Second)> GetAssignmentsPairs() =>
            from line in InputData.GetInputLines()
            let match = Regex.Match(line, @"(\d+)-(\d+),(\d+)-(\d+)")
            select (match.GetInt32Group(1)..match.GetInt32Group(2),
                    match.GetInt32Group(3)..match.GetInt32Group(4));
    }
}
