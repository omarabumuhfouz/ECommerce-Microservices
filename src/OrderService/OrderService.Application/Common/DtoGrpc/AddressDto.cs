using Contracts.Customer;

namespace OrderService.Domain.DTOs;

public class AddressDto
{
    public Guid Id { get; set; }
    public string State { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public static AddressDto FromGrpcModel(AddressModel model)
    {
        return new()
        {
            Id = Guid.Parse(model.Id),
            State = model.State,
            City = model.City,
            Country = model.Country
        };
    }

}
