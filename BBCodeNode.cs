using System.Text;

namespace bbsharp;

public class BBCodeNode
{
    private readonly List<BBCodeNode> _children;

    /// <summary>
    ///     Creates a new BBCodeNode.
    /// </summary>
    /// <param name="tagName">The node's tag name. Mandatory.</param>
    /// <param name="attribute">The node's optional attribute. This may be an empty string or null.</param>
    /// <param name="isSingular">Singular nodes are self-closing and may not have children</param>
    public BBCodeNode(string tagName, string attribute, bool isSingular)
    {
        if (tagName == null)
            throw new ArgumentNullException(nameof(tagName), "TagName cannot be null");

        tagName = tagName.Trim();
        if (tagName == "")
            throw new ArgumentException("TagName cannot be empty");

        this.TagName = tagName.ToLower();
        this.Attribute = attribute;
        Singular = isSingular;
        _children = new List<BBCodeNode>();
    }

    /// <summary>
    ///     Creates a new BBCodeNode.
    /// </summary>
    /// <param name="tagName">The node's tag name. Mandatory.</param>
    /// <param name="attribute">The node's optional attribute. This may be an empty string or null.</param>
    public BBCodeNode(string tagName, string attribute) : this(tagName, attribute, false)
    {
    }

    /// <summary>
    ///     Creates a new BBCodeNode.
    /// </summary>
    /// <param name="tagName">The node's tag name. Mandatory.</param>
    public BBCodeNode(string tagName) : this(tagName, string.Empty)
    {
    }

    protected BBCodeNode()
    {
        _children = new List<BBCodeNode>();
    }

    /// <summary>
    ///     Gets an array of this node's child nodes
    /// </summary>
    public BBCodeNode[] Children => _children.ToArray();

    /// <summary>
    ///     Gets the parent node of this node.
    /// </summary>
    public BBCodeNode? Parent { get; protected set; }

    /// <summary>
    ///     Gets whether this node is singular. Singular nodes are self-closing and can have no children.
    /// </summary>
    public bool Singular { get; protected set; }

    /// <summary>
    ///     Gets the tag name of this node. The tag name is the main part of the tag, and is mandatory.
    /// </summary>
    public string TagName { get; protected set; }

    /// <summary>
    ///     Gets or sets this node's attribute. The Attribute is the part of the tag that comes after the equals sign. It is
    ///     optional, and this property may return either null or an empty string.
    /// </summary>
    public string? Attribute { get; set; }

    /// <summary>
    ///     Gets an array of children BBCodeNodes with the specified TagName
    /// </summary>
    /// <param name="tagName">The TagName of BBCodeNodes to return</param>
    /// <returns>Array of matching BBCodeNodes</returns>
    public BBCodeNode[] this[string tagName]
    {
        get { return _children.Where(x => x.TagName == tagName).ToArray(); }
    }

    /// <summary>
    ///     Gets the nth child BBCodeNode
    /// </summary>
    /// <param name="index">The index of the BBCodeNode to access</param>
    /// <returns>BBCodeNode at the specified index</returns>
    public BBCodeNode this[int index] => _children[index];

    /// <summary>
    ///     Adds a new child node at the end of this node's descendants
    /// </summary>
    /// <param name="node">The existing BBCodeNode to add. This may not already be childed to another node.</param>
    /// <returns>The node passed</returns>
    public virtual BBCodeNode AppendChild(BBCodeNode node)
    {
        if (Singular)
            throw new InvalidOperationException("Cannot add children to a singular node");

        if (node == null)
            throw new ArgumentNullException(nameof(node), "Node may not be null");

        if (node.Parent != null)
            throw new ArgumentException("The BBCodeNode provided is already a child of another node");

        _children.Add(node);
        node.Parent = this;

        return node;
    }

    /// <summary>
    ///     Adds a new child node at the end of this node's descendants
    /// </summary>
    /// <param name="tagName">The node's tag name. Mandatory.</param>
    /// <param name="attribute">The node's optional attribute. This may be an empty string or null.</param>
    /// <returns>The newly created child node</returns>
    public virtual BBCodeNode AppendChild(string tagName, string attribute)
    {
        var node = new BBCodeNode(tagName, attribute)
        {
            Parent = this
        };

        return AppendChild(node);
    }

    /// <summary>
    ///     Adds a new child node at the end of this node's descendants
    /// </summary>
    /// <param name="tagName">The node's tag name. Mandatory.</param>
    /// <returns>The newly created child node</returns>
    public virtual BBCodeNode AppendChild(string tagName)
    {
        return AppendChild(tagName, "");
    }

    /// <summary>
    ///     Creates a recursive copy of the current nodes and its children
    /// </summary>
    /// <returns>A deep clone of the current node</returns>
    public virtual object Clone()
    {
        var node = new BBCodeNode(TagName, Attribute);
        foreach (var child in Children)
            node.AppendChild((BBCodeNode)child.Clone());
        return node;
    }

    /// <summary>
    ///     Inserts a new child node after the reference node passed.
    /// </summary>
    /// <param name="node">The new child node to add. This may not be already childed to another node</param>
    /// <param name="after">The reference node. This must be a child of the current node</param>
    /// <returns>The added node</returns>
    public virtual BBCodeNode InsertAfter(BBCodeNode node, BBCodeNode after)
    {
        if (Singular)
            throw new InvalidOperationException("Cannot add children to a singular node");

        if (node == null)
            throw new ArgumentNullException(nameof(node), "Node may not be null");

        if (after == null)
            throw new ArgumentNullException(nameof(after), "After may not be null");

        if (node.Parent != null)
            throw new ArgumentException("The Node provided is already a child of another node");

        if (after.Parent == null || after.Parent != this)
            throw new ArgumentException("The After node provided is not a child of this node");

        _children.Insert(_children.IndexOf(after) + 1, node);

        return node;
    }

    /// <summary>
    ///     Inserts a new child node before the reference node passed.
    /// </summary>
    /// <param name="node">The new child node to add. This may not be already childed to another node</param>
    /// <param name="before">The reference node. This must be a child of the current node</param>
    /// <returns>The added node</returns>
    public virtual BBCodeNode InsertBefore(BBCodeNode node, BBCodeNode before)
    {
        if (Singular)
            throw new InvalidOperationException("Cannot add children to a singular node");

        if (node == null)
            throw new ArgumentNullException(nameof(node), "Node may not be null");

        if (before == null)
            throw new ArgumentNullException(nameof(before), "After may not be null");

        if (node.Parent != null)
            throw new ArgumentException("The Node provided is already a child of another node");

        if (before.Parent == null || before.Parent != this)
            throw new ArgumentException("The Before node provided is not a child of this node");

        _children.Insert(_children.IndexOf(before), node);

        return node;
    }

    /// <summary>
    ///     Adds a new child node at the beginning of this node's descendants
    /// </summary>
    /// <param name="node">The existing BBCodeNode to add. This may not already be childed to another node.</param>
    /// <returns>The node passed</returns>
    public virtual BBCodeNode PrependChild(BBCodeNode node)
    {
        if (Singular)
            throw new InvalidOperationException("Cannot add children to a singular node");

        if (node == null)
            throw new ArgumentNullException(nameof(node), "Node may not be null");

        if (node.Parent != null)
            throw new ArgumentException("The BBCodeNode provided is already a child of another node");

        _children.Insert(0, node);

        return node;
    }

    /// <summary>
    ///     Removes all child nodes
    /// </summary>
    public virtual void RemoveAll()
    {
        _children.Clear();
    }

    /// <summary>
    ///     Removes a specific child node
    /// </summary>
    /// <param name="node">The child node to remove. This must be a child of the current node.</param>
    /// <returns>The removed node</returns>
    public virtual BBCodeNode RemoveChild(BBCodeNode node)
    {
        if (node == null)
            throw new ArgumentNullException(nameof(node), "Node may not be null");

        if (node.Parent != null)
            throw new ArgumentException("The BBCodeNode provided is not a child of this node");

        _children.Remove(node);

        return node;
    }

    /// <summary>
    ///     Replaces a specific child node with another
    /// </summary>
    /// <param name="old">The node to remove. This must be a child of this node</param>
    /// <param name="new">The replacement node. This may not already be childed to another node</param>
    /// <returns>The removed node</returns>
    public virtual BBCodeNode ReplaceChild(BBCodeNode old, BBCodeNode @new)
    {
        if (old == null)
            throw new ArgumentNullException(nameof(old), "Arguments may not be null");

        if (@new == null)
            throw new ArgumentNullException(nameof(@new), "Arguments may not be null");

        if (old.Parent != this)
            throw new ArgumentException("The Old node provided is not a child of this node");

        if (@new.Parent != null)
            throw new ArgumentException("The New node provided is a child of another node");

        var index = _children.IndexOf(old);
        _children.Remove(old);
        _children.Insert(index, @new);

        return old;
    }

    /// <summary>
    ///     Recursively generates the BBCode representation of the current node and its children
    /// </summary>
    /// <returns>A BBCode string</returns>
    public override string ToString()
    {
        var str = new StringBuilder();
        str.Append("[" + TagName);

        if ((Attribute ?? "").Trim() != "")
            str.Append("=" + Attribute);

        str.Append("]");

        if (Singular)
            return str.ToString();

        foreach (var child in _children)
            str.Append(child);

        str.Append("[/" + TagName + "]");

        return str.ToString();
    }
}