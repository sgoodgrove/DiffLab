using System;
using System.Collections.Generic;
using System.Linq;

namespace DiffLab
{
    public class MemoizedDiffer<TObject>
    {
        private readonly TObject left;
        private readonly TObject right;

        private readonly Dictionary<(Type, Type, Type), List<IDiff<object>>> cache = new Dictionary<(Type, Type, Type), List<IDiff<object>>>();

        public MemoizedDiffer(TObject left, TObject right)
        {
            this.left = left;
            this.right = right;
        }

        public IEnumerable<IDiff<TItem>> Differences<TItem, TId>(Func<TObject, IEnumerable<TItem>> collectionSelector, Func<TItem, TId> idOf, Func<TItem, TItem, bool> equals)
        {
            if (!cache.ContainsKey((collectionSelector.GetType(), idOf.GetType(), equals.GetType())))
            {
                cache[(collectionSelector.GetType(), idOf.GetType(), equals.GetType())] = Differ.Differences(left, right, collectionSelector, idOf, equals).Cast<IDiff<object>>().ToList();
            }

            return cache[(collectionSelector.GetType(), idOf.GetType(), equals.GetType())].Cast<IDiff<TItem>>();
        }
    }
}
