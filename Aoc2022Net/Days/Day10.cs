namespace Aoc2022Net.Days
{
    internal sealed class Day10 : Day
    {
        private record LineInfo(char? IllegalCharacter, char[] UncompletedChars);

        public override object SolvePart1()
        {
            var linesInfo = GetLinesInfo();
            return linesInfo
                .Where(i => i.IllegalCharacter != null)
                .Sum(i => i.IllegalCharacter switch
                {
                    ')' => 3,
                    ']' => 57,
                    '}' => 1197,
                    '>' => 25137
                });
        }

        public override object SolvePart2()
        {
            var linesInfo = GetLinesInfo();
            var scores = linesInfo
                .Where(i => i.IllegalCharacter == null)
                .Select(i => i.UncompletedChars.Aggregate(0L, (res, c) => res * 5 + (c switch
                {
                    '(' => 1,
                    '[' => 2,
                    '{' => 3,
                    '<' => 4
                })))
                .OrderBy(s => s)
                .ToArray();
            return scores[scores.Length / 2];
        }

        private LineInfo[] GetLinesInfo()
        {
            var info = new List<LineInfo>();

            foreach (var line in InputData.GetInputLines())
            {
                var charactersStack = new Stack<char>();
                char? illegalCharacter = default;

                foreach (var c in line)
                {
                    if (c == '(' || c == '[' || c == '<' || c == '{')
                        charactersStack.Push(c);
                    else
                    {
                        var top = charactersStack.Peek();
                        if ((c == ')' && top != '(') ||
                            (c == ']' && top != '[') ||
                            (c == '>' && top != '<') ||
                            (c == '}' && top != '{'))
                        {
                            illegalCharacter = c;
                            break;
                        }

                        charactersStack.Pop();
                    }
                }

                info.Add(new(illegalCharacter, charactersStack.ToArray()));
            }

            return info.ToArray();
        }
    }
}
