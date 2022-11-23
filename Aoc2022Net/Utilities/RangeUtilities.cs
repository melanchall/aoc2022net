namespace Aoc2022Net.Utilities
{
    internal static class RangeUtilities
    {
        public static bool ContainsValue(this Range range, int value) =>
            value >= range.Start.Value && value <= range.End.Value;

        public static IEnumerable<int> Enumerate(this Range range) =>
            Enumerable.Range(range.Start.Value, range.End.Value - range.Start.Value + 1);
    }
}
