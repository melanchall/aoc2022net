using Aoc2022Net.Utilities;

namespace Aoc2022Net.Days
{
    internal sealed class Day7 : Day
    {
        public override object SolvePart1()
        {
            var positions = InputData.GetCommaSeparatedInt32Numbers().OrderBy(n => n).ToArray();
            var medianPosition = positions[positions.Length / 2];
            return positions.Sum(p => Math.Abs(p - medianPosition));
        }

        public override object SolvePart2()
        {
            var positions = InputData.GetCommaSeparatedInt32Numbers();
            var averagePosition = (int)positions.Average();

            int CalculateFuel(int position) =>
                positions.Sum(p => (1..Math.Abs(p - position)).Enumerate().Sum());

            return Math.Min(
                CalculateFuel(averagePosition),
                CalculateFuel(averagePosition + 1));
        }
    }
}
