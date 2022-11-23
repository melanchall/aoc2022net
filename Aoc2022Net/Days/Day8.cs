namespace Aoc2022Net.Days
{
    internal sealed class Day8 : Day
    {
        private record Entry(string[] SignalPatterns, string[] OutputValues);

        private const int Pattern1Length = 2;
        private const int Pattern4Length = 4;
        private const int Pattern7Length = 3;
        private const int Pattern8Length = 7;

        public override object SolvePart1() => GetInputEntries()
            .Sum(e => e.OutputValues.Count(v =>
                v.Length == Pattern1Length ||
                v.Length == Pattern4Length ||
                v.Length == Pattern7Length ||
                v.Length == Pattern8Length));

        public override object SolvePart2()
        {
            var entries = GetInputEntries();
            var result = 0;

            foreach (var entry in entries)
            {
                var patterns = new string[10];

                patterns[1] = entry.SignalPatterns.First(d => d.Length == Pattern1Length);
                patterns[4] = entry.SignalPatterns.First(d => d.Length == Pattern4Length);
                patterns[7] = entry.SignalPatterns.First(d => d.Length == Pattern7Length);
                patterns[8] = entry.SignalPatterns.First(d => d.Length == Pattern8Length);

                var patterns069 = entry.SignalPatterns.Where(p => patterns[8].Except(p).Count() == 1).ToArray();
                patterns[9] = patterns069.First(p => patterns[4].Intersect(p).Count() == Pattern4Length);
                patterns[0] = patterns069.First(p => patterns[7].Intersect(p).Count() == Pattern7Length && p != patterns[9]);
                patterns[6] = patterns069.Except(new[] { patterns[9], patterns[0] }).First();

                var topRight = patterns[8].Except(patterns[6]).First();
                var bottomLeft = patterns[8].Except(patterns[9]).First();
                var patterns235 = entry.SignalPatterns.Where(p => patterns[8].Except(p).Count() == 2).ToArray();
                patterns[3] = patterns235.First(p => patterns[7].Intersect(p).Count() == Pattern7Length);
                patterns[5] = patterns235.First(p => p != patterns[3] && !patterns[8].Except(new[] { topRight, bottomLeft }.Concat(p)).Any());
                patterns[2] = patterns235.Except(new[] { patterns[5], patterns[3] }).First();

                var outputNumbers = entry
                    .OutputValues
                    .Select(value => Array.IndexOf(
                        patterns,
                        patterns.First(pattern => pattern.Length == value.Length && !pattern.Except(value).Any())))
                    .ToArray();

                result += Convert.ToInt32(string.Join(string.Empty, outputNumbers));
            }

            return result;
        }

        private Entry[] GetInputEntries()
        {
            string[] GetParts(string s) =>
                s.Trim().Split(' ').Select(part => part.Trim()).ToArray();

            return (from line in InputData.GetInputLines()
                    let entryParts = line.Split('|', StringSplitOptions.RemoveEmptyEntries)
                    select new Entry(
                        GetParts(entryParts[0]),
                        GetParts(entryParts[1]))).ToArray();
        }
    }
}
