using System;
using Training.DomainClasses;

namespace Training.Specificaton;

public static class Where<TItem>
{
    public static CriteriaBuilder<TItem, TProperty> HasAn<TProperty>(
        Func<TItem, TProperty> propertySelector)
    {
        return new CriteriaBuilder<TItem, TProperty>(propertySelector);
    }
}

public class CriteriaBuilder<TItem, TProperty>(Func<TItem, TProperty> propertySelector)
{
    public ICriteria<TItem> EqualTo(TProperty property)
    {
        return new AnonymousCriteria<TItem>(x => propertySelector(x).Equals(property)
        );
    }
}
