using Aoc2022Net.Tests.Attributes;
using NUnit.Framework;

namespace Aoc2022Net.Tests.Days
{
    [DayDataPart1(
@"    [D]    
[N] [C]    
[Z] [M] [P]
    1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2", "CMZ")]
    [DayDataPart1("TLNGFGMFN")]
    [DayDataPart2(
@"    [D]    
[N] [C]    
[Z] [M] [P]
    1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2", "MCD")]
    [DayDataPart2("FGLQJCMBD")]
    [TestFixture]
    public sealed class Day5Tests : DayTests<Day5Tests>
    {
    }
}
