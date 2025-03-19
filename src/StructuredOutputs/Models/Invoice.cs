using System.ComponentModel;

namespace StructuredOutputs.Models;

public class Invoice
{
    [Description("Customer being invoiced, e.g. Microsoft Corp")]
    public string? CustomerName { get; set; }

    [Description("Mailing address for the customer, e.g. 123 Other St, Redmond WA, 98052")]
    public InvoiceAddress? CustomerAddress { get; set; }

    [Description("Purchase order reference number, e.g. PO-3333")]
    public string? PurchaseOrder { get; set; }

    [Description("ID for this specific invoice (often 'Invoice Number'), e.g. INV-100")]
    public string? InvoiceId { get; set; }

    [Description("Date the invoice was issued, e.g. 2019-11-15")]
    public DateTime? InvoiceDate { get; set; }

    [Description("Date payment for this invoice is due, e.g. 2019-12-15")]
    public DateTime? DueDate { get; set; }

    [Description("Vendor who has created this invoice, e.g. CONTOSO LTD.")]
    public string? VendorName { get; set; }

    [Description("Mailing address for the vendor, e.g. 123 456th St, New York, NY 10001")]
    public InvoiceAddress? VendorAddress { get; set; }

    [Description("Subtotal amount for the invoice before taxes and discounts, e.g. 100.00")]
    public double? SubTotal { get; set; }

    [Description("Total discount field identified on this invoice, e.g. 5.00")]
    public double? TotalDiscount { get; set; }

    [Description("Total tax field identified on this invoice, e.g. 10.00")]
    public double? TotalTax { get; set; }

    [Description("Total charges associated with this invoice, e.g. 110.00")]
    public double? InvoiceTotal { get; set; }

    [Description("Line items on the invoice")]
    public List<InvoiceLineItem>? Items { get; set; }
}

public class InvoiceAddress
{
    [Description("Street address, e.g. 123 Main St.")]
    public string? Street { get; set; }

    [Description("City, e.g. New York.")]
    public string? City { get; set; }

    [Description("State, e.g. NY.")]
    public string? State { get; set; }

    [Description("Postal code, e.g. 10001.")]
    public string? PostalCode { get; set; }

    [Description("Country, e.g. USA.")]
    public string? Country { get; set; }
}

public class InvoiceLineItem
{
    [Description("Product code, product number, or SKU associated with the specific line item, e.g. A123")]
    public string? ProductCode { get; set; }

    [Description("The text description for the invoice line item, e.g. Consulting service")]
    public string? Description { get; set; }

    [Description("The quantity of the line item, e.g. 2")]
    public int? Quantity { get; set; }

    [Description("The net or gross price (depending on the gross invoice setting of the invoice) of one unit of this item, e.g. 30.00")]
    public double? UnitPrice { get; set; }

    [Description("Tax associated with each line item, e.g. 6.00")]
    public double? Tax { get; set; }

    [Description("Tax rate associated with each line item, e.g. 20%")]
    public string? TaxRate { get; set; }

    [Description("The total amount of the line item, e.g. 60.00")]
    public double? Total { get; set; }
}
