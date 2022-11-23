namespace Aoc2022Net.Days
{
    internal sealed class Day18 : Day
    {
        private abstract class Node
        {
            public Node Parent { get; set; }
        };

        private sealed class LeafNode : Node
        {
            public LeafNode(long value) => Value = value;

            public long Value { get; set; }

            public override string ToString() => Value.ToString();
        }

        private sealed class PairNode : Node
        {
            public PairNode(Node left, Node right)
            {
                Left = left;
                Right = right;
                left.Parent = right.Parent = this;
            }

            public Node Left { get; set; }

            public Node Right { get; set; }

            public override string ToString() => $"[{Left},{Right}]";
        }

        public override object SolvePart1()
        {
            var treesRoots = InputData.GetInputLines().Select(ParseTreeRoot).ToArray();
            return CalculateSumMagnitude(treesRoots);
        }

        public override object SolvePart2()
        {
            var lines = InputData.GetInputLines();
            return Enumerable
                .Range(0, lines.Length)
                .SelectMany(i => Enumerable.Range(0, lines.Length).Select(j => (i, j)))
                .Where(ij => ij.i != ij.j)
                .Max(ij =>
                {
                    var treesRoots = lines.Select(ParseTreeRoot).ToArray();
                    return CalculateSumMagnitude(treesRoots[ij.i], treesRoots[ij.j]);
                });
        }

        private static long CalculateSumMagnitude(params Node[] treesRoots)
        {
            var sum = treesRoots.Aggregate((result, node) => ReduceNode(new PairNode(result, node)));
            return CalculateMagnitude(sum);
        }

        private static long CalculateMagnitude(Node node)
        {
            if (node is LeafNode leafNode)
                return leafNode.Value;

            var pairNode = (PairNode)node;
            return CalculateMagnitude(pairNode.Left) * 3 +
                   CalculateMagnitude(pairNode.Right) * 2;
        }

        private static Node ReduceNode(Node node)
        {
            while (true)
            {
                var deepPairNode = FindDeepPairNode(node, 0);
                if (deepPairNode != null)
                    ExplodeDeepPairNode(deepPairNode);
                else
                {
                    var bigLeafNode = FindBigLeafNode(node);
                    if (bigLeafNode != null)
                        SplitBigLeafNode(bigLeafNode);
                    else
                        break;
                }
            }

            return node;
        }

        private static void ExplodeDeepPairNode(PairNode deepPairNode)
        {
            var leftLeafNode = FindLeftLeafNode(deepPairNode);
            if (leftLeafNode != null)
                leftLeafNode.Value += ((LeafNode)deepPairNode.Left).Value;

            var rightLeafNode = FindRightLeafNode(deepPairNode);
            if (rightLeafNode != null)
                rightLeafNode.Value += ((LeafNode)deepPairNode.Right).Value;

            ReplaceNode((PairNode)deepPairNode.Parent, deepPairNode, new LeafNode(0));
        }

        private static void SplitBigLeafNode(LeafNode bigLeafNode) =>
            ReplaceNode(
                (PairNode)bigLeafNode.Parent,
                bigLeafNode,
                new PairNode(
                    new LeafNode((long)Math.Floor(bigLeafNode.Value / 2.0)),
                    new LeafNode((long)Math.Ceiling(bigLeafNode.Value / 2.0))));

        private static void ReplaceNode(PairNode parent, Node oldChildNode, Node newChildNode)
        {
            newChildNode.Parent = parent;

            if (parent.Left == oldChildNode)
                parent.Left = newChildNode;
            else
                parent.Right = newChildNode;
        }

        private static LeafNode FindLeftLeafNode(PairNode node)
        {
            var parent = node.Parent as PairNode;
            if (parent == null)
                return null;

            if (parent.Right == node)
                return FindRightMostLeafNode(parent.Left) ?? FindLeftLeafNode(parent);

            return FindLeftLeafNode(parent);
        }

        private static LeafNode FindRightLeafNode(PairNode node)
        {
            var parent = node.Parent as PairNode;
            if (parent == null)
                return null;

            if (parent.Left == node)
                return FindLeftMostLeafNode(parent.Right) ?? FindRightLeafNode(parent);

            return FindRightLeafNode(parent);
        }

        private static LeafNode FindRightMostLeafNode(Node node)
        {
            if (node is LeafNode leafNode)
                return leafNode;

            var pairNode = (PairNode)node;
            return FindRightMostLeafNode(pairNode.Right) ?? FindRightMostLeafNode(pairNode.Left);
        }

        private static LeafNode FindLeftMostLeafNode(Node node)
        {
            if (node is LeafNode leafNode)
                return leafNode;

            var pairNode = (PairNode)node;
            return FindLeftMostLeafNode(pairNode.Left) ?? FindLeftMostLeafNode(pairNode.Right);
        }

        private static PairNode FindDeepPairNode(Node node, int level) => node is PairNode pairNode
            ? (level == 4
                ? pairNode
                : (FindDeepPairNode(pairNode.Left, level + 1) ?? FindDeepPairNode(pairNode.Right, level + 1)))
            : null;

        private static LeafNode FindBigLeafNode(Node node)
        {
            if (node == null)
                return null;

            if (node is LeafNode leafNode)
                return leafNode.Value >= 10 ? leafNode : null;

            var pairNode = (PairNode)node;
            return FindBigLeafNode(pairNode.Left) ?? FindBigLeafNode(pairNode.Right);
        }

        private static Node ParseTreeRoot(string input)
        {
            var index = 0;
            return ParseNode(null, input, ref index);
        }

        private static Node ParseNode(Node parent, string input, ref int index) => input[index] == '['
            ? ParsePairNode(parent, input, ref index)
            : ParseLeafNode(parent, input, ref index);

        private static LeafNode ParseLeafNode(Node parent, string input, ref int index)
        {
            var valueString = string.Empty;

            while (char.IsDigit(input[index]))
            {
                valueString += input[index++];
            }

            return new LeafNode(int.Parse(valueString))
            {
                Parent = parent
            };
        }

        private static PairNode ParsePairNode(Node parent, string input, ref int index)
        {
            index++; // skip [
            var left = ParseNode(parent, input, ref index);
            index++; // skip comma
            var right = ParseNode(parent, input, ref index);
            index++; // skip ]

            return new PairNode(left, right)
            {
                Parent = parent
            };
        }
    }
}
