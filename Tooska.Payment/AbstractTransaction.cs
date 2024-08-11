using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tooska.Payment;

public class AbstractTransaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public int RelatedRowId { get; set; }

    //public string UserID { get; set; }
    //public IdentityUser User { get; set; }
    public long Amount { get; set; }
    public TransactionGateway? Gateway { get; set; }
    public TransactionStatus Status { get; set; } = TransactionStatus.Saved;
    public DateTime CreateDate { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string UserComment { get; set; } = "";

    public string CardNumber { get; set; } = "";
    
    //response from server
    public string Token { get; set; } = "";
    public int ErrorCode { get; set; }
    public string Comment { get; set; } = "";
        
    //extra data
    internal string Data1 { get; set; } = "";
    internal string Data2 { get; set; } = "";
    internal string Data3 { get; set; } = "";
    internal string Data4 { get; set; } = "";
}

public class AbstractTransaction<TUser, TKey> : AbstractTransaction
{
    public TUser User { get; set; }
    public TKey UserId { get; set; }
}