using TDDLab.Core.InvoiceMgmt;
using TDDLab.Core.Tests.Helpers;

namespace TDDLab.Core.Tests.InvoiceMgmt;

public class InvoiceTests
{
    private const string InvoiceNumberDescription = "Invoice number should be specified";
    private const string BillingAddressDescription = "Billing address should be valid";
    private const string RecipientDescription = "Recipient should be valid";
    private const string DiscountDescription = "Discount should be valid";
    private const string LinesDescription = "Invoice lines should all be valid";

    [Fact]
    public void Validate_ValidInvoice_ShouldPassValidation()
    {
        // Arrange
        var invoice = DefaultsFactory.CreateDefaultInvoice();

        // Act
        var brokenRules = invoice.Validate();

        // Assert
        Assert.Empty(brokenRules);
        Assert.True(invoice.IsValid);
    }

    [Fact]
    public void Validate_EmptyInvoiceNumber_ShouldFailValidation()
    {
        // Arrange
        var invoice = DefaultsFactory.CreateDefaultInvoice(invoiceNumber: string.Empty);

        // Act
        var brokenRules = invoice.Validate();

        // Assert
        Assert.Single(brokenRules);
        Assert.Equal(InvoiceNumberDescription, brokenRules.First().Description);
        Assert.False(invoice.IsValid);
    }

    [Fact]
    public void Validate_NullInvoiceNumber_ShouldFailValidation()
    {
        // Arrange
        var invoice = DefaultsFactory.CreateDefaultInvoice(invoiceNumber: null!);

        // Act
        var brokenRules = invoice.Validate();

        // Assert
        Assert.Single(brokenRules);
        Assert.Equal(InvoiceNumberDescription, brokenRules.First().Description);
        Assert.False(invoice.IsValid);
    }

    [Fact]
    public void Validate_InvalidBillToAddress_ShouldFailValidation()
    {
        // Arrange
        var invalidAddress = DefaultsFactory.CreateDefaultAddress(addressLine1: string.Empty);
        var invoice = DefaultsFactory.CreateDefaultInvoice(
            recipient: DefaultsFactory.CreateDefaultRecipient(),
            billToAddress: invalidAddress,
            lines: [DefaultsFactory.CreateDefaultInvoiceLine()],
            discount: DefaultsFactory.CreateDefaultMoney()
        );

        // Act
        var brokenRules = invoice.Validate();

        // Assert
        Assert.Single(brokenRules);
        Assert.Equal(BillingAddressDescription, brokenRules.First().Description);
        Assert.False(invoice.IsValid);
    }

    [Fact]
    public void Validate_NullBillToAddress_ShouldFailValidation()
    {
        // Arrange
        var invoice = DefaultsFactory.CreateDefaultInvoice(
            recipient: DefaultsFactory.CreateDefaultRecipient(),
            billToAddress: null!,
            lines: [DefaultsFactory.CreateDefaultInvoiceLine()],
            discount: DefaultsFactory.CreateDefaultMoney()
        );

        // Act
        var brokenRules = invoice.Validate();

        // Assert
        Assert.Single(brokenRules);
        Assert.Equal(BillingAddressDescription, brokenRules.First().Description);
        Assert.False(invoice.IsValid);
    }

    [Fact]
    public void Validate_InvalidRecipient_ShouldFailValidation()
    {
        // Arrange
        var invalidRecipient = DefaultsFactory.CreateDefaultRecipient(name: string.Empty);
        var invoice = DefaultsFactory.CreateDefaultInvoice(
            recipient: invalidRecipient,
            billToAddress: DefaultsFactory.CreateDefaultAddress(),
            lines: [DefaultsFactory.CreateDefaultInvoiceLine()],
            discount: DefaultsFactory.CreateDefaultMoney()
        );

        // Act
        var brokenRules = invoice.Validate();

        // Assert
        Assert.Single(brokenRules);
        Assert.Equal(RecipientDescription, brokenRules.First().Description);
        Assert.False(invoice.IsValid);
    }

