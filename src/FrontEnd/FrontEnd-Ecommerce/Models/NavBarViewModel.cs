namespace FrontEnd_Ecommerce.Models;

public class NavBarViewModel
{
    public int CartCount { get; set; }
    public bool IsAuthenticated { get; set; }
    public string UserName { get; set; } // optional for testing
    // public IEnumerable<Category> Categories { get; set; }
}
