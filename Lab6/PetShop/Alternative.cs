namespace Training.DomainClasses;

public class Alternative<T> : BinaryOperator<T>
{
    public Alternative(ICriteria<T> first, ICriteria<T> second) : base(first, second)
    {
    }

    public override bool IsSatisfiedBy(T item)
    {
        return first.IsSatisfiedBy(item) || second.IsSatisfiedBy(item);
    }
}
