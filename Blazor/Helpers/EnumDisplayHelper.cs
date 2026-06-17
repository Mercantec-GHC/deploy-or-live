using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Blazor.Helpers;

public static class EnumDisplayHelper
{
    public static string GetDisplayName<T>(T enumValue) where T : Enum
    {
        var member = typeof(T).GetMember(enumValue.ToString()).FirstOrDefault();
        if (member != null)
        {
            var attr = member.GetCustomAttribute<DisplayAttribute>();
            if (attr != null)
                return attr.Name ?? enumValue.ToString();
        }

        return enumValue.ToString();
    }

    public static IEnumerable<string> GetDisplayNames<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T)).Cast<T>().Select(GetDisplayName);
    }
}
