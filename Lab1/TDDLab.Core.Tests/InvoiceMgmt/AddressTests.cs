using TDDLab.Core.InvoiceMgmt;
using TDDLab.Core.Tests.Helpers;

namespace TDDLab.Core.Tests.InvoiceMgmt;

public class AddressTests
{
    private const string AddressLine1Description = "AddressLine1 should be specified";
    private const string CityDescription = "City should be specified";
    private const string StateDescription = "State should be properly specified";
    private const string ZipDescription = "Zip code should be specified";

    [Fact]
    public void Validate_ValidAddressLine1_ShouldPassValidation()
    {
        // Arrange
        var address = DefaultsFactory.CreateDefaultAddress();

        // Act
        var brokenRules = address.Validate();

        // Assert
        Assert.Empty(brokenRules);
        Assert.True(address.IsValid);
    }

    [Fact]
    public void Validate_EmptyAddressLine1_ShouldFailValidation()
    {
        // Arrange
        var address = DefaultsFactory.CreateDefaultAddress(addressLine1: string.Empty);

        // Act
        var brokenRules = address.Validate();

        // Assert
        Assert.Single(brokenRules);
        Assert.Equal(AddressLine1Description, brokenRules.First().Description);
        Assert.False(address.IsValid);
    }

    [Fact]
    public void Validate_EmptyCity_ShouldFailValidation()
    {
        // Arrange
        var address = DefaultsFactory.CreateDefaultAddress(city: string.Empty);

        // Act
        var brokenRules = address.Validate();

        // Assert
        Assert.Single(brokenRules);
        Assert.Equal(CityDescription, brokenRules.First().Description);
        Assert.False(address.IsValid);
    }

    [Fact]
    public void Validate_NullCity_ShouldFailValidation()
    {
        // Arrange
        var address = DefaultsFactory.CreateDefaultAddress(city: null!);

        // Act
        var brokenRules = address.Validate();

        // Assert
        Assert.Single(brokenRules);
        Assert.Equal(CityDescription, brokenRules.First().Description);
        Assert.False(address.IsValid);
    }

    [Fact]
    public void Validate_EmptyZip_ShouldFailValidation()
    {
        // Arrange
        var address = DefaultsFactory.CreateDefaultAddress(zip: string.Empty);

        // Act
        var brokenRules = address.Validate();

        // Assert
        Assert.Single(brokenRules);
        Assert.Equal(ZipDescription, brokenRules.First().Description);
        Assert.False(address.IsValid);
    }

    [Fact]
    public void Validate_NullZip_ShouldFailValidation()
    {
        // Arrange
        var address = DefaultsFactory.CreateDefaultAddress(zip: null!);

        // Act
        var brokenRules = address.Validate();

        // Assert
        Assert.Single(brokenRules);
        Assert.Equal(ZipDescription, brokenRules.First().Description);
        Assert.False(address.IsValid);
    }

    [Fact]
    public void Validate_EmptyState_ShouldFailValidation()
    {
        // Arrange
        var address = DefaultsFactory.CreateDefaultAddress(state: string.Empty);

        // Act
        var brokenRules = address.Validate();

        // Assert
        Assert.Single(brokenRules);
        Assert.Equal(StateDescription, brokenRules.First().Description);
        Assert.False(address.IsValid);
    }

    [Fact]
    public void Validate_NullState_ShouldFailValidation()
    {
        // Arrange
        var address = DefaultsFactory.CreateDefaultAddress(state: null!);

        // Act
        var brokenRules = address.Validate();

        // Assert
        Assert.Single(brokenRules);
        Assert.Equal(StateDescription, brokenRules.First().Description);
        Assert.False(address.IsValid);
    }
}
