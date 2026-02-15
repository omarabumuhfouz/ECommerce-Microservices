namespace ShoppingCartService.Application.Carts.Mappings;

public class CartProfile : Profile
{
        public CartProfile()
        {
                // CreateMap<CartTestDto, Cart>()
                // .ConstructUsing(dto => Cart.Create(
                //         dto.Id,
                //         dto.CustomerId,
                //         dto.IsCheckedOut,
                //         dto.CreatedAt,
                //         dto.UpdatedAt
                // ).Value);


                // CreateMap<CartItemTestDto, CartItem>()
                // .ConstructUsing(dto => CartItem.Create(
                //                         dto.CartId,
                //                         dto.ProductId,
                //                         dto.Quantity,
                //                         dto.UnitPrice,
                //                         dto.Discount
                //                 ).Value);

        }
}
