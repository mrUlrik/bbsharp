using bbsharp.Renderers.Html;

namespace bbsharp.Easy;

public static class EasyHtmlRenderer
{
    /// <summary>
    ///     Quick and easy method for converting BBCode to HTML. Batteries included.
    /// </summary>
    /// <param name="BBCode">A string of BBCode formatted text</param>
    /// <returns>A string of HTML code</returns>
    public static string BbToHtml(this string BBCode)
    {
        return BBCodeDocument.Load(BBCode, false, new[] { "hr" }).ToHtml(false);
    }
}