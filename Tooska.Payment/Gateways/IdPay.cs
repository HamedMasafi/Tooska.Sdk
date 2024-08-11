using System.Collections.Specialized;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tooska.Payment.Gateways;

// ReSharper disable once UnusedType.Global
public class IdPay<T> : AbstractPaymentGateway<T> where T : AbstractTransaction
{
    public string ApiKey { get; set; }

    struct CreateResult
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Link { get; set; }
    }
    public override PaymentForm CreateForm(ref T t)
    {
        return new PaymentForm
        {
            Method = PaymentForm.MethodType.Get,
            ActionUrl = t.Data1
        };
    }

    public override void InitTransaction(ref T t)
    {
        var data = new NameValueCollection()
        {
            {"order_id", t.Id.ToString()},
            {"amount", t.Amount.ToString()},
            {"name", ""},
            {"phone", ""},
            {"mail", ""},
            {"desc", ""},
            {"callback", CallbackUrl},
        };

        var r = Opener.Open("https://api.idpay.ir/v1.1/payment", data).Result;

        if (r.Code == HttpStatusCode.Created)
        {
            var resultJson = JsonSerializer.Deserialize<CreateResult>(r.Content);
            t.Token = resultJson.Id;
            t.Data1 = resultJson.Link;
            t.Status = TransactionStatus.Inited;
        }
        else
        {
            t.Status = TransactionStatus.Unsuccessful;
            t.ErrorCode = (int) r.Code;
        }
    }

    public override bool VerifyTransaction(ref T t)
    {
        var status = Options.Payment.Global.Getter("status");
        var trackId =  Options.Payment.Global.Getter("track_id");
        var id =  Options.Payment.Global.Getter("id");
        var orderId =  Options.Payment.Global.Getter("order_id");
        var amount =  Options.Payment.Global.Getter("amount");
        var cardNo =  Options.Payment.Global.Getter("card_no");
        var hashedCardNo =  Options.Payment.Global.Getter("hashed_card_no");

        t.CardNumber = hashedCardNo;
        throw new NotImplementedException();
    }

    public override int GetTransactionId()
    {
        throw new NotImplementedException();
    }
}