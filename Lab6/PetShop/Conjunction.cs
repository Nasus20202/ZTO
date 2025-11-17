namespace Training.DomainClasses;

public class Conjunction<T> : BinaryOperator<T>
{
    public Conjunction(ICriteria<T> first, ICriteria<T> second) : base(first, second)
    {
    }

    public override bool IsSatisfiedBy(T item)
    {
        return first.IsSatisfiedBy(item) && second.IsSatisfiedBy(item);
    }
}
