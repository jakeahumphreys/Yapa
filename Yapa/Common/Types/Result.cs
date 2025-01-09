namespace Yapa.Common.Types;

public class Result<T>
{
    public T Content { get;}
    public string ErrorMessage { get;}
    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    private Result(T content, string errorMessage)
    {
        ErrorMessage = errorMessage;
        Content = content;
    }
    
    public static Result<T> Success(T content) => new Result<T>(content, string.Empty);
    public static Result<T> Failure (string errorMessage) => new Result<T>(default, errorMessage);
}