using TDDLab.Core.InvoiceMgmt;
using TDDLab.Core.Tests.Helpers;

namespace TDDLab.Core.Tests.Processing;

public class InvoiceProcessorImplTests
{
    private const string ProductA = "ProductA";
    private const string ProductB = "ProductB";

    private readonly InvoiceProcessorImpl _processor = new InvoiceProcessorImpl();

    [Fact]
    public void Process_WhenProvidedInvalidInvoice_ShouldReturnFailure()
    {
        // Arrange
        var invalidInvoice = new InvoiceBuilder().WithEmptyLines().Build();

        // Act
        var result = _processor.Process(invalidInvoice);

        // Assert
        Assert.Equal(InvoiceResult.Failed, result.Result);
    }

    [Fact]
    public void Process_WhenProvidedValidInvoice_ShouldReturnSuccess()
    {
        // Arrange
        var validInvoice = new InvoiceBuilder().WithDefaultValues().Build();

        // Act
        var result = _processor.Process(validInvoice);

        // Assert
        Assert.Equal(InvoiceResult.Succeeded, result.Result);
    }

    [Fact]
    public void Process_WhenProvidedDuplicatedProducts_ShouldSumMoneyByProduct()
    {
        // Arrange
        var invoice = new InvoiceBuilder()
            .WithEmptyLines()
            .WithLine(ProductA, 200)
            .WithLine(ProductB, 300)
            .WithLine(ProductA, 150)
            .WithDiscount(0)
            .Build();

        // Act
        _processor.Process(invoice);

        // Assert
        Assert.Equal(350u, _processor.Products[ProductA].Amount);
        Assert.Equal(300u, _processor.Products[ProductB].Amount);
        Assert.Equal(2, _processor.Products.Count);
    }

    [Fact]
    // This should be fixed, but we can't change the code
    public void Process_ShouldThrowException_WhenTwoLinesForSameProductAndNoDiscountProvided()
    {
        // Arrange
        var invoice = new InvoiceBuilder()
            .WithEmptyLines()
            .WithLine(ProductA, 200)
            .WithLine(ProductA, 200)
            .WithNoDiscount()
            .Build();

        // Act & Assert
        Assert.Throws<NullReferenceException>(() => _processor.Process(invoice));
    }

    [Fact]
    // This should be fixed, but we can't change the code
    public void Process_ShouldThrowException_WhenNullInvoiceProvided()
    {
        // Act & Assert
        Assert.Throws<NullReferenceException>(() => _processor.Process(null));
    }

    [Fact]
    public void Process_ShouldSucceed_WhenNoDuplicateProductsAndNoDiscountProvided()
    {
        // Arrange
        var invoice = new InvoiceBuilder()
            .WithEmptyLines()
            .WithLine(ProductA, 200)
            .WithLine(ProductB, 200)
            .WithNoDiscount()
            .Build();

        // Act
        var result = _processor.Process(invoice);

        // Assert
        Assert.Equal(InvoiceResult.Succeeded, result.Result);
    }

    [Fact]
    public void Process_ShouldDiscountEachDuplicateProductLine_WhenDiscountIsApplied()
    {
        // Arrange
        var invoice = new InvoiceBuilder()
            .WithEmptyLines()
            .WithLine(ProductA, 200)
            .WithLine(ProductA, 200)
            .WithLine(ProductB, 300)
            .WithLine(ProductB, 300)
            .WithDiscount(200)
            .Build();

        // Act
        _processor.Process(invoice);

        // Assert
        Assert.Equal(200u, _processor.Products[ProductA].Amount);
        Assert.Equal(400u, _processor.Products[ProductB].Amount);
    }

    [Fact]
    public void Process_ShouldNotDiscountItemsToNegative_WhenItemPriceIsLowerThanDiscount()
    {
        // Arrange
        var invoice = new InvoiceBuilder()
            .WithEmptyLines()
            .WithLine(ProductA, 50)
            .WithLine(ProductA, 50)
            .WithLine(ProductA, 50)
            .WithDiscount(1000)
            .Build();

        // Act
        _processor.Process(invoice);

        // Assert
        Assert.Equal(50u, _processor.Products[ProductA].Amount);
    }

    [Fact]
    public void Process_ShouldFirstPreserveCurrency_WhenMultipleCurrenciesAreUsed()
    {
        // Arrange
        const string firstCurrency = "JPY";
        var invoice = new InvoiceBuilder()
            .WithEmptyLines()
            .WithLine(ProductA, 200, firstCurrency)
            .WithLine(ProductA, 150, "GBP")
            .WithDiscount(new Money(50, "EUR"))
            .Build();

        // Act
        _processor.Process(invoice);

        // Assert
        Assert.Equal(300u, _processor.Products[ProductA].Amount);
        Assert.Equal(firstCurrency, _processor.Products[ProductA].Currency);
    }

    [Fact]
    public void Process_ShouldAccumulateProductsAcrossInvoices_whenProcessedMultipleTimes()
    {
        // Arrange
        var invoice1 = new InvoiceBuilder().WithEmptyLines().WithLine(ProductA, 100).Build();
        var invoice2 = new InvoiceBuilder().WithEmptyLines().WithLine(ProductB, 200).Build();

        // Act
        _processor.Process(invoice1);
        _processor.Process(invoice2);

        // Assert
        Assert.Equal(2, _processor.Products.Count);
        Assert.Equal(100u, _processor.Products[ProductA].Amount);
        Assert.Equal(200u, _processor.Products[ProductB].Amount);
    }

    [Fact]
    public void Process_ShouldHandleDuplicateProductsAcrossInvoices_whenProcessedMultipleTimes()
    {
        // Arrange
        var invoice1 = new InvoiceBuilder()
            .WithDefaultValues()
            .WithEmptyLines()
            .WithLine(ProductA, 100)
            .WithDiscount(50)
            .Build();
        var invoice2 = new InvoiceBuilder()
            .WithDefaultValues()
            .WithEmptyLines()
            .WithLine(ProductA, 50)
            .WithDiscount(25)
            .Build();

        // Act
        _processor.Process(invoice1);
        _processor.Process(invoice2);
        _processor.Process(invoice2);
        _processor.Process(invoice1);

        // Assert
        Assert.Single(_processor.Products);
        Assert.Equal(200u, _processor.Products[ProductA].Amount);
    }
}
