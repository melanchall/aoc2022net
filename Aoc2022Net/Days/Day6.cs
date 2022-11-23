namespace Aoc2022Net.Days
{
    internal sealed class Day6 : Day
    {
        public override object SolvePart1() => CountLanternfishes(80);

        public override object SolvePart2() => CountLanternfishes(256);

        private long CountLanternfishes(int days)
        {
            const int newLanternfishCounter = 8;
            const int resetLanternfishCounter = 6;

            var initialCounters = InputData.GetCommaSeparatedInt32Numbers();
            var counters = Enumerable
                .Range(0, newLanternfishCounter + 1)
                .ToDictionary(ic => ic, ic => (long)initialCounters.Count(c => c == ic));

            for (var i = 0; i < days; i++)
            {
                var newCounters = Enumerable.Range(0, newLanternfishCounter + 1).ToDictionary(c => c, _ => 0L);

                for (var j = newLanternfishCounter; j >= 0; j--)
                {
                    if (j == 0 && counters[0] > 0)
                    {
                        newCounters[newLanternfishCounter] += counters[0];
                        newCounters[resetLanternfishCounter] += counters[0];
                    }
                    else if (j > 0)
                        newCounters[j - 1] += counters[j];
                }

                counters = newCounters;
            }

            return counters.Values.Sum();
        }
    }
}
