using System.Globalization;

namespace Tooska.Core.Extensions;

public static class DateTimeExtensions
{
    public static string ToPersianDateTime(this System.DateTime dateTime)
    {
        try
        {
            var persianCalendar = new PersianCalendar();
            var y = persianCalendar.GetYear(dateTime);
            var m = persianCalendar.GetMonth(dateTime);
            var d = persianCalendar.GetDayOfMonth(dateTime);

            return $"{y}/{m}/{d} {dateTime.Hour}:{dateTime.Minute}:{dateTime.Second}";
        }
        catch
        {
            return "";
        }
    }

    public static string ToPersianDate(this System.DateTime dateTime)
    {
        try
        {
            var persianCalendar = new PersianCalendar();
            var y = persianCalendar.GetYear(dateTime);
            var m = persianCalendar.GetMonth(dateTime);
            var d = persianCalendar.GetDayOfMonth(dateTime);

            return $"{y}/{m}/{d}";
        }
        catch
        {
            return "<INVALID DATE>";
        }
    }
}