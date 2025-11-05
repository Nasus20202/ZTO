using TDDLab.Core.InvoiceMgmt;

namespace TDDLab.Core.Tests.Helpers;

public class InvoiceBuilder
{
    private const string DefaultAddressLine1 = "Default Address Line 1";
    private const string DefaultCity = "Default City";
    private const string DefaultState = "Default State";
    private const string DefaultZip = "Default Zip";
    private const string DefaultName = "Default Name";
    private const string DefaultCurrency = "USD";
    private const string DefaultProductName = "Default Product";
    private const string DefaultInvoiceNumber = "Default Invoice Number";
    private const ulong DefaultAmount = 100;

    // Invalid values
    private const string InvalidString = "";
    private const ulong InvalidAmount = 0;

    private string _invoiceNumber = DefaultInvoiceNumber;
    private Recipient _recipient = CreateDefaultRecipient();
    private Address _billToAddress = CreateDefaultAddress();
    private List<InvoiceLine> _lines = [CreateDefaultInvoiceLine()];
    private Money? _discount = null;

    private static Address CreateDefaultAddress(
        string addressLine1 = DefaultAddressLine1,
        string city = DefaultCity,
        string state = DefaultState,
        string zip = DefaultZip
    )
    {
        return new Address(addressLine1, city, state, zip);
    }

    private static Recipient CreateDefaultRecipient(Address address, string name = DefaultName)
    {
        return new Recipient(name, address);
    }

    private static Recipient CreateDefaultRecipient(string name = DefaultName)
    {
        return CreateDefaultRecipient(CreateDefaultAddress(), name);
    }

    private static Money CreateDefaultMoney(
        ulong amount = DefaultAmount,
        string currency = DefaultCurrency
    )
    {
        return new Money(amount, currency);
    }

    private static InvoiceLine CreateDefaultInvoiceLine(
        Money money,
        string productName = DefaultProductName
    )
    {
        return new InvoiceLine(productName, money);
    }

    private static InvoiceLine CreateDefaultInvoiceLine(string productName = DefaultProductName)
    {
        return CreateDefaultInvoiceLine(CreateDefaultMoney(), productName);
    }

    public InvoiceBuilder WithInvoiceNumber(string invoiceNumber)
    {
        _invoiceNumber = invoiceNumber;
        return this;
    }

    public InvoiceBuilder WithRecipient(Recipient recipient)
    {
        _recipient = recipient;
        return this;
    }

    public InvoiceBuilder WithBillToAddress(Address address)
    {
        _billToAddress = address;
        return this;
    }

    public InvoiceBuilder WithEmptyLines()
    {
        _lines = [];
        return this;
    }

    public InvoiceBuilder WithLines(IEnumerable<InvoiceLine> lines)
    {
        _lines = lines.ToList();
        return this;
    }

    public InvoiceBuilder WithLine(InvoiceLine line)
    {
        _lines.Add(line);
        return this;
    }

    public InvoiceBuilder WithLine(
        string productName,
        ulong amount,
        string currency = DefaultCurrency
    )
    {
        _lines.Add(new InvoiceLine(productName, new Money(amount, currency)));
        return this;
    }

    public InvoiceBuilder WithDiscount(Money discount)
    {
        _discount = discount;
        return this;
    }

    public InvoiceBuilder WithDiscount(ulong amount, string currency = DefaultCurrency)
    {
        _discount = new Money(amount, currency);
        return this;
    }

    public InvoiceBuilder WithNoDiscount()
    {
        _discount = null;
        return this;
    }

    public InvoiceBuilder WithDefaultValues()
    {
        _invoiceNumber = DefaultInvoiceNumber;
        _recipient = CreateDefaultRecipient();
        _billToAddress = CreateDefaultAddress();
        _lines = [CreateDefaultInvoiceLine()];
        _discount = CreateDefaultMoney();
        return this;
    }

    public InvoiceBuilder WithInvalidInvoiceNumber()
    {
        _invoiceNumber = InvalidString;
        return this;
    }

    public InvoiceBuilder WithInvalidRecipient()
    {
        _recipient = new Recipient(InvalidString, CreateDefaultAddress());
        return this;
    }

    public InvoiceBuilder WithInvalidBillToAddress()
    {
        _billToAddress = new Address(InvalidString, DefaultCity, DefaultState, DefaultZip);
        return this;
    }

    public InvoiceBuilder WithInvalidLines()
    {
        _lines = [new InvoiceLine(InvalidString, new Money(InvalidAmount))];
        return this;
    }

    public InvoiceBuilder WithInvalidDiscount()
    {
        _discount = new Money(InvalidAmount, InvalidString);
        return this;
    }

    public Invoice Build()
    {
        return new Invoice(_invoiceNumber, _recipient, _billToAddress, _lines, _discount);
    }
}
