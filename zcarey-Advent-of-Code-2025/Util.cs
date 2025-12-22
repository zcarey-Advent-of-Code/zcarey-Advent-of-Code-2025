public static class Util
{
    public static ulong Sum(this IEnumerable<ulong> values)
    {
        ulong total = 0;
        foreach(ulong value in values)
        {
            total += value;
        }
        return total;
    }
}