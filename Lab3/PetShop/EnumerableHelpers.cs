using System.Collections.Generic;
using Training.DomainClasses;

public static class EnumerableHelpers
{
    public static IEnumerable<TItem> OneAtATime<TItem>(
        this IList<TItem> items
    )
    {
        foreach (var item in items)
        {
            yield return item;
        }
    }
}
