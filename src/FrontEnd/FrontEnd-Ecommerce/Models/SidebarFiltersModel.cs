namespace FrontEnd_Ecommerce.Models;

public class SidebarFiltersModel
{
    public List<CategoryFilter> Categories { get; set; }
    public int MinPrice { get; set; }
    public int MaxPrice { get; set; }
    public string SelectedSort { get; set; }
}