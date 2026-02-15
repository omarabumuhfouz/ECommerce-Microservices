using CustomerService.Application.Addresses.Commands.CreateAddress;
using CustomerService.Application.Customers.Commands.AddCustomer;
using AddressDto = CustomerService.Application.Addresses.DTOs.AddressDto;

namespace CustomerService.Application.Mappings;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {

        CreateMap<Customer, CustomerDetailsDto>()
            .ConstructUsing((c, context) => new CustomerDetailsDto
                (
                    c.Id,
                    c.UserId,
                    c.FullName.FirstName,
                    c.FullName.LastName,
                    c.PhoneNumber.Value,
                    context.Mapper.Map<List<AddressDto>>(c.Addresses.ToList())));

        CreateMap<Customer, CustomerDto>()
            .ConstructUsing((c, context) => new CustomerDto
                (
                    c.Id,
                    c.UserId,
                    c.FullName.FirstName,
                    c.FullName.LastName,
                    c.PhoneNumber.Value));


        CreateMap<CreateAddressCommand, Address>()
             .ConstructUsing(c => Address.Create(
                Guid.NewGuid(),
                 c.CustomerId,
                 c.AddressLine1,
                 c.AddressLine2,
                 c.City,
                 c.State,
                 c.PostalCode,
                 c.Country,
                c.IsDefault
                 ).Value);

        CreateMap<EditAddressCommand, Address>()
            .ConstructUsing(c => Address.Create(
                    c.AddressId,
                    c.CustomerId,
                    c.AddressLine1,
                    c.AddressLine2,
                    c.City,
                    c.State,
                    c.PostalCode,
                    c.Country,
                    c.IsDefault
            ).Value);


        CreateMap<Address, AddressDto>()
            .ConstructUsing(c => new AddressDto(
                c.CustomerId,
                c.Id,
                c.AddressLine1,
                c.AddressLine2,
                c.City,
                c.State,
                c.PostalCode,
                c.Country,
                c.IsDefault));

        CreateMap<AddressSeedDto, Address>()
                .ConstructUsing(asd => Address.Create
                (
                    asd.Id,
                    asd.CustomerId,
                    asd.AddressLine1,
                    asd.AddressLine2,
                    asd.City,
                    asd.State,
                    asd.PostalCode,
                    asd.Country,
                    asd.IsDefault
                ).Value);

        CreateMap<CustomerSeedDto, Customer>()
            .ConstructUsing((csd, context) => Customer.Create(
                csd.UserId,
                csd.FirstName,
                csd.LastName,
                csd.PhoneNumber,
                context.Mapper.Map<List<Address>>(csd.Addresses.ToList())
            ).Value);
    }
}
