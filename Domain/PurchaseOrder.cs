// Domain/PurchaseOrder.cs (new)
using System.Numerics;

namespace Domain
{
    public class PurchaseOrder
    {
        public Guid Id { get; set; }
        public Guid VendorId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public Vendor? Vendor { get; set; }
        public List<PurchaseOrderDetail> OrderDetails { get; set; } = new();
    }
}