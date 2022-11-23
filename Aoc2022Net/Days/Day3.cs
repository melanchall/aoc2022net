using Aoc2022Net.Utilities;

namespace Aoc2022Net.Days
{
    internal sealed class Day3 : Day
    {
        private const char ZeroBit = '0';
        private const char OneBit = '1';

        private record BitsInfo(char MostCommonBit, char LeastCommonBit, bool BothCountsEqual);
        private record DayInputData(string[] Lines, int Width, BitsInfo[] BitsInfo);

        public override object SolvePart1()
        {
            var (lines, width, bitsInfo) = GetInputData();

            int CalculateRate(bool takeMostCommonBit) =>
                Convert.ToInt32(
                    new string(Enumerable.Range(0, width).Select(bitPosition => takeMostCommonBit
                        ? bitsInfo[bitPosition].MostCommonBit
                        : bitsInfo[bitPosition].LeastCommonBit).ToArray()),
                    2);

            var gammaRate = CalculateRate(true);
            var epsilonRate = CalculateRate(false);

            return gammaRate * epsilonRate;
        }

        public override object SolvePart2()
        {
            var inputData = GetInputData();

            var oxygenRate = CalculateLifeSupportRate(inputData, true);
            var co2ScrubberRate = CalculateLifeSupportRate(inputData, false);

            return oxygenRate * co2ScrubberRate;
        }

        private int CalculateLifeSupportRate(DayInputData inputData, bool oxygenGenerator)
        {
            var numbers = inputData.Lines.ToList();

            for (var i = 0; i < inputData.Width; i++)
            {
                var bitsInfo = GetBitsInfo(numbers, inputData.Width)[i];
                var goodBit = oxygenGenerator
                    ? (bitsInfo.BothCountsEqual ? OneBit : bitsInfo.MostCommonBit)
                    : (bitsInfo.BothCountsEqual ? ZeroBit : bitsInfo.LeastCommonBit);
                numbers.RemoveAll(n => n[i] != goodBit);

                if (numbers.Count == 1)
                    break;
            }

            return Convert.ToInt32(numbers.First(), 2);
        }

        private DayInputData GetInputData()
        {
            var lines = InputData.GetInputLines();
            var width = lines[0].Length;
            return new DayInputData(lines, width, GetBitsInfo(lines, width));
        }

        private BitsInfo[] GetBitsInfo(ICollection<string> lines, int width)
        {
            var bitsInfo = from bitPosition in Enumerable.Range(0, width)
                           let bitsColumn = lines.Select(line => line[bitPosition]).ToArray()
                           let countsDifference = bitsColumn.CountValue(ZeroBit) - bitsColumn.CountValue(OneBit)
                           select new BitsInfo(
                               countsDifference > 0 ? ZeroBit : OneBit,
                               countsDifference > 0 ? OneBit : ZeroBit,
                               countsDifference == 0);
            return bitsInfo.ToArray();
        }
    }
}
