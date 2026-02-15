public static class ValidatorExtensions
{
    // This allows you to chain .WithMetadata("key", value)
    public static IRuleBuilderOptions<T, TProperty> WithMetadata<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule, 
        string key, 
        object value)
    {
        return rule.WithState(x => new Dictionary<string, object> 
        { 
            { key, value } 
        });
    }

    // Advanced: Allows adding multiple items .WithMetadata(new { Max = 5, Min = 1 })
    public static IRuleBuilderOptions<T, TProperty> WithMetadata<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule, 
        object values)
    {
        // Converts an anonymous object to a Dictionary
        var dictionary = values.GetType()
            .GetProperties()
            .ToDictionary(prop => prop.Name, prop => prop.GetValue(values));

        return rule.WithState(x => dictionary);
    }
}