namespace Training.DomainClasses;

public class Negation<T>(ICriteria<T> criteria) : ICriteria<T>
{
    public bool IsSatisfiedBy(T item)
    {
        return !criteria.IsSatisfiedBy(item);
    }
}
