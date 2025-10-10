namespace BasicUtils.Tests.Validation;

public class BusinessRuleTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsSatisfiedBy_ValidPredicate_ShouldResolvePredicate(bool isValid)
    {
        // Arrange
        var _validatedObjectMock = new Mock<IValidatedObject>();
        _validatedObjectMock.Setup(v => v.IsValid).Returns(isValid);
        bool wasPredicateCalled = false;

        var rule = CreateDefaultRule(
            CreateDefaultPredicate<IValidatedObject>((called) => wasPredicateCalled = called)
        );

        // Act
        var isValidSatisfied = rule.IsSatisfiedBy(_validatedObjectMock.Object);

        // Assert
        Assert.Equal(isValidSatisfied, isValid);
        Assert.True(wasPredicateCalled);
    }

    private IBusinessRule<T> CreateDefaultRule<T>(Predicate<T> predicate)
        where T : IValidatedObject
    {
        return new BusinessRule<T>("TestRule", "A test business rule", predicate);
    }

    private Predicate<T> CreateDefaultPredicate<T>(Action<bool> onPredicateCalled)
        where T : IValidatedObject
    {
        return (obj) =>
        {
            onPredicateCalled(true);
            return obj.IsValid;
        };
    }
}
