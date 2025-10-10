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
}
