// Distress Signal: https://adventofcode.com/2022/day/13

using Aoc2022Net.Utilities;

namespace Aoc2022Net.Days
{
    internal sealed class Day13 : Day
    {
        private abstract record Node();
        private sealed record IntNode(int Value) : Node;
        private sealed record ListNode(Node[] Children) : Node;

        private sealed record Packet(Node Node, string Line);

        public override object SolvePart1()
        {
            var pairs = GetPackets().Chunk(2).ToArray();
            return Enumerable
                .Range(0, pairs.Length)
                .Where(i => Compare(pairs[i].First(), pairs[i].Last()) < 0)
                .Sum(i => i + 1);
        }

        public override object SolvePart2()
        {
            const string divider1 = "[[2]]";
            const string divider2 = "[[6]]";

            var packets = new[]
                {
                    divider1,
                    divider2
                }
                .Select(ParsePacket)
                .Concat(GetPackets())
                .ToList();

            packets.Sort(Compare);

            return new[] { divider1, divider2 }.Product(d => packets.FindIndex(p => p.Line == d) + 1);
        }

        private Packet[] GetPackets() => InputData
            .GetInputLines()
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(ParsePacket)
            .ToArray();

        private int Compare(Packet left, Packet right) =>
            Compare(left.Node, right.Node);

        private int Compare(Node left, Node right) => left switch
        {
            IntNode leftInt => right switch
            {
                IntNode rightInt => Compare(leftInt, rightInt),
                ListNode rightList => Compare(new ListNode(new[] { leftInt }), rightList)
            },
            ListNode leftList => right switch
            {
                IntNode rightInt => Compare(leftList, new ListNode(new[] { rightInt })),
                ListNode rightList => Compare(leftList, rightList)
            }
        };

        private int Compare(IntNode left, IntNode right) =>
            left.Value - right.Value;

        private int Compare(ListNode left, ListNode right)
        {
            var leftChildren = left.Children;
            var rightChildren = right.Children;

            var count = Math.Max(leftChildren.Length, rightChildren.Length);

            for (var i = 0; i < count; i++)
            {
                if (i >= leftChildren.Length)
                    return -1;

                if (i >= rightChildren.Length)
                    return 1;

                var result = Compare(leftChildren[i], rightChildren[i]);
                if (result != 0)
                    return result;
            }

            return 0;
        }

        private Packet ParsePacket(string line)
        {
            var index = 0;
            return new Packet(ParseNode(line, ref index), line);
        }

        private Node ParseNode(string line, ref int index)
        {
            if (line[index] == ',')
                return null;

            if (line[index] == '[')
            {
                index++;
                return ParseList(line, ref index);
            }

            return ParseInt(line, ref index);
        }

        private ListNode ParseList(string line, ref int index)
        {
            var children = new List<Node>();

            for (; index < line.Length; index++)
            {
                if (line[index] == ']')
                    break;

                var node = ParseNode(line, ref index);
                if (node != null)
                {
                    children.Add(node);
                    index--;
                }
            }

            index++;
            return new ListNode(children.ToArray());
        }


        private IntNode ParseInt(string line, ref int index)
        {
            var intString = string.Empty;

            for (; index < line.Length; index++)
            {
                if (!char.IsDigit(line[index]))
                    break;

                intString += line[index];
            }

            return new IntNode(int.Parse(intString));
        }
    }
}
