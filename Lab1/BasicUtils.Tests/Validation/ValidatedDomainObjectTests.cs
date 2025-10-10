namespace BasicUtils.Tests.Validation;

public class ValidatedDomainObjectTests
{
    [Fact]
    public void Validate_RulesetIsValid_ShouldReturnEmptyList()
    {
        // Arrange
        var ruleSetMock = CreateDefaultRuleSetMock();
        var validatedObject = new TestValidatedDomainObject(ruleSetMock.Object);

        // Act
        var result = validatedObject.Validate();

        // Assert
        Assert.Empty(result);
        ruleSetMock.Verify(r => r.BrokenBy(validatedObject), Times.Once);
    }

    [Fact]
    public void Validate_RulesetIsNotValid_ShouldReturnBrokenRules()
    {
        // Arrange
        var brokenRuleMock = new Mock<IRule>();
        var ruleSetMock = CreateDefaultRuleSetMock(new List<IRule> { brokenRuleMock.Object });
        var validatedObject = new TestValidatedDomainObject(ruleSetMock.Object);

        // Act
        var result = validatedObject.Validate();

        // Assert
        Assert.Single(result);
        Assert.Equal(brokenRuleMock.Object, result.First());
        ruleSetMock.Verify(r => r.BrokenBy(validatedObject), Times.Once);
    }

    [Fact]
    public void IsValid_RulesetIsValid_ShouldReturnTrue()
    {
        // Arrange
        var ruleSetMock = CreateDefaultRuleSetMock();
        var validatedObject = new TestValidatedDomainObject(ruleSetMock.Object);

        // Act
        var isValid = validatedObject.IsValid;

        // Assert
        Assert.True(isValid);
        ruleSetMock.Verify(r => r.BrokenBy(validatedObject), Times.Once);
    }

    [Fact]
    public void IsValid_RulesetIsNotValid_ShouldReturnFalse()
    {
        // Arrange
        var brokenRuleMock = new Mock<IRule>();
        var ruleSetMock = CreateDefaultRuleSetMock(new List<IRule> { brokenRuleMock.Object });
        var validatedObject = new TestValidatedDomainObject(ruleSetMock.Object);

        // Act
        var isValid = validatedObject.IsValid;

        // Assert
        Assert.False(isValid);
        ruleSetMock.Verify(r => r.BrokenBy(validatedObject), Times.Once);
    }

    private Mock<IBusinessRuleSet> CreateDefaultRuleSetMock(List<IRule>? brokenRules = null)
    {
        var ruleSetMock = new Mock<IBusinessRuleSet>();
        ruleSetMock
            .Setup(r => r.BrokenBy(It.IsAny<IValidatedObject>()))
            .Returns(brokenRules ?? new List<IRule>());
        return ruleSetMock;
    }

    private class TestValidatedDomainObject : ValidatedDomainObject
    {
        public TestValidatedDomainObject(IBusinessRuleSet ruleSet)
            : base()
        {
            Rules = ruleSet;
        }
    }
}