    [Fact]
    public void Validate_NullRecipient_ShouldFailValidation()
    {
        // Arrange
        var invoice = DefaultsFactory.CreateDefaultInvoice(
            recipient: null!,
            billToAddress: DefaultsFactory.CreateDefaultAddress(),
            lines: [DefaultsFactory.CreateDefaultInvoiceLine()],
            discount: DefaultsFactory.CreateDefaultMoney()
        );

        // Act
        var brokenRules = invoice.Validate();

        // Assert
        Assert.Single(brokenRules);
        Assert.Equal(RecipientDescription, brokenRules.First().Description);
        Assert.False(invoice.IsValid);
    }

    [Fact]
    public void Validate_InvalidDiscount_ShouldFailValidation()
    {
        // Arrange
        var invalidDiscount = DefaultsFactory.CreateDefaultMoney(currency: string.Empty);
        var invoice = DefaultsFactory.CreateDefaultInvoice(
            discount: invalidDiscount,
            recipient: DefaultsFactory.CreateDefaultRecipient(),
            billToAddress: DefaultsFactory.CreateDefaultAddress(),
            lines: [DefaultsFactory.CreateDefaultInvoiceLine()]
        );

        // Act
        var brokenRules = invoice.Validate();

        // Assert
        Assert.Single(brokenRules);
        Assert.Equal(DiscountDescription, brokenRules.First().Description);
        Assert.False(invoice.IsValid);
    }

    [Fact]
    public void Validate_NullDiscount_ShouldPassValidation()
    {
        // Arrange
        var invoice = DefaultsFactory.CreateDefaultInvoice(
            discount: null!,
            recipient: DefaultsFactory.CreateDefaultRecipient(),
            billToAddress: DefaultsFactory.CreateDefaultAddress(),
            lines: [DefaultsFactory.CreateDefaultInvoiceLine()]
        );

        // Act
        var brokenRules = invoice.Validate();

        // Assert
        Assert.Empty(brokenRules);
        Assert.True(invoice.IsValid);
    }

    [Fact]
    public void Validate_EmptyLines_ShouldFailValidation()
    {
        // Arrange
        var invoice = DefaultsFactory.CreateDefaultInvoice(
            lines: new List<InvoiceLine>(),
            recipient: DefaultsFactory.CreateDefaultRecipient(),
            billToAddress: DefaultsFactory.CreateDefaultAddress(),
            discount: DefaultsFactory.CreateDefaultMoney()
        );

        // Act
        var brokenRules = invoice.Validate();

        // Assert
        Assert.Single(brokenRules);
        Assert.Equal(LinesDescription, brokenRules.First().Description);
        Assert.False(invoice.IsValid);
    }

    [Fact]
    public void Validate_InvalidLine_ShouldFailValidation()
    {
        // Arrange
        var validLine = DefaultsFactory.CreateDefaultInvoiceLine();
        var invalidLine = DefaultsFactory.CreateDefaultInvoiceLine(productName: string.Empty);
        var invoice = DefaultsFactory.CreateDefaultInvoice(
            lines: [validLine, invalidLine],
            recipient: DefaultsFactory.CreateDefaultRecipient(),
            billToAddress: DefaultsFactory.CreateDefaultAddress(),
            discount: DefaultsFactory.CreateDefaultMoney()
        );

        // Act
        var brokenRules = invoice.Validate();

        // Assert
        Assert.Single(brokenRules);
        Assert.Equal(LinesDescription, brokenRules.First().Description);
        Assert.False(invoice.IsValid);
    }

    [Fact]
    public void AttachInvoiceLine_ValidLine_ShouldAttachSuccessfully()
    {
        // Arrange
        var invoice = DefaultsFactory.CreateDefaultInvoice();
        var newLine = DefaultsFactory.CreateDefaultInvoiceLine();

        // Act
        invoice.AttachInvoiceLine(newLine);

        // Assert
        Assert.Contains(newLine, invoice.Lines);
        Assert.Equal(invoice, newLine.Invoice);
    }
}
