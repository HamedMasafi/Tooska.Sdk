using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;

namespace Tooska.Payment.Gateways;

//document url

public class Pasargad<T> : AbstractPaymentGateway<T> where T : AbstractTransaction
{
    public string MerchantCode { get; set; }
    public string TerminalCode { get; set; }
    public string PrivateKey { get; set; }
    
    public override PaymentForm CreateForm(ref T t)
    {
        var form = new PaymentForm()
        {
            Method = PaymentForm.MethodType.Post,
            ActionUrl = "https://pep.shaparak.ir/gateway.aspx",
            Data = new Dictionary<string, object>()
        };
        
        var timeStamp = t.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
        var s = $"#{MerchantCode}#{TerminalCode}#{t.Id}#{t.CreateDate}#{t.Amount}#{CallbackUrl}#1003#{timeStamp}#";
        s = Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes(s)));

        
        form.AddKey("merchantCode", MerchantCode);
        form.AddKey("terminalCode", TerminalCode);
        form.AddKey("amount", t.Amount.ToString());
        form.AddKey("redirectAddress", CallbackUrl);
        form.AddKey("invoiceNumber", t.Id.ToString());
        form.AddKey("invoiceDate", t.CreateDate.ToString());
        form.AddKey("action", "1003");
        form.AddKey("sign", s);
        form.AddKey("timeStamp", timeStamp);

        return form;
    }

    public override void InitTransaction(ref T t)
    {       
        var timeStamp = t.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");

        var s = $"#{MerchantCode}#{TerminalCode}#{t.Id}#{t.CreateDate}#{t.Amount}#{CallbackUrl}#1003#{timeStamp}";
        s = Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes(s)));

        var data = new NameValueCollection
        {
            {"merchantCode", MerchantCode},
            {"terminalCode", TerminalCode},
            {"invoiceNumber", t.Id.ToString()},
            {"invoiceDate", t.CreateDate.ToString()},
            {"amount", t.Amount.ToString()},
            {"redirectAddress", CallbackUrl},
            {"action", "1003"},
            {"timeStamp", timeStamp},
            {"merchantCode", MerchantCode}
        };

        var r = Opener.Open("https://pep.shaparak.ir/VerifyPayment.aspx", data).Result;
    }

    public override bool VerifyTransaction(ref T t)
    {
        throw new NotImplementedException();
    }

    public override int GetTransactionId()
    {
        throw new NotImplementedException();
    }
}