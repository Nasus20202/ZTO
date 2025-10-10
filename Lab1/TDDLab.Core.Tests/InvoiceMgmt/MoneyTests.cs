using TDDLab.Core.InvoiceMgmt;
using TDDLab.Core.Tests.Helpers;

namespace TDDLab.Core.Tests.InvoiceMgmt;

public class MoneyTests
{
    private const string CurrencyDescription = "Currency should be specified";

    [Fact]
    public void Validate_ValidMoney_ShouldPassValidation()
    {
        // Arrange
        var money = DefaultsFactory.CreateDefaultMoney();

        // Act
        var brokenRules = money.Validate();

        // Assert
        Assert.Empty(brokenRules);
        Assert.True(money.IsValid);
    }

    [Fact]
    public void Validate_EmptyCurrency_ShouldFailValidation()
    {
        // Arrange
        var money = DefaultsFactory.CreateDefaultMoney(currency: string.Empty);

        // Act
        var brokenRules = money.Validate();

        // Assert
        Assert.Single(brokenRules);
        Assert.Equal(CurrencyDescription, brokenRules.First().Description);
        Assert.False(money.IsValid);
    }

    [Fact]
    public void Validate_NullCurrency_ShouldFailValidation()
    {
        // Arrange
        var money = DefaultsFactory.CreateDefaultMoney(currency: null!);

        // Act
        var brokenRules = money.Validate();

        // Assert
        Assert.Single(brokenRules);
        Assert.Equal(CurrencyDescription, brokenRules.First().Description);
        Assert.False(money.IsValid);
    }

    [Fact]
    public void Addition_WithValidOperands_ShouldAddAmounts()
    {
        // Arrange
        var money1 = DefaultsFactory.CreateDefaultMoney(100);
        var money2 = DefaultsFactory.CreateDefaultMoney(50);

        // Act
        var result = money1 + money2;

        // Assert
        Assert.Equal(money1.Amount + money2.Amount, result.Amount);
    }

    [Fact]
    public void Subtraction_WhenLeftIsGreater_ShouldSubtractAmounts()
    {
        // Arrange
        var money1 = new Money(100);
        var money2 = new Money(30);

        // Act
        var result = money1 - money2;

        // Assert
        Assert.Equal(money1.Amount - money2.Amount, result.Amount);
    }

    [Fact]
    public void Subtraction_WhenLeftIsSmaller_ShouldReturnZero()
    {
        // Arrange
        var money1 = new Money(30);
        var money2 = new Money(100);

        // Act
        var result = money1 - money2;

        // Assert
        Assert.Equal(0ul, result.Amount);
    }
}
