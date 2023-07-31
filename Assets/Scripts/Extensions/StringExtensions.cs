public static class StringExtensions
{
    public static string Color(this string input, string color)
    {
        return input.AddToFront($"<color={color}>").AddToEnd("</color>");
    }

    public static string Italic(this string input)
    {
        return input.AddToFront("<i>").AddToEnd("</i>");
    }

    public static string Bold(this string input)
    {
        return input.AddToFront("<b>").AddToEnd("</b>");
    }

    private static string AddToFront(this string input, string value)
    {
        return $"{value}{input}";
    }

    private static string AddToEnd(this string input, string value)
    {
        return $"{input}{value}";
    }
}
