using System.ComponentModel.DataAnnotations;

namespace Tooska.Core.Extensions;

public static class EnumExtensions
{
    public static string Display(this Enum e)
    {
        if (e == null)
            return "";

        var info = e.GetType().GetMember(e.ToString());
        if (info.Length == 0)
            return "";

        var att = info[0].GetCustomAttributes(typeof(DisplayAttribute), false);
        return att.Length == 0 ? e.ToString() : ((DisplayAttribute) att[0]).Name;
    }
}