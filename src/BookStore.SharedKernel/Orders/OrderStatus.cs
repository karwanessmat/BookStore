﻿namespace BookStore.SharedKernel.Orders;

public enum OrderStatus
{
    Pending = 0, 
    Paid = 1,
    Shipped = 2,
    Completed = 3,
    Cancelled = 4
}