// Monkey in the Middle: https://adventofcode.com/2022/day/11

using Aoc2022Net.Utilities;
using System.Text.RegularExpressions;

namespace Aoc2022Net.Days
{
    internal sealed class Day11 : Day
    {
        private sealed class Item
        {
            public ulong WorryLevel { get; set; }
        }

        private sealed class Monkey
        {
            public ulong InspectingCount { get; set; }

            public ICollection<Item> Items { get; set; }

            public Func<ulong, ulong> Operation { get; set; }

            public int Test { get; set; }

            public int TestSuccessNext { get; set; }

            public int TestFailNext { get; set; }
        }

        public override object SolvePart1() => Solve(20, true);

        public override object SolvePart2() => Solve(10000, false);

        private ulong Solve(int roundsCount, bool divByThree)
        {
            var monkeys = (from g in InputData.GetInputLinesGroups()
                           let m = Regex.Match(g[1], @"Starting items: (.+)")
                           let items = m.Groups[1].Value.Split(", ").Select(i => new Item { WorryLevel = ulong.Parse(i) })
                           let op = Regex.Match(g[2], @"Operation: new = (.+)")
                           let operation = GetOperation(op.Groups[1].Value)
                           let t = Regex.Match(g[3], @"Test: divisible by (.+)")
                           let test = t.GetInt32Group(1)
                           let s = Regex.Match(g[4], @"If true: throw to monkey (.+)")
                           let success = s.GetInt32Group(1)
                           let f = Regex.Match(g[5], @"If false: throw to monkey (.+)")
                           let fail = f.GetInt32Group(1)
                           select new Monkey
                           {
                               Items = items.ToList(),
                               Operation = operation,
                               Test = test,
                               TestSuccessNext = success,
                               TestFailNext = fail
                           }).ToArray();

            for (var i = 0; i < roundsCount; i++)
            {
                foreach (var m in monkeys)
                {
                    foreach (var item in m.Items.ToArray())
                    {
                        m.InspectingCount++;

                        item.WorryLevel = m.Operation(item.WorryLevel);
                        if (divByThree)
                            item.WorryLevel /= 3;

                        var s = item.WorryLevel % (ulong)m.Test == 0;

                        m.Items.Remove(item);
                        monkeys[s ? m.TestSuccessNext : m.TestFailNext].Items.Add(item);
                    }
                }
            }

            return monkeys
                .Select(m => m.InspectingCount)
                .OrderByDescending(c => c)
                .Take(2)
                .Aggregate((x, y) => x * y);
        }

        private Func<ulong, ulong> GetOperation(string operation)
        {
            Func<ulong, ulong, ulong> calculate = operation.Split(' ')[1] switch
            {
                "+" => (x, y) => x + y,
                "*" => (x, y) => x * y
            };

            return level =>
            {
                var newExpressionParts = operation.Replace("old", level.ToString()).Split(' ');
                return calculate(
                    ulong.Parse(newExpressionParts[0]),
                    ulong.Parse(newExpressionParts[2]));
            };
        }
    }
}
