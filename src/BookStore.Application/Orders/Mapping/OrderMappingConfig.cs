using BookStore.Contracts.Orders;
using BookStore.Domain.Orders;
using BookStore.Domain.Orders.Entities;
using Mapster;

namespace BookStore.Application.Orders.Mapping;

public class OrderMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig cfg)
    {
        cfg.NewConfig<Order, OrderResponse>()
            .Map(dest => dest.OrderId, src => src.Id.Value.ToString())
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.OrderedDate, src => src.OrderedDate)
            .Map(dest => dest.Status, src => src.Status.ToString())
            .Map(dest => dest.Items, src => src.Items)
            .Map(d => d.ShippingAddress, s => s.ShippingAddress)
            .Map(d => d.ShippingCost, s => s.ShippingCost)
            .Map(dest => dest.Total, src => src.Total);

        cfg.NewConfig<OrderItem, OrderItemResponse>()
            .Map(d => d.OrderItemId, s => s.Id.Value)
            .Map(d => d.BookId, s => s.Book.Id.Value.ToString())
            .Map(d => d.BookTitle, s => s.BookTitle)
            .Map(d => d.Quantity, s => s.Quantity)
            .Map(d => d.UnitPrice, s => s.UnitPrice)
            .Map(d => d.SubTotal, s => s.SubTotal);
    }
}
