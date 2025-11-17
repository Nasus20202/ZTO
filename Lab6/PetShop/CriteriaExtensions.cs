namespace Training.DomainClasses
{
    public static class CriteriaExtensions
    {
        public static ICriteria<T> Or<T>(this ICriteria<T> left, ICriteria<T> right)
        {
            return new Alternative<T>(left, right);
        }

        public static ICriteria<T> And<T>(this ICriteria<T> left, ICriteria<T> right)
        {
            return new Conjunction<T>(left, right);
        }
    }
}
