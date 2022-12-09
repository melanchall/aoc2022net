using Aoc2022Net.Tests.Attributes;
using NUnit.Framework;

namespace Aoc2022Net.Tests.Days
{
    [DayDataPart1(@"R 4
                    U 4
                    L 3
                    D 1
                    R 4
                    D 1
                    L 5
                    R 2", 13)]
    [DayDataPart1(6090)]
    [DayDataPart2(@"R 4
                    U 4
                    L 3
                    D 1
                    R 4
                    D 1
                    L 5
                    R 2", 1)]
    [DayDataPart2(@"R 5
                    U 8
                    L 8
                    D 3
                    R 17
                    D 10
                    L 25
                    U 20", 36)]
    [DayDataPart2(2566)]
    [TestFixture]
    public sealed class Day9Tests : DayTests<Day9Tests>
    {
    }
}
