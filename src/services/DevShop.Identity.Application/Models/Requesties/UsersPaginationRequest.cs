namespace DevShop.Identity.Application.Models.Requesties;

public class UsersPaginationRequest
{
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 0;
    public string Filter { get; set; } = string.Empty;
}