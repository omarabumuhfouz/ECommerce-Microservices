namespace SharedKernel.Common;

public record PagingParams
{
    private const int MaxPageSize = 50;
    private int _pageSize = 10;

    public string SortBy { get; init; } = "Default";
    
    public bool IsAscending { get; init; } = true;

    public int Page { get; init; } = 1;

    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    // Helper to calculate how many records to skip in SQL/EF
    public int Skip => (Page - 1) * PageSize;

    public static PagingParams Default => new PagingParams
    {
        SortBy = "Default",
        IsAscending = true,
        Page = 1,
        PageSize = 10,
    };
}

