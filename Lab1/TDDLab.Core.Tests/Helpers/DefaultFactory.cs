using TDDLab.Core.InvoiceMgmt;

namespace TDDLab.Core.Tests.Helpers;

public static class DefaultsFactory
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

    public static Address CreateDefaultAddress(
        string addressLine1 = DefaultAddressLine1,
        string city = DefaultCity,
        string state = DefaultState,
        string zip = DefaultZip
    )
    {
        return new Address(addressLine1, city, state, zip);
    }

    public static Recipient CreateDefaultRecipient(Address address, string name = DefaultName)
    {
        return new Recipient(name, address);
    }

    public static Recipient CreateDefaultRecipient(string name = DefaultName)
    {
        return CreateDefaultRecipient(CreateDefaultAddress(), name);
    }

    public static Money CreateDefaultMoney(
        ulong amount = DefaultAmount,
        string currency = DefaultCurrency
    )
    {
        return new Money(amount, currency);
    }

    public static InvoiceLine CreateDefaultInvoiceLine(
        Money money,
        string productName = DefaultProductName
    )
    {
        return new InvoiceLine(productName, money);
    }

    public static InvoiceLine CreateDefaultInvoiceLine(string productName = DefaultProductName)
    {
        return CreateDefaultInvoiceLine(CreateDefaultMoney(), productName);
    }

    public static Invoice CreateDefaultInvoice(
        Recipient recipient,
        Address billToAddress,
        IEnumerable<InvoiceLine> lines,
        Money discount,
        string invoiceNumber = DefaultInvoiceNumber
    )
    {
        return new Invoice(invoiceNumber, recipient, billToAddress, lines, discount);
    }

    public static Invoice CreateDefaultInvoice(string invoiceNumber = DefaultInvoiceNumber)
    {
        return CreateDefaultInvoice(
            CreateDefaultRecipient(),
            CreateDefaultAddress(),
            [CreateDefaultInvoiceLine()],
            CreateDefaultMoney(),
            invoiceNumber
        );
    }
}
