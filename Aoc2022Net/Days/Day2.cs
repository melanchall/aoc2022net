using Aoc2022Net.Utilities;
using System.Text.RegularExpressions;

namespace Aoc2022Net.Days
{
    internal sealed class Day2 : Day
    {
        private record Change(int Horizontal, int SupportValue);

        private static readonly Dictionary<string, Change> DirectionsToChanges =
            new Dictionary<string, Change>
            {
                ["forward"] = new(1, 0),
                ["down"] = new(0, 1),
                ["up"] = new(0, -1)
            };

        public override object SolvePart1()
        {
            var position = GetPositionChanges().Aggregate(
                (Horizontal: 0, Depth: 0),
                (result, p) => (
                    result.Horizontal + p.Horizontal,
                    result.Depth + p.SupportValue));

            return position.Horizontal * position.Depth;
        }

        public override object SolvePart2()
        {
            var position = GetPositionChanges().Aggregate(
                (Horizontal: 0, Depth: 0, Aim: 0),
                (result, p) => (
                    result.Horizontal + p.Horizontal,
                    result.Depth + p.Horizontal * result.Aim,
                    result.Aim + p.SupportValue));

            return position.Horizontal * position.Depth;
        }

        private IEnumerable<Change> GetPositionChanges()
        {
            const string directionGroupName = "dir";
            const string valueGroupName = "val";

            var regex = new Regex($@"(?<{directionGroupName}>.+)\s(?<{valueGroupName}>\d+)");

            return from line in InputData.GetInputLines()
                   let match = regex.Match(line)
                   let instruction = new
                   {
                       Direction = match.GetStringGroup(directionGroupName),
                       Value = match.GetInt32Group(valueGroupName)
                    }
                   let step = DirectionsToChanges[instruction.Direction]
                   select new Change(
                       step.Horizontal * instruction.Value,
                       step.SupportValue * instruction.Value);
        }
    }
}
