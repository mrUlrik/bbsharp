using System.Web;

namespace bbsharp.Renderers.Html;

public static partial class HtmlRenderer
{
    private static string Error(BBCodeNode Node, object LookupTable)
    {
        if (Node.Singular)
            return "["
                   + Node.TagName
                   + ((Node.Attribute ?? "").Trim() != ""
                       ? "=" + HttpUtility.HtmlEncode(Node.Attribute ?? "")
                       : "")
                   + "]";
        return "["
               + Node.TagName
               + ((Node.Attribute ?? "").Trim() != ""
                   ? "=" + HttpUtility.HtmlEncode(Node.Attribute ?? "")
                   : "")
               + "]"
               + Node.Children.ToHtml(false, LookupTable)
               + "[/" + Node.TagName + "]";
    }
}