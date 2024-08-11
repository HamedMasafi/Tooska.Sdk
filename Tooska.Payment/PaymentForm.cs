using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Tooska.Payment
{

    public class PaymentForm
    {
        public enum MethodType
        {
            Get,
            Post
        }

        public PaymentForm()
        {
            Method = MethodType.Post;
            Data = new Dictionary<string, object>();
        }
        public MethodType Method { get; set; }
        public string ActionUrl { get; set; }
        public string ButtonText { get; set; }
        public Dictionary<string, object> Data{ get; set; }

        public void AddKey(string name, string value)
        {
            Data.Add(name, value);
        }
    }

}