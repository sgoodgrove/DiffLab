namespace DiffLab
{
    internal class Diff<T> : IDiff<T>
    {
        public Diff(T left, T right, DiffType type)
        {
            this.DiffType = type;
            Left = left;
            Right = right;
        }

        public DiffType DiffType { get; }

        public T Left { get; }

        public T Right { get; }

        public override string ToString() => $"{DiffType} Left: {Left} Right: {Right}";
    }
}