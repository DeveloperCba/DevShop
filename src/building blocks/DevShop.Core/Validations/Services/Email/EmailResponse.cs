namespace DevShop.Core.Validations.Services.Email;

public class EmailResponse
{
    public string Id { get; set; }
    public bool Success { get; set; }
    public string Error { get; set; }
    public DateTime Data { get; set; }

    public EmailResponse()
    {
        this.Success = true;
        this.Error = string.Empty;
        this.Id = string.Empty;
        this.Data = DateTime.Now;
    }
}