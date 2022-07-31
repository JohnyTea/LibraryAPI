namespace Library.API.Models;

public class MyErrorResponse
{
    public string Type { get; set; }
    public string Message { get; set; }
    public string StackTrace { get; set; }

    public MyErrorResponse(Exception ex)
    {
        Type = ex.GetType().Name;
        Message = ex.Message;
        #if (DEBUG)
            StackTrace = ex.ToString();
        #endif
    }
}
