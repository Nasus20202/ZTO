namespace Training.DomainClasses;

public abstract class BinaryOperator<T>: ICriteria<T>
{
    protected ICriteria<T> first { get; }
    protected ICriteria<T> second { get; }

    protected BinaryOperator(ICriteria<T> first, ICriteria<T> second)
    {
        this.first = first;
        this.second = second;
    }

    public abstract bool IsSatisfiedBy(T item);
}
