using System.Reflection;

namespace PRN232.LMS.API.Models.Common;

public static class FieldSelector
{
    public static object SelectFields<T>(T item, string? fields)
    {
        if (item is null || string.IsNullOrWhiteSpace(fields))
        {
            return item!;
        }

        var requested = fields.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var result = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

        foreach (var property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (requested.Contains(property.Name))
            {
                result[ToCamelCase(property.Name)] = property.GetValue(item);
            }
        }

        return result;
    }

    public static IReadOnlyList<object> SelectCollection<T>(IEnumerable<T> items, string? fields) =>
        items.Select(item => SelectFields(item, fields)).ToList();

    private static string ToCamelCase(string value) =>
        string.IsNullOrWhiteSpace(value) ? value : char.ToLowerInvariant(value[0]) + value[1..];
}
