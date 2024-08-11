using System.Net;

namespace Tooska.Payment
{
    public abstract class AbstractPaymentGateway<T> where T : AbstractTransaction
    {
        protected string CallbackUrl { get; set; }
        public T Transaction { get; set; }
        public string Message { get; protected set; }


        public AbstractPaymentGateway()
        {
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true;

            CallbackUrl = string.Format(Tooska.Options.Payment.Global.CallbackUrl, GetType().Name);
            /*ServicePointManager.ServerCertificateValidationCallback +=
            delegate (
                Object sender1,
                X509Certificate certificate,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };*/
        }

        public abstract PaymentForm CreateForm(ref T t);

        public abstract void InitTransaction(ref T t);

        public abstract bool VerifyTransaction(ref T t);

        public abstract int GetTransactionId();
        
        

        //public Func<long, T> TransactionSelector { get; set; }
    }

}