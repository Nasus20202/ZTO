using System;
using System.Collections.Generic;
using Training.DomainClasses;

public static class EnumerableHelpers
{
    public static IEnumerable<TItem> OneAtATime<TItem>(this IEnumerable<TItem> items)
    {
        foreach (var item in items)
        {
            yield return item;
        }
    }

    public static IEnumerable<TItem> Filter<TItem>(this IEnumerable<TItem> items, Predicate<TItem> condition)
    {
        return items.Filter(new AnonnymousCryteria<TItem>(condition));
    }

    public static IEnumerable<TItem> Filter<TItem>(this IEnumerable<TItem> items, ICriteria<TItem> criteria)
    {
        foreach (var item in items)
        {
            if (criteria.IsSatisfiedBy(item))
            {
                yield return item;
            }
        }
    }
}

public class AnonnymousCryteria<TItem>(Predicate<TItem> condition) : ICriteria<TItem>
{
    public bool IsSatisfiedBy(TItem item)
    {
        return condition(item);
    }
}

public interface ICriteria<TItem>
{
    bool IsSatisfiedBy(TItem item);
}
