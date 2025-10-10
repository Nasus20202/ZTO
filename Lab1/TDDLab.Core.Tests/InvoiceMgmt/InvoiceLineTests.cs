using TDDLab.Core.InvoiceMgmt;
using TDDLab.Core.Tests.Helpers;

namespace TDDLab.Core.Tests.InvoiceMgmt;

public class InvoiceLineTests
{
    private const string ProductNameDescription = "Product name should be specified";
    private const string MoneyDescription = "Money should be valid";

    [Fact]
    public void Validate_ValidInvoiceLine_ShouldPassValidation()
    {
        // Arrange
        var invoiceLine = DefaultsFactory.CreateDefaultInvoiceLine();

        // Act
        var brokenRules = invoiceLine.Validate();

        // Assert
        Assert.Empty(brokenRules);
        Assert.True(invoiceLine.IsValid);
    }

    [Fact]
    public void Validate_EmptyProductName_ShouldFailValidation()
    {
        // Arrange
        var invoiceLine = DefaultsFactory.CreateDefaultInvoiceLine(productName: string.Empty);

        // Act
        var brokenRules = invoiceLine.Validate();

        // Assert
        Assert.Single(brokenRules);
        Assert.Equal(ProductNameDescription, brokenRules.First().Description);
        Assert.False(invoiceLine.IsValid);
    }

    [Fact]
    public void Validate_NullProductName_ShouldFailValidation()
    {
        // Arrange
        var invoiceLine = DefaultsFactory.CreateDefaultInvoiceLine(productName: null!);

        // Act
        var brokenRules = invoiceLine.Validate();

        // Assert
        Assert.Single(brokenRules);
        Assert.Equal(ProductNameDescription, brokenRules.First().Description);
        Assert.False(invoiceLine.IsValid);
    }

    [Fact]
    public void Validate_InvalidMoney_ShouldFailValidation()
    {
        // Arrange
        var invalidMoney = DefaultsFactory.CreateDefaultMoney(currency: string.Empty);
        var invoiceLine = DefaultsFactory.CreateDefaultInvoiceLine(money: invalidMoney);

        // Act
        var brokenRules = invoiceLine.Validate();

        // Assert
        Assert.Single(brokenRules);
        Assert.Equal(MoneyDescription, brokenRules.First().Description);
        Assert.False(invoiceLine.IsValid);
    }

    [Fact]
    public void Validate_NullMoney_ShouldFailValidation()
    {
        // Arrange
        var invoiceLine = new InvoiceLine("Default Product", null!);

        // Act
        var brokenRules = invoiceLine.Validate();

        // Assert
        Assert.Single(brokenRules);
        Assert.Equal(MoneyDescription, brokenRules.First().Description);
        Assert.False(invoiceLine.IsValid);
    }
}
