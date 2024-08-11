namespace Tooska.Options.Payment
{
    public class Global
    {
        public static string CallbackUrl { get; set; } = "";
        public static string DashboardUrl { get; set; }
        public static Func<string, string> Getter { get; set; }
    }

    public class Behpardakht
    {
        public static long terminalId{ get; set; }
        public static string userName{ get; set; }
        public static string userPassword{ get; set; }
    }

    public class Pasargad
    {
        public static string MerchantCode { get; set; }
        public static string TerminalCode { get; set; }
        public static string PrivateKey { get; set; }        
    }
}