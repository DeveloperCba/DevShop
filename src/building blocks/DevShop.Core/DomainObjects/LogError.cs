namespace DevShop.Core.DomainObjects;

public class LogError  
{
    public Guid Id { get; set; }
    public string Method { get; set; }
    public string Path { get; set; }
    public string Erro { get; set; }
    public string ErroCompleto { get; set; }
    public string Query { get; set; }
}