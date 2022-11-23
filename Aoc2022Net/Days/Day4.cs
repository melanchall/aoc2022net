using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc2022Net.Days
{
    internal sealed class Day4 : Day
    {
        public override object SolvePart1() => CalculateFinalScore(true);

        public override object SolvePart2() => CalculateFinalScore(false);

        private int CalculateFinalScore(bool waitFirstWin)
        {
            var groups = InputData.GetInputLinesGroups();

            var numbers = groups[0][0].Split(",").Select(s => s.Trim()).Select(int.Parse);

            var boards = groups.Skip(1).Select((g, i) =>
            {
                var height = g.Length;
                var grid = Enumerable.Range(0, height).Select(r => g[r].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => n.Trim()).Select(n => int.Parse(n)).ToArray()).ToArray();

                var width = grid[0].Length;

                return new
                {
                    Rows = Enumerable.Range(0, height).Select(r => width).ToArray(),
                    Columns = Enumerable.Range(0, width).Select(c => height).ToArray(),
                    Grid = grid,
                    Index = i
                };
            }).ToArray();

            var taken = new List<int>();
            var boardsWin = new bool[boards.Length];

            foreach (var n in numbers)
            {
                taken.Add(n);

                foreach (var b in boards)
                {
                    if (boardsWin[b.Index])
                        continue;

                    var y = Enumerable.Range(0, b.Grid.Length).FirstOrDefault(r => b.Grid[r].Contains(n), -1);
                    if (y < 0)
                        continue;

                    var x = Array.IndexOf(b.Grid[y], n);

                    b.Rows[y]--;
                    b.Columns[x]--;

                    if (b.Rows.Contains(0) || b.Columns.Contains(0))
                    {
                        boardsWin[b.Index] = true;
                        if (waitFirstWin || boardsWin.All(_ => _))
                        {
                            var unmarkedSum = b.Grid.SelectMany(r => r.Where(nn => !taken.Contains(nn))).Sum();
                            return unmarkedSum * n;
                        }
                    }
                }
            }

            return -1;
        }
    }
}
