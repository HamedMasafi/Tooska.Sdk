using Tooska.SMS.Services.MeliPayamak;

namespace Tooska.SMS;

public class MeliPayamakSender : ISmsSender
{
    public string SenderNumber { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public async Task<bool> Send(string number, string message)
    {
        var s = new SendSoapClient(SendSoapClient.EndpointConfiguration.SendSoap);
        var d = new SendSmsRequest()
        {
            to = new string[] {number},
            from = SenderNumber,
            text = message
        };
        var r = await s.SendSimpleSMSAsync(Username, Password, new string[] {number}, SenderNumber, message, false);

        if (r == null)
            return false;

        return true;
    }
    
    public async Task<bool> BulkSend(string[] numbers, string message)
    {
        var s = new SendSoapClient(SendSoapClient.EndpointConfiguration.SendSoap);

        var r = await s.SendSimpleSMSAsync(Username, Password, numbers, SenderNumber, message, false);

        if (r == null)
            return false;

        return true;
    }
}