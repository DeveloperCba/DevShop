namespace DevShop.Core.DomainObjects;

public class LogRequest  
{

    public Guid Id { get; set; }
    public string Device { get; set; }
    public string Host { get; set; }
    public string Method { get; set; }
    public string Path { get; set; }
    public string Url { get; set; }
    public string Header { get; set; }
    public string Body { get; set; }
    public string Query { get; set; }
    public string Ip { get; set; }
    public int StatusCode { get; set; }
    public TimeSpan ExecutionTime { get; set; }
    public string Response { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}