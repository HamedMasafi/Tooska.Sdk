namespace Tooska.Api;

public class Result
{
    public bool Success { get; } = false;

    public int ErrorCode { get; }
    public string Message { get; }

    public Result()
    {
        Success = true;
    }

    public Result(int code, string message)
    {
        ErrorCode = code;
        Message = message;
        Success = false;
    }
}

public class Result<TDataType> : Result
{
    public Result(TDataType data) : base()
    {
        Data = data;
    }

    public Result(int code, string message) : base(code, message)
    {
    }

    public TDataType Data { get; } = default(TDataType);

    //public static implicit operator ActionResult<ApiResult< DataType>>(ApiResult<DataType> d) => new ActionResult<ApiResult<DataType>>(d);
}