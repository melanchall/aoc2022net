namespace Aoc2022Net.Days
{
    internal sealed class Day2 : Day
    {
        private static readonly int[][] OutcomesTable =
        {
            new[] { 1, 2, 0 },
            new[] { 0, 1, 2 },
            new[] { 2, 0, 1 }
        };

        public override object SolvePart1() => CalculateTotalScore((abcI, xyzI) =>
            (xyzI + 1) + OutcomesTable[abcI][xyzI] * 3);

        public override object SolvePart2() => CalculateTotalScore((abcI, xyzI) =>
            (Array.IndexOf(OutcomesTable[abcI], xyzI) + 1) + xyzI * 3);

        private int CalculateTotalScore(Func<int, int, int> calculate)
        {
            const string abc = "ABC";
            const string xyz = "XYZ";

            return InputData.GetInputLines().Sum(line =>
            {
                var parts = line.Split(' ');
                var abcI = abc.IndexOf(parts[0][0]);
                var xyzI = xyz.IndexOf(parts[1][0]);
                return calculate(abcI, xyzI);
            });
        }
    }
}
