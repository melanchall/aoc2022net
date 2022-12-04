namespace Aoc2022Net.Utilities
{
    internal static class RangeUtilities
    {
        public static bool ContainsValue(this Range range, int value) =>
            value >= range.Start.Value && value <= range.End.Value;

        public static bool Contains(this Range range, Range anotherRange) =>
            anotherRange.Start.Value >= range.Start.Value && anotherRange.End.Value <= range.End.Value;

        public static bool Overlaps(this Range range, Range anotherRange) =>
            range.Enumerate().Intersect(anotherRange.Enumerate()).Any();

        public static IEnumerable<int> Enumerate(this Range range) =>
            Enumerable.Range(range.Start.Value, range.End.Value - range.Start.Value + 1);
    }
}
