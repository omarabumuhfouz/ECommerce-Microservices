using FrontEnd_Ecommerce.DTOs.Auth.Responses;

namespace FrontEnd_Ecommerce.ViewModels;

public class ProfileViewModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; } = new();
    public bool IsActive { get; set; }
    public int TotalOrders { get; set; }
    public DateTime ActiveSince { get; set; }
    public List<AddressViewModel> Addresses { get; set; } = new();
}

public class AddressViewModel
{
    public int Id { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Country { get; set; }
    public bool IsDefault { get; set; }
    public string AddressType { get; set; } // Home, Work, etc.
}