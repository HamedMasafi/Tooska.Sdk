using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Tooska.Core.Extensions;

// ReSharper disable UnusedMember.Global

public static class NumberExtensions
{
    public static string MultiLineToolTip(params string[] parts)
    {
        return string.Join('\n', parts);
    }

    public static string ToCurrency(this int n)
    {
        return n == 0 ? "0" : $"{n:0,0}";
    }


    public static string ToCurrency(this long n)
    {
        return n == 0 ? "0" : $"{n:0,0}";
    }

    public static string ToCurrency(this ulong n)
    {
        return n == 0 ? "0" : $"{n:0,0}";
    }

    public static string ToCurrency(this double n)
    {
        return n == 0 ? "0" : $"{n:0,0}";
    }

    /// <summary>
    /// تبدیل عدد به متن عددی
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    // ReSharper disable once UnusedMember.Global
    public static string ToDigitString(this long n)
    {
        // ReSharper disable StringLiteralTypo
        var numbers1 = new[] {string.Empty, "یک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه"};
        var numbers2 = new[]
            {"ده", "یازده", "دوازده", "سیزده", "چهارده", "پانزده", "شانزده", "هفده", "هجده", "نوزده"};
        var numbersTen = new[] {"", "", "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود"};
        var numbersHundred = new[]
            {string.Empty, "یکصد", "دویست", "سیصد", "چهارصد", "پانصد", "ششصد", "هفتصد", "هشتصد", "نهصد"};
        var numbersThousand = new[] {string.Empty, " هزار", " میلیون", " میلیارد", " تریلیون"};

        string Translate3Digit(long group3)
        {
            if (group3 == 0) return "";

            var y1 = group3 % 10;
            group3 /= 10;
            var y2 = group3 % 10;
            group3 /= 10;
            var y3 = group3 % 10;

            string ret = "";
            if (y2 == 1)
                ret = numbers2[y1];
            else if (y2 == 0)
                ret = numbers1[y1];
            else if (y1 == 0)
                ret = numbersTen[y2];
            else
                ret = numbersTen[y2] + " و " + numbers1[y1];

            if (y3 != 0)
            {
                if (ret == "")
                    ret = numbersHundred[y3];
                else
                    ret = numbersHundred[y3] + " و " + ret;
            }

            return ret;
        }


        string numberString = "";
        var index = 0;
        do
        {
            var group = n % 1000;
            var buffer = Translate3Digit(group);
            if (buffer != "")
            {
                if (index == 0)
                    numberString = buffer;
                else
                    numberString = buffer + " " + numbersThousand[index] + " و " + numberString;
            }

            if (numberString.EndsWith(" و "))
                numberString = numberString.Remove(numberString.Length - 3, 3);

            n /= 1000;
            index++;
        } while (n > 0);

        return numberString;
    }

    public static string ToSequentialDigitString(this long n)
    {
        if (n == 1)
            return "اول";

        var s = n.ToDigitString();
        if (n % 10 == 3)
            s = s.Remove(s.Length - 1, 1) + "و";

        return s + "م";
    }

    public static string ToFixed(this double d)
    {
        return $"{d:N2}";
    }

    public static string ToFixed(this int d)
    {
        return $"{d:n0}";
    }

    public static string ToPersianDigits(this int n)
    {
        return n.ToString().ToPersianDigits();
    }

    public static string ToPersianDigits(this long n)
    {
        return n.ToString().ToPersianDigits();
    }

    public static string ToPersianCurrency(this int n)
    {
        return n.ToCurrency().ToPersianDigits();
    }
}