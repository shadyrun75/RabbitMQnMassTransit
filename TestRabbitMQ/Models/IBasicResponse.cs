namespace Models
{
    public interface IBasicResponse
    {
        string Message { get; set; }
        bool Success { get; set; }
    }
}