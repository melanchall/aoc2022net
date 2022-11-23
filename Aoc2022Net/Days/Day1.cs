namespace Aoc2022Net.Days
{
    internal sealed class Day1 : Day
    {
        public override object SolvePart1() =>
            GetIncreasingElementsCount(InputData.GetInputInt32NumbersFromLines());

        public override object SolvePart2()
        {
            var numbers = InputData.GetInputInt32NumbersFromLines();
            var sums = Enumerable
                .Range(0, numbers.Length - 2)
                .Select(i => numbers[i] + numbers[i + 1] + numbers[i + 2])
                .ToArray();

            return GetIncreasingElementsCount(sums);
        }

        private int GetIncreasingElementsCount(int[] numbers) => Enumerable
            .Range(0, numbers.Length - 1)
            .Select(i => (Previous: numbers[i], Next: numbers[i + 1]))
            .Count(pair => pair.Next > pair.Previous);
    }
}
