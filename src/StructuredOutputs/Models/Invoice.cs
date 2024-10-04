namespace StructuredOutputs.Models;

public class Invoice
{
    public string? InvoiceNumber { get; set; }

    public string? PurchaseOrderNumber { get; set; }

    public string? CustomerName { get; set; }

    public string? CustomerAddress { get; set; }

    public string? DeliveryDate { get; set; }

    public string? PayableBy { get; set; }

    public List<Product>? Products { get; set; }

    public List<Product>? Returns { get; set; }

    public double? TotalQuantity { get; set; }

    public double? TotalPrice { get; set; }

    public List<Signature>? ProductsSignatures { get; set; }

    public List<Signature>? ReturnsSignatures { get; set; }

    public class Product
    {
        public string? Id { get; set; }

        public string? Description { get; set; }

        public double? UnitPrice { get; set; }

        public double Quantity { get; set; }

        public double? Total { get; set; }

        public string? Reason { get; set; }
    }

    public class Signature
    {
        public string? Type { get; set; }

        public string? Name { get; set; }

        public bool? IsSigned { get; set; }
    }
}

