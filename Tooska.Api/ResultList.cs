namespace Tooska.Api;

public class ResultList<T> : Result<List<T>>
{
    public ResultList(List<T> data) : base(data)
    {
    }

    public ResultList(int code, string message) : base(code, message)
    {
    }
}