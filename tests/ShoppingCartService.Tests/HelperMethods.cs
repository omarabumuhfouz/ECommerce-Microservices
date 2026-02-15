using ShoppingCartService.Application.Mappings;

namespace ShoppingCartService.Tests;

public static class HelperMethods
{
    public static IMapper CreateMapper()
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CartProfile>();
        });

        return mapperConfig.CreateMapper();
    }


    public static LocalizedString GetCategoryUpdatedSuccessfullyLocalizedMessage()
    => new("CategoryUpdatedSuccessfully", "Category updated successfully.");

}
