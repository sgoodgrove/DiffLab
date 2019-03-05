using System;
using System.Collections.Generic;
using System.Linq;

namespace DiffLab.CurrentPrevious
{
    public static class CurrentPreviousFacade
    {
        public enum DiffType
        {
            CurrentOnly,
            PreviousOnly,
            Same,
            Different
        }

        public static IEnumerable<U> AllCurrent<U>(this IEnumerable<Diff<U>> diffs) => diffs.Where(d => d.DiffType != DiffType.PreviousOnly).Select(d => d.Current);

        public static IEnumerable<U> AllPrevious<U>(this IEnumerable<Diff<U>> diffs) => diffs.Where(d => d.DiffType != DiffType.CurrentOnly).Select(d => d.Previous);

        public static bool AllSame<U>(this IEnumerable<Diff<U>> diffs) => diffs.All(d => d.DiffType == DiffType.Same);

        public static IEnumerable<U> CurrentOnly<U>(this IEnumerable<Diff<U>> diffs) => diffs.Where(d => d.DiffType == DiffType.CurrentOnly).Select(d => d.Current);

        public static IEnumerable<Diff<TItem>> Differences<TObject, TItem, TId>(TObject left, TObject right, Func<TObject, IEnumerable<TItem>> collectionSelector, Func<TItem, TId> idOf, Func<TItem, TItem, bool> equals)
        {
            var differences = Differ.Differences(left, right, collectionSelector, idOf, equals);
            return differences.Select(diff => new Diff<TItem>(diff));
        }

        public static IEnumerable<U> PreviousOnly<U>(this IEnumerable<Diff<U>> diffs) => diffs.Where(d => d.DiffType == DiffType.PreviousOnly).Select(d => d.Previous);

        public class Diff<T>
        {
            internal Diff(IDiff<T> diff)
            {
                this.DiffType = diff.DiffType.ToCurPrev();
                this.Previous = diff.Left;
                this.Current = diff.Right;
            }

            public T Current { get; }
            public DiffType DiffType { get; }

            public T Previous { get; }

            public override string ToString() => $"{DiffType} Previous: {Previous} Current: {Current}";
        }
    }

    public static class DiffTypeExtensions
    {
        public static CurrentPreviousFacade.DiffType ToCurPrev(this DiffLab.DiffType diffType)
        {
            switch (diffType)
            {
                case DiffLab.DiffType.Different:
                    return CurrentPreviousFacade.DiffType.Different;

                case DiffLab.DiffType.LeftOnly:
                    return CurrentPreviousFacade.DiffType.PreviousOnly;

                case DiffLab.DiffType.RightOnly:
                    return CurrentPreviousFacade.DiffType.CurrentOnly;

                case DiffLab.DiffType.Same:
                    return CurrentPreviousFacade.DiffType.Same;

                default:
                    throw new InvalidOperationException($"DiffType {diffType} not supported");
            }
        }
    }
}