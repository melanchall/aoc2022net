// Cathode-Ray Tube: https://adventofcode.com/2022/day/10

namespace Aoc2022Net.Days
{
    internal sealed class Day10 : Day
    {
        public override object SolvePart1()
        {
            const int startCycleNumber = 20;
            const int cycleNumberStep = 40;

            var xValues = GetCycleFinishXValues();
            return Enumerable
                .Range(0, xValues.Length - startCycleNumber)
                .Where(cycleNumber => cycleNumber % cycleNumberStep == 0)
                .Sum(cycleNumber => (cycleNumber + startCycleNumber) * xValues[cycleNumber + startCycleNumber - 1]);
        }

        public override object SolvePart2()
        {
            const int crtHeight = 6;
            const int crtWidth = 40;

            var xValues = GetCycleFinishXValues();
            var symbols = new List<char>();

            for (var cycleNumber = 1; cycleNumber <= crtWidth * crtHeight; cycleNumber++)
            {
                var xValue = xValues[cycleNumber - 1];
                var crtX = (cycleNumber - 1) % crtWidth;
                symbols.Add(crtX >= xValue - 1 && crtX <= xValue + 1 ? '#' : '.');
            }

            return string.Join(Environment.NewLine, symbols.Chunk(crtWidth).Select(c => new string(c)));
        }

        private int[] GetCycleFinishXValues()
        {
            var x = 1;
            var result = new List<int> { x };

            foreach (var line in InputData.GetInputLines())
            {
                if (line == "noop")
                {
                    result.Add(x);
                    continue;
                }

                var increment = int.Parse(line.Split(' ')[1]);
                result.AddRange(new[] { x, x += increment });
            }

            return result.ToArray();
        }
    }
}
