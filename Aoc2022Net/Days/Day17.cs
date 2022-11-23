using Aoc2022Net.Utilities;
using System.Text.RegularExpressions;

namespace Aoc2022Net.Days
{
    internal sealed class Day17 : Day
    {
        private record TargetArea(int FromX, int ToX, int FromY, int ToY);

        public override object SolvePart1() => FindMaxY(GetTargetArea()).MaxY;

        public override object SolvePart2()
        {
            var targetArea = GetTargetArea();
            var maxYVelocity = FindMaxY(targetArea).MaxYVelocity;

            var initialVelocitiesCount = 0;

            for (var xVelocity = 1; xVelocity <= targetArea.ToX; xVelocity++)
            {
                for (var yVelocity = targetArea.FromY; yVelocity <= maxYVelocity; yVelocity++)
                {
                    var ySequence = GetYSequence(yVelocity).GetEnumerator();
                    ySequence.MoveNext();

                    var x = 0;
                    var xVelocityDelta = xVelocity;

                    while (true)
                    {
                        x += xVelocityDelta;
                        if (xVelocityDelta > 0)
                            xVelocityDelta--;

                        var y = ySequence.Current;
                        if (y < targetArea.FromY)
                            break;

                        if (x >= targetArea.FromX && x <= targetArea.ToX && y >= targetArea.FromY && y <= targetArea.ToY)
                        {
                            initialVelocitiesCount++;
                            break;
                        }

                        ySequence.MoveNext();
                    }
                }
            }

            return initialVelocitiesCount;
        }

        private (int MaxY, int MaxYVelocity) FindMaxY(TargetArea targetArea)
        {
            var maxY = int.MinValue;
            var maxYVelocity = 0;

            for (var xVelocity = 1; xVelocity <= targetArea.ToX; xVelocity++)
            {
                var badYCount = 0;

                for (var yVelocity = 1; badYCount < 1000; yVelocity++)
                {
                    foreach (var y in GetYSequence(yVelocity).Skip(xVelocity))
                    {
                        if (y < targetArea.FromY)
                        {
                            badYCount++;
                            break;
                        }

                        if (y <= targetArea.ToY)
                        {
                            maxY = Math.Max(maxY, GetYSequence(yVelocity).TakeWhile(y => y > 0).Max());
                            maxYVelocity = Math.Max(maxYVelocity, yVelocity);
                        }
                    }
                }
            }

            return (maxY, maxYVelocity);
        }

        private TargetArea GetTargetArea()
        {
            var input = InputData.GetInputText();
            var match = Regex.Match(input, @"target area: x=(\d+)\.\.(\d+), y=([\-\d]+)\.\.([\-\d]+)");
            return new(
                match.GetInt32Group(1),
                match.GetInt32Group(2),
                match.GetInt32Group(3),
                match.GetInt32Group(4));
        }

        private static IEnumerable<int> GetYSequence(int yVelocity)
        {
            var previousDelta = yVelocity;
            yield return yVelocity;

            while (true)
            {
                var delta = previousDelta - 1;
                yVelocity += delta;
                yield return yVelocity;
                previousDelta = delta;
            }
        }
    }
}
