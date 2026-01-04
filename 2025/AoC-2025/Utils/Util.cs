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

    public static IEnumerable<(T, T)> Permutations<T>(this IList<T> elements)
    {
        for (int i = 0; i < elements.Count - 1; i++)
        {
            for (int j = i + 1; j < elements.Count; j++)
            {
                yield return (elements[i], elements[j]);
            }
        }
    }

    public static IEnumerable<(T, T)> Permutations<T>(this IEnumerable<T> elements)
    {
        return elements.ToList().Permutations();
    }
}