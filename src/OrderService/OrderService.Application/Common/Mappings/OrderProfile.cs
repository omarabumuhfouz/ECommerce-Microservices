using OrderService.Domain.Orders;

namespace OrderService.Domain.Mappings;


public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice.Value))
            .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount.Value));

        // 2. Map Order -> OrderDto
        CreateMap<Order, OrderDto>()
            // Unwrapping Strongly Typed IDs
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.BillingAddressId, opt => opt.MapFrom(src => src.BillingAddressId))
            .ForMember(dest => dest.ShippingAddressId, opt => opt.MapFrom(src => src.ShippingAddressId))
            
            // Mapping Value Objects to Primitives
            .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderNumber.Value))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.OrderStatus.ToString()))
            
            // Financials (Using the computed properties from your Order class)
            .ForMember(dest => dest.ShippingCost, opt => opt.MapFrom(src => src.ShippingCost.Value))
            .ForMember(dest => dest.TotalBaseAmount, opt => opt.MapFrom(src => src.TotalBaseAmount.Value))
            .ForMember(dest => dest.TotalDiscountAmount, opt => opt.MapFrom(src => src.TotalDiscountAmount.Value))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount.Value))
            
            // Nested Collection
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems));
    }
}
