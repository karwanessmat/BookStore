using BookStore.Application.Orders.IRepositories;
using BookStore.Domain.Orders;
using BookStore.Domain.Orders.ValueObjects.Ids;
using BookStore.Infrastructure.Shared.Persistence;
using BookStore.Infrastructure.Shared.Persistence.Repositories.Base;

namespace BookStore.Infrastructure.Orders;

public class OrderRepository(BookStoreAppContext logisticsDbContext)
    : Repository<Order, OrderId>(logisticsDbContext), IOrderRepository
{ }