namespace BasicUtils.Tests.Validation;

public class BusinessRuleSetTests
{
    [Fact]
    public void BrokenBy_ValidObject_ShouldReturnEmptyList()
    {
        // Arrange
        var validatedObjectMock = new Mock<IValidatedObject>();
        validatedObjectMock.Setup(v => v.IsValid).Returns(true);

        var rules = CreateDefaultRules();
        var ruleSet = new BusinessRuleSet<IValidatedObject>(rules);

        // Act
        var brokenRules = ruleSet.BrokenBy(validatedObjectMock.Object);

        // Assert
        Assert.Empty(brokenRules);
        validatedObjectMock.Verify(v => v.IsValid, Times.Exactly(rules.Count));
    }

    [Fact]
    public void BrokenBy_InvalidObject_ShouldReturnBrokenRules()
    {
        // Arrange
        var validatedObjectMock = new Mock<IValidatedObject>();
        validatedObjectMock.SetupSequence(v => v.IsValid).Returns(true).Returns(false);

        var rules = CreateDefaultRules(3);
        var ruleSet = new BusinessRuleSet<IValidatedObject>(rules);

        // Act
        var brokenRules = ruleSet.BrokenBy(validatedObjectMock.Object);

        // Assert
        Assert.Equal(2, brokenRules.Count());
        Assert.Contains(rules[1], brokenRules);
        Assert.Contains(rules[2], brokenRules);
        validatedObjectMock.Verify(v => v.IsValid, Times.Exactly(rules.Count));
    }

    [Fact]
    public void Contains_RuleFound_ShouldReturnTrue()
    {
        // Arrange
        var rules = CreateDefaultRules();
        var ruleSet = new BusinessRuleSet<IValidatedObject>(rules);

        // Act
        var contains = ruleSet.Contains(rules[0]);

        // Assert
        Assert.True(contains);
    }

    [Fact]
    public void Contains_RuleNotFound_ShouldReturnFalse()
    {
        // Arrange
        var rules = CreateDefaultRules();
        var ruleSet = new BusinessRuleSet<IValidatedObject>(rules);
        var otherRule = CreateDefaultRule(name: "OtherRule");

        // Act
        var contains = ruleSet.Contains(otherRule);

        // Assert
        Assert.False(contains);
    }

    [Fact]
    public void Messages_NonEmpty_SShouldReturnAllMessageNames()
    {
        // Arrange
        var rules = CreateDefaultRules();
        var ruleSet = new BusinessRuleSet<IValidatedObject>(rules);

        // Act
        var messages = ruleSet.Messages;

        // Assert
        Assert.Equal(rules.Count, messages.Count);
        Assert.All(rules, r => messages.Contains(r.Name));
    }

    private IBusinessRule<IValidatedObject> CreateDefaultRule(
        string name = "TestRule",
        string description = "A test business rule"
    )
    {
        return new BusinessRule<IValidatedObject>(name, description, obj => obj.IsValid);
    }

    private IList<IBusinessRule<IValidatedObject>> CreateDefaultRules(int count = 2)
    {
        var rules = new List<IBusinessRule<IValidatedObject>>();
        Enumerable
            .Range(0, count)
            .ToList()
            .ForEach(i =>
                rules.Add(CreateDefaultRule($"TestRule{i}", $"A test business rule {i}"))
            );
        return rules;
    }
}
