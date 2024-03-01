namespace bbsharp.Renderers.Html;

internal class HtmlRenderException : Exception
{
    public HtmlRenderException(string Message)
        : base(Message)
    {
    }
}