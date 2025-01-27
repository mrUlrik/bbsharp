﻿using System.Text;

namespace bbsharp;

public class BBCodeTextNode : BBCodeNode
{
    private StringBuilder text = new();

    /// <summary>
    ///     Creates a new BBCodeTextNode with the specified InnerText
    /// </summary>
    /// <param name="InnerText">The initial value for InnerText</param>
    public BBCodeTextNode(string InnerText)
    {
        TagName = "span";
        Singular = true;
        text.Append(InnerText);
    }

    public string InnerText
    {
        get => text.ToString();
        set => text = new StringBuilder(value);
    }

    /// <summary>
    ///     Appends a string to InnerText
    /// </summary>
    /// <param name="Text">The text to append</param>
    public void AppendText(string Text)
    {
        text.Append(Text);
    }

    /// <summary>
    ///     Appends a char to InnerText
    /// </summary>
    /// <param name="Text">The char to append</param>
    public void AppendText(char Text)
    {
        text.Append(Text);
    }

    public override string ToString()
    {
        return InnerText;
    }
}