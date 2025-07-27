namespace Domain
{
    public enum SalesOrderStatus
    {
        Placed,    // Initial status after creation
        Paid,      // After payment is confirmed
        Shipped,    // After shipment is dispatched
        Complete,  // After delivery and any final confirmations
        Cancelled  // Optional: For cancelled orders
    }
}