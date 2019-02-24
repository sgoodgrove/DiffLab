namespace DiffLab
{
    public interface IDiff<out T>
    {
        DiffType DiffType { get; }
        T Left { get; }
        T Right { get; }
    }
}