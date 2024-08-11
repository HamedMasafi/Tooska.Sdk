using System;
using System.ComponentModel.DataAnnotations;

namespace Tooska.Payment;

public enum TransactionStatus
{
    [Display(Name = "ثبت شده")]
    Saved,
    [Display(Name = "ارسال شده به بانک")]
    Inited,
    [Display(Name = "موفقیت آمیز")]
    Successful,
    [Display(Name = "ناموفقیت آمیز")]
    Unsuccessful
}
public enum TransactionGateway
{
    [Display(Name = "پرداخت دستی")]
    CustomTransaction,
    [Display(Name = "بانک قوامین")]
    Sayan,
    [Display(Name = "بانک ملت")]
    Behardakht,

    [Display(Name = "زرین پال")]
    Zarinpal,

    [Display(Name = "پاسارگاه")]
    Pasargad,

    [Obsolete("InPlace has been obsoleted, use CustomTransaction")]
    [Display(Name = "پرداخت در محل")]
    InPlace = 99
}
/*
public class AbstractTransaction<T> : AbstractTransaction where T : IdentityUser
{
    public string UserID { get; set; }
    public T User { get; set; }
}*/