namespace DevShop.WebAPI.Core.Responses;

public class ResultResponse
{
    public ResultResponse()
    {
        Errors = new ErrorMessageResponse();
    }

    public string Title { get; set; }
    public int Status { get; set; }
    public ErrorMessageResponse Errors { get; set; }
}

public class ErrorMessageResponse
{
    public ErrorMessageResponse()
    {
        Messages = new List<string>();
    }

    public List<string> Messages { get; set; }
}