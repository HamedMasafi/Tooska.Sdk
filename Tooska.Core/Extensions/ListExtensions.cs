namespace Tooska.Core.Extensions;

public static class ListExtensions
{
    public static IList<(T Value, int Index)> ToIndexedList<T>(this IEnumerable<T> list)
    {
        return list.Select((value, index) => (Value: value, Index: index)).ToList();
    }
}