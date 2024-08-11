namespace Tooska.Core.Extensions;

public static class StringExtensions
{
    public static string ToPersianDigits(this string s)
    {
        return s
            .Replace("1", "۱")
            .Replace("2", "۲")
            .Replace("3", "۳")
            .Replace("4", "۴")
            .Replace("5", "۵")
            .Replace("6", "۶")
            .Replace("7", "۷")
            .Replace("8", "۸")
            .Replace("9", "۹")
            .Replace("0", "۰");
    }
    
    public static DateTime ToDateTime(this string s)
    {
        return DateTime.Parse(s);
    }
    
    public enum DateType
    {
        BeginOfDay,
        EndOfDay
    }
    public static DateTime ToDate(this string s, DateType type = DateType.BeginOfDay)
    {
        var d =  DateTime.Parse(s);
        return type switch
        {
            DateType.BeginOfDay => new DateTime(d.Year, d.Month, d.Day),
            DateType.EndOfDay => new DateTime(d.Year, d.Month, d.Day, 23, 59, 59),
            _ => new DateTime()
        };
    }
}