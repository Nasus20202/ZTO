using TDDLab.Core.InvoiceMgmt;
using TDDLab.Core.Tests.Helpers;

namespace TDDLab.Core.Tests.InvoiceMgmt;

public class RecipientTests
{
    private const string NameDescription = "Recipient name should be specified";
    private const string AddressDescription = "Address should be valid";

    [Fact]
    public void Validate_ValidRecipient_ShouldPassValidation()
    {
        // Arrange
        var recipient = DefaultsFactory.CreateDefaultRecipient();

        // Act
        var brokenRules = recipient.Validate();

        // Assert
        Assert.Empty(brokenRules);
        Assert.True(recipient.IsValid);
    }

    [Fact]
    public void Validate_EmptyName_ShouldFailValidation()
    {
        // Arrange
        var recipient = DefaultsFactory.CreateDefaultRecipient(name: string.Empty);

        // Act
        var brokenRules = recipient.Validate();

        // Assert
        Assert.Single(brokenRules);
        Assert.Equal(NameDescription, brokenRules.First().Description);
        Assert.False(recipient.IsValid);
    }

    [Fact]
    public void Validate_NullName_ShouldFailValidation()
    {
        // Arrange
        var recipient = DefaultsFactory.CreateDefaultRecipient(name: null!);

        // Act
        var brokenRules = recipient.Validate();

        // Assert
        Assert.Single(brokenRules);
        Assert.Equal(NameDescription, brokenRules.First().Description);
        Assert.False(recipient.IsValid);
    }

    [Fact]
    public void Validate_InvalidAddress_ShouldFailValidation()
    {
        // Arrange
        var invalidAddress = DefaultsFactory.CreateDefaultAddress(addressLine1: string.Empty);
        var recipient = DefaultsFactory.CreateDefaultRecipient(address: invalidAddress);

        // Act
        var brokenRules = recipient.Validate();

        // Assert
        Assert.Single(brokenRules);
        Assert.Equal(AddressDescription, brokenRules.First().Description);
        Assert.False(recipient.IsValid);
    }

    [Fact]
    public void Validate_NullAddress_ShouldFailValidation()
    {
        // Arrange
        var recipient = DefaultsFactory.CreateDefaultRecipient(address: null!);

        // Act
        var brokenRules = recipient.Validate();

        // Assert
        Assert.Single(brokenRules);
        Assert.Equal(AddressDescription, brokenRules.First().Description);
        Assert.False(recipient.IsValid);
    }
}
