using Aoc2022Net.Utilities;
using System.Text.RegularExpressions;

namespace Aoc2022Net.Days
{
    internal sealed class Day14 : Day
    {
        private record ElementCount(char Element, long Count);

        public override object SolvePart1() => CalculateDelta(10);

        public override object SolvePart2() => CalculateDelta(40);

        private long CalculateDelta(int steps)
        {
            var lines = InputData.GetInputLines(true);

            var template = lines[0];
            var rules = lines
                .Skip(1)
                .Select(line => Regex.Match(line, @"(\w+) -> (\w)"))
                .ToDictionary(
                    match => match.GetStringGroup(1),
                    match => match.GetStringGroup(2)[0]);

            var elementsCounts = template
                .GroupBy(element => element)
                .Select(countsGroup => new ElementCount(countsGroup.Key, countsGroup.LongCount()))
                .ToList();

            for (var i = 0; i < template.Length - 1; i++)
            {
                elementsCounts.AddRange(
                    CalculateElementsCounts(
                        $"{template[i]}{template[i + 1]}",
                        rules,
                        0,
                        steps,
                        new Dictionary<string, Dictionary<int, ElementCount[]>>()));
            }

            elementsCounts = elementsCounts
                .GroupBy(count => count.Element)
                .Select(countsGroup => new ElementCount(countsGroup.Key, countsGroup.Sum(count => count.Count)))
                .OrderByDescending(count => count.Count)
                .ToList();

            return elementsCounts.First().Count - elementsCounts.Last().Count;
        }

        private static ElementCount[] CalculateElementsCounts(
            string pair,
            Dictionary<string, char> rules,
            int step,
            int maxStep,
            Dictionary<string, Dictionary<int, ElementCount[]>> subResults)
        {
            if (step >= maxStep)
                return Array.Empty<ElementCount>();

            if (!subResults.TryGetValue(pair, out var countsBySteps))
                subResults.Add(pair, countsBySteps = new Dictionary<int, ElementCount[]>());

            if (countsBySteps.TryGetValue(step, out var elementsCounts))
                return elementsCounts;

            var middleElement = rules[pair];
            var leftPair = $"{pair[0]}{middleElement}";
            var rightPair = $"{middleElement}{pair[1]}";

            return countsBySteps[step] = new[] { new ElementCount(middleElement, 1L) }
                .Concat(CalculateElementsCounts(leftPair, rules, step + 1, maxStep, subResults))
                .Concat(CalculateElementsCounts(rightPair, rules, step + 1, maxStep, subResults))
                .GroupBy(count => count.Element)
                .Select(countsGroup => new ElementCount(countsGroup.Key, countsGroup.Sum(count => count.Count)))
                .ToArray();
        }
    }
}
