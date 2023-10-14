using System.Net;

namespace DevShop.Core.DomainObjects;

public abstract class IntegrationLogBase : Entity
{
    public string Name { get; set; }
    public string Reference { get; set; }
    public string ReferenceType { get; set; }
    public string Request { get; set; }
    public string Response { get; set; }
    public HttpStatusCode? StatusCode { get; set; }
    public DateTime RequestedAt { get; set; }
    public DateTime? RepliedAt { get; set; }
    public double? TimeSpent { get; set; }
    public string Exception { get; set; }
}