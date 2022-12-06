// Tuning Trouble: https://adventofcode.com/2022/day/6

namespace Aoc2022Net.Days
{
    internal sealed class Day6 : Day
    {
        public override object SolvePart1() => GetMarkerPosition(4);

        public override object SolvePart2() => GetMarkerPosition(14);

        private int GetMarkerPosition(int dataPacketSize)
        {
            var input = InputData.GetInputText();
            return Enumerable
                .Range(0, input.Length)
                .First(i => input[i..(i + dataPacketSize)].Distinct().Count() == dataPacketSize)
                + dataPacketSize;
        }
    }
}
