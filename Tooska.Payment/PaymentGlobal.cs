using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;

#pragma warning disable 
namespace Tooska.Payment
{
    internal class GatewayAddresses
    {
        public const string SAYAN = "https://sayan.shaparak.ir/ws/payment/merchant.wsdl";
        public const string BEHPARDAKHT = "https://bpm.shaparak.ir/pgwchannel/services/pgw";
        public const string ZARINPAL = "https://www.zarinpal.com/pg/services/WebGate/service";
    }


    public static class Helper
    {

        public static AbstractPaymentGateway<Tran> CreateInstance<Tran>(Tran transaction) where Tran : AbstractTransaction
        {
            if (!transaction.Gateway.HasValue)
                return null;
            return CreateInstance<Tran>(transaction.Gateway.Value);
        }
        public static AbstractPaymentGateway<Tran> CreateInstance<Tran>(TransactionGateway Gateway) where Tran : AbstractTransaction
        {
            switch (Gateway)
            {
                // case TransactionGateway.Behardakht:
                //     return new Behardakht<Tran>();
                // case TransactionGateway.Sayan:
                //     return new Sayan<Tran>();
                // case TransactionGateway.Zarinpal:
                //     return new Zarinpal<Tran>();
                //
                // case TransactionGateway.Pasargad:
                //     return new Pasargad<Tran>();
                //
                // case TransactionGateway.InPlace:
                // case TransactionGateway.CustomTransaction:
                //     return new CustomTransaction<Tran>();
            }
            return null;
            //try
            //{
            //    var type = Type.GetType(typeof(Helper).Namespace + "." + Gateway.ToString() + "`1")
            //        .MakeGenericType(new Type[] { typeof(IShop.Models.Transaction) });

            //    return (PaymentGateway<Tran>)Activator.CreateInstance(type);
            //}
            //catch
            //{
            //    return null;
            //}
        }
    }
}

#pragma warning restore