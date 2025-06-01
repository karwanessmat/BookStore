using BookStore.Contracts.ShoppingCards;
using BookStore.Domain.ShoppingCards;
using BookStore.Domain.ShoppingCards.Entities;
using Mapster;

namespace BookStore.Application.ShoppingCards.Mapping;

public class CartMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {

        config.NewConfig<Cart, CartResponse>()
            .Map(dest => dest.CartId, src => src.Id.Value.ToString())  
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.CreatedDate, src => src.CreatedDate)
            .Map(dest => dest.Items, src => src.Items)
            .Map(dest => dest.Total, src => src.Total)
            .Map(dest => dest.Currency, _ => "USD");         

        config.NewConfig<CartItem, CartItemResponse>()
            .Map(dest => dest.CartItemId, src => src.Id.Value)
            .Map(dest => dest.BookId, src => src.Book.Id.Value.ToString())
            .Map(dest => dest.BookTitle, src => src.BookTitle)
            .Map(dest => dest.Quantity, src => src.Quantity)
            .Map(dest => dest.UnitPrice, src => src.UnitPrice)
            .Map(dest => dest.SubTotal, src => src.SubTotal)
            .Map(dest => dest.Currency, _ => "USD");        



    }
}