namespace RaceDay.Web.ViewModels;

public class CategoryViewModel
{
    public int CategoryId { get; set; }
    public int EventId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public decimal EntryFee { get; set; }
    public double DistanceKm { get; set; }
}

public class CreateCategoryViewModel
{
    public int EventId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public decimal EntryFee { get; set; }
    public double DistanceKm { get; set; }
}