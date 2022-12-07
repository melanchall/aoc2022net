using Aoc2022Net.Tests.Attributes;
using NUnit.Framework;

namespace Aoc2022Net.Tests.Days
{
    [DayDataPart1(@"$ cd /
                    $ ls
                    dir a
                    14848514 b.txt
                    8504156 c.dat
                    dir d
                    $ cd a
                    $ ls
                    dir e
                    29116 f
                    2557 g
                    62596 h.lst
                    $ cd e
                    $ ls
                    584 i
                    $ cd ..
                    $ cd ..
                    $ cd d
                    $ ls
                    4060174 j
                    8033020 d.log
                    5626152 d.ext
                    7214296 k", 95437)]
    [DayDataPart1(2104783)]
    [DayDataPart2(@"$ cd /
                    $ ls
                    dir a
                    14848514 b.txt
                    8504156 c.dat
                    dir d
                    $ cd a
                    $ ls
                    dir e
                    29116 f
                    2557 g
                    62596 h.lst
                    $ cd e
                    $ ls
                    584 i
                    $ cd ..
                    $ cd ..
                    $ cd d
                    $ ls
                    4060174 j
                    8033020 d.log
                    5626152 d.ext
                    7214296 k", 24933642)]
    [DayDataPart2(5883165)]
    [TestFixture]
    public sealed class Day7Tests : DayTests<Day7Tests>
    {
    }
}
