using Aoc2022Net.Tests.Attributes;
using NUnit.Framework;

namespace Aoc2022Net.Tests.Days
{
    [DayDataPart1(@"1000
                    2000
                    3000

                    4000

                    5000
                    6000

                    7000
                    8000
                    9000

                    10000", 24000)]
    [DayDataPart1(72602)]
    [DayDataPart2(@"1000
                    2000
                    3000

                    4000

                    5000
                    6000

                    7000
                    8000
                    9000

                    10000", 45000)]
    [DayDataPart2(207410)]
    [TestFixture]
    public sealed class Day1Tests : DayTests<Day1Tests>
    {
    }
}
