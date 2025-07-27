namespace Domain
{
    public enum SalesOrderDetailStatus
    {
        Pending,     // Initial status
        Fulfilled,   // Item shipped/fulfilled
        Backordered, // Insufficient stock, awaiting restock
        Cancelled    // Item cancelled
    }
}