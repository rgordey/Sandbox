// Domain/PurchaseOrderDetail.cs (new)
namespace Domain
{
    public class PurchaseOrderDetail
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public PurchaseOrder? Order { get; set; }
        public Product? Product { get; set; }
    }
}