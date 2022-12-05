using Aoc2022Net.Utilities;
using System.Text.RegularExpressions;

namespace Aoc2022Net.Days
{
    internal sealed class Day5 : Day
    {
        public Day5()
        {
            TrimInput = false;
        }

        public override object SolvePart1() =>
            GetFinalCrates(_ => _);

        public override object SolvePart2() =>
            GetFinalCrates(batch => batch.Reverse());

        private string GetFinalCrates(Func<IEnumerable<char>, IEnumerable<char>> processCratesBatch)
        {
            var (stacks, instructions) = GetData();

            foreach (var instruction in instructions)
            {
                var batch = processCratesBatch((0..(instruction.Count - 1)).Enumerate().Select(_ => stacks[instruction.From - 1].Pop())).ToArray();

                for (var i = 0; i < batch.Length; i++)
                {
                    stacks[instruction.To - 1].Push(batch[i]);
                }
            }

            return new string(stacks.Select(stack => stack.Peek()).ToArray());
        }

        private (Stack<char>[] Stacks, (int Count, int From, int To)[] Instructions) GetData()
        {
            var lines = InputData.GetInputLines(trim: false);
            var separatorIndex = Array.IndexOf(lines, string.Empty);

            var stacksCount = (lines[separatorIndex - 2].Length / 4) + 1;
            var stacks = (0..(stacksCount - 1))
                .Enumerate()
                .Select(stackIndex => new Stack<char>((0..(separatorIndex - 2))
                    .Enumerate()
                    .Select(stackRowIndex => lines[separatorIndex - stackRowIndex - 2][stackIndex * 4 + 1])
                    .Where(crate => crate != ' ')))
                .ToArray();

            var instructions = lines
                .Skip(separatorIndex + 1)
                .Select(line => Regex.Match(line, @"move (\d+) from (\d+) to (\d+)"))
                .Select(match => (Count: match.GetInt32Group(1), From: match.GetInt32Group(2), To: match.GetInt32Group(3)))
                .ToArray();

            return (stacks, instructions);
        }
    }
}
