using Aoc2022Net.Tests.Attributes;
using NUnit.Framework;

namespace Aoc2022Net.Tests.Days
{
    [DayDataPart1(@"498,4 -> 498,6 -> 496,6
                    503,4 -> 502,4 -> 502,9 -> 494,9", 24)]
    [DayDataPart1(768)]
    [DayDataPart2(@"498,4 -> 498,6 -> 496,6
                    503,4 -> 502,4 -> 502,9 -> 494,9", 93)]
    [DayDataPart2(26686)]
    [TestFixture]
    public sealed class Day14Tests : DayTests<Day14Tests>
    {
    }
}
