namespace Tooska.SMS;

public interface ISmsSender
{
    Task<bool> Send(string number, string message);
}