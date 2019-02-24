using System;
using System.Collections.Generic;
using System.Linq;

namespace DiffLab
{

    public static class Differ
    {
        public static IEnumerable<U> AllLeft<U>(this IEnumerable<IDiff<U>> diffs) => diffs.Where(d => d.DiffType != DiffType.RightOnly).Select(d => d.Left);

        public static IEnumerable<U> AllRight<U>(this IEnumerable<IDiff<U>> diffs) => diffs.Where(d => d.DiffType != DiffType.LeftOnly).Select(d => d.Right);

        public static bool AllSame<U>(this IEnumerable<IDiff<U>> diffs) => diffs.All(d => d.DiffType == DiffType.Same);

        public static IEnumerable<IDiff<TItem>> Differences<TObject, TItem, TId>(TObject left, TObject right, Func<TObject, IEnumerable<TItem>> collectionSelector, Func<TItem, TId> idOf, Func<TItem, TItem, bool> equals)
        {
            var leftDict = (collectionSelector(left) ?? new List<TItem>()).ToDictionary(idOf);
            var rightDict = (collectionSelector(right) ?? new List<TItem>()).ToDictionary(idOf);

            var leftOnly = leftDict.Keys.Where(k => !rightDict.Keys.Contains(k)).Select(k => new Diff<TItem>(leftDict[k], default(TItem), DiffType.LeftOnly));
            var both = leftDict.Keys.Join(rightDict.Keys, x => x, x => x, (p, c) => new Diff<TItem>(leftDict[p], rightDict[c], equals(leftDict[p], rightDict[c]) ? DiffType.Same : DiffType.Different));
            var rightOnly = rightDict.Keys.Where(k => !leftDict.Keys.Contains(k)).Select(k => new Diff<TItem>(default(TItem), rightDict[k], DiffType.RightOnly));

            return rightOnly.Concat(both).Concat(leftOnly);
        }

        public static IEnumerable<U> LeftOnly<U>(this IEnumerable<IDiff<U>> diffs) => diffs.Where(d => d.DiffType == DiffType.LeftOnly).Select(d => d.Left);

        public static IEnumerable<U> RightOnly<U>(this IEnumerable<IDiff<U>> diffs) => diffs.Where(d => d.DiffType == DiffType.RightOnly).Select(d => d.Right);
    }
}