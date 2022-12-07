// No Space Left On Device: https://adventofcode.com/2022/day/7

namespace Aoc2022Net.Days
{
    internal sealed class Day7 : Day
    {
        private sealed class Node
        {
            public Node(string name, Node parent)
            {
                Name = name;
                Parent = parent;
            }

            public string Name { get; }

            public long Size { get; set; }

            public Node Parent { get; }

            public List<Node> Children { get; } = new List<Node>();
        }

        public override object SolvePart1() => GetDirectories(GetFileSystem())
            .Where(d => d.Size <= 100000)
            .Sum(d => d.Size);

        public override object SolvePart2()
        {
            var fileSystem = GetFileSystem();
            var directories = GetDirectories(fileSystem);
            var requiredSize = 30000000 - (70000000 - fileSystem.Size);
            return directories
                .Select(d => d.Size)
                .OrderBy(s => s)
                .First(s => s >= requiredSize);
        }

        private IEnumerable<Node> GetDirectories(Node fileSystem)
        {
            var queue = new Queue<Node>(new[] { fileSystem });

            while (queue.Any())
            {
                var node = queue.Dequeue();
                yield return node;

                foreach (var child in node.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }

        private Node GetFileSystem()
        {
            var root = new Node("/", null);
            var currentNode = root;

            foreach (var line in InputData.GetInputLines())
            {
                var parts = line.Trim('$', ' ').Split(' ');
                if (parts.Length < 2)
                    continue;

                switch (parts[0])
                {
                    case "cd":
                        currentNode = parts[1] switch
                        {
                            "/" => root,
                            ".." => currentNode.Parent,
                            _ => currentNode.Children.OfType<Node>().First(n => n.Name == parts[1])
                        };
                        break;
                    case "dir":
                        currentNode.Children.Add(new Node(parts[1], currentNode));
                        break;
                    default:
                        var fileSize = long.Parse(parts[0]);
                        for (var node = currentNode; node != null; node = node.Parent)
                            node.Size += fileSize;
                        break;
                }
            }

            return root;
        }
    }
}
