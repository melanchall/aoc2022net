using Aoc2022Net.Tests.Attributes;
using NUnit.Framework;

namespace Aoc2022Net.Tests.Days
{
    [DayDataPart1(@"2-4,6-8
                    2-3,4-5
                    5-7,7-9
                    2-8,3-7
                    6-6,4-6
                    2-6,4-8", 2)]
    [DayDataPart1(536)]
    [DayDataPart2(@"2-4,6-8
                    2-3,4-5
                    5-7,7-9
                    2-8,3-7
                    6-6,4-6
                    2-6,4-8", 4)]
    [DayDataPart2(845)]
    [TestFixture]
    public sealed class Day4Tests : DayTests<Day4Tests>
    {
    }
}
