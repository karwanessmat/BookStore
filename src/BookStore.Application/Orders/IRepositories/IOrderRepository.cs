using BookStore.Application.Abstractions.Interfaces.Persistence.Base;
using BookStore.Domain.Orders;
using BookStore.Domain.Orders.ValueObjects.Ids;

namespace BookStore.Application.Orders.IRepositories;

public interface IOrderRepository : IRepository<Order, OrderId>
{
}