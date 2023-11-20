using ProtoEngine.UI.Internal;
using SFML.Graphics;
namespace ProtoEngine.UI;

public class Element : Drawable
{
    public static List<Element> animatedElements = new();

    public RoundedRectangle Box {get; protected set;} = new();

    protected Element root;
    private Element? _parent = null;
    public Element Parent
    {
        get => _parent!;
        set
        {
            if (value == null) 
            {
                _parent?.children.Remove(this);
                _parent = null;
                return;
            }
            
            if (_parent == value) return;
            if (value == this) throw new System.Exception("Cannot set parent to self");
            _parent = value;
            root = value?.root ?? this;
            value?.AddChild(this);
        }
    }

    public List<Element> children = new();
    public List<Element> Siblings => Parent?.children ?? new List<Element>();
    public Element? NextSibling
    {
        get
        {
            if (Parent == null) return null;
            var index = Parent.children.IndexOf(this);
            if (index == Parent.children.Count - 1) return null;
            return Parent.children[index + 1];
        }
    }
    public Element? PreviousSibling
    {
        get
        {
            if (Parent == null) return null;
            var index = Parent.children.IndexOf(this);
            if (index == 0) return null;
            return Parent.children[index - 1];
        }
    }
    protected StyleStack styleStack;
    public ComputedStyle ComputedStyle {get; private set;}
    public ref Style Style => ref styleStack.uniqueStyle;
    protected ref Style DefaultStyle => ref styleStack.defaultStyle;

    private Style? _hoverStyle = null;
    public Style? HoverStyle
    {
        get => _hoverStyle;
        set
        {
            if(value == null) 
                return;

            bool firstSet = _hoverStyle == null;
            _hoverStyle = value;

            if (firstSet)
            {
                events.OnMouseEntered += (window) => 
                {
                    AddStyle(_hoverStyle.Value);
                };
                events.OnMouseLeft += (window) => 
                {
                    RemoveStyle(_hoverStyle.Value);
                };
            }
        }
    }

    private Style? _pressedStyle = null;
    public Style? PressedStyle
    {
        get => _pressedStyle;
        set
        {
            if(value == null) 
                return;

            bool firstSet = _pressedStyle == null;
            _pressedStyle = value;

            if (firstSet)
            {
                var firstPress = true;
                events.OnMouseButtonPressed += (args, w) => 
                {
                    AddStyle(_pressedStyle.Value);
                    if(firstPress)
                    {
                        w.globalEvents.MouseButtonReleased += (args, w) => 
                        {
                            RemoveStyle(_pressedStyle.Value);
                        };

                        events.OnMouseLeft += (window) => 
                        {
                            RemoveStyle(_pressedStyle.Value);
                        };

                        firstPress = false;
                    }
                };
            }
        }
    }

    public Rect Bounds {get; private set;}
    public Rect MarginBounds {get; private set;}
    public Rect PaddedBounds {get; private set;}

    public AbsPx Width {get; private set;}
    public AbsPx Height {get; private set;}
    public AbsPx InnerWidth {get; private set;}
    public AbsPx InnerHeight {get; private set;}
    public AbsPx OuterWidth {get; private set;}
    public AbsPx OuterHeight {get; private set;}
    public AbsPx OuterLeft {get; private set;}
    public AbsPx OuterRight {get; private set;}
    public AbsPx OuterTop {get; private set;}
    public AbsPx OuterBottom {get; private set;}
    public AbsPx Left {get; private set;}
    public AbsPx Right {get; private set;}
    public AbsPx Top {get; private set;}
    public AbsPx Bottom {get; private set;}
    public AbsPx InnerLeft {get; private set;}
    public AbsPx InnerRight {get; private set;}
    public AbsPx InnerTop {get; private set;}
    public AbsPx InnerBottom {get; private set;}
    public AbsPx CenterX {get; private set;}
    public AbsPx CenterY {get; private set;}
    public AbsPx CalculatedHeight {get; private set;}
    public AbsPx CalculatedWidth {get; private set;}

    public ElementEvents events;

    public Element(Element parent)
    {
        Init(parent);
    }

    public Element(Element parent, Style baseStyle)
    {
        Init(parent);
        SetBaseStyle(baseStyle);
    }

    public Element(Style baseStyle)
    {
        Init(null);
        SetBaseStyle(baseStyle);
    }

    public Element()
    {
        Init(null);
    }

    public void Init(Element? parent)
    {
        styleStack = new(this);
        Bounds = new Rect();
        MarginBounds = new Rect();
        PaddedBounds = new Rect();

        Width = new(() => Bounds.Width);
        Height = new(() => Bounds.Height);
        InnerWidth = new(() => PaddedBounds.Width);
        InnerHeight = new(() => PaddedBounds.Height);
        OuterWidth = new(() => MarginBounds.Width);
        OuterHeight = new(() => MarginBounds.Height);
        OuterLeft = new(() => MarginBounds.Left);
        OuterRight = new(() => MarginBounds.Right);
        OuterTop = new(() => MarginBounds.Top);
        OuterBottom = new(() => MarginBounds.Bottom);
        Left = new(() => Bounds.Left);
        Right = new(() => Bounds.Right);
        Top = new(() => Bounds.Top);
        Bottom = new(() => Bounds.Bottom);
        InnerLeft = new(() => PaddedBounds.Left);
        InnerRight = new(() => PaddedBounds.Right);
        InnerTop = new(() => PaddedBounds.Top);
        InnerBottom = new(() => PaddedBounds.Bottom);
        CenterX = new(() => Bounds.Center.X);
        CenterY = new(() => Bounds.Center.Y);
        CalculatedHeight = new(() => 0);
        CalculatedWidth = new(() => 0);

        BuildBox();

        events.OnMouseWheelScrolled += (args, el) => {
            if (events.OnMouseWheelScrolled.GetInvocationList().Length == 1) 
                Parent?.events.OnMouseWheelScrolled?.Invoke(args, el);
        };

        events.OnMouseButtonPressed += (args, el) => {
            if (events.OnMouseButtonPressed.GetInvocationList().Length == 1) 
                Parent?.events.OnMouseButtonPressed?.Invoke(args, el);
        };

        events.OnMouseButtonReleased += (args, el) => {
            if (events.OnMouseButtonReleased.GetInvocationList().Length == 1) 
                Parent?.events.OnMouseButtonReleased?.Invoke(args, el);
        };

        events.OnMouseEntered += (el) => {
            if (events.OnMouseEntered.GetInvocationList().Length == 1) 
                Parent?.events.OnMouseEntered?.Invoke(el);
        };

        events.OnMouseLeft += (el) => {
            if (events.OnMouseLeft.GetInvocationList().Length == 1) 
                Parent?.events.OnMouseLeft?.Invoke(el);
        };

        events.OnMouseMoved += (args, el) => {
            if (events.OnMouseMoved.GetInvocationList().Length == 1) 
                Parent?.events.OnMouseMoved?.Invoke(args, el);
        };

        events.OnTouchBegan += (args, el) => {
            if (events.OnTouchBegan.GetInvocationList().Length == 1) 
                Parent?.events.OnTouchBegan?.Invoke(args, el);
        };

        events.OnTouchMoved += (args, el) => {
            if (events.OnTouchMoved.GetInvocationList().Length == 1) 
                Parent?.events.OnTouchMoved?.Invoke(args, el);
        };

        events.OnTouchEnded += (args, el) => {
            if (events.OnTouchEnded.GetInvocationList().Length == 1) 
                Parent?.events.OnTouchEnded?.Invoke(args, el);
        };

        if (parent != null) 
        {
            parent.AddChild(this, Alignment.End);
            root = parent.root;
        }
        else 
        {
            root = this;
        }
    }

    public void AddChild(Element child, AlignmentProperty? insertPosition = null)
    {
        if (child.Parent != null && child.Parent!.children.Contains(child)) child.Parent?.children.Remove(child);

        child._parent = this;
        child.root = root;

        if (insertPosition == null) // default add to the end
        {
            children.Add(child);
        }
        else
        {
            switch (insertPosition.Value)
            {
                case Alignment.Start:
                    children.Insert(0, child);
                    break;
                case Alignment.Center:
                    children.Insert(children.Count / 2, child);
                    break;
                case Alignment.End:
                    children.Add(child);
                    break;
            }
        }

        BuildBox();
        child.BuildBox();
    }


    private Rect lastBounds = new();
    public virtual void BuildBox()
    {
        ComputedStyle = styleStack.GetStyle();
        CalcWidth();
        CalcHeight();

        var width = CalculatedWidth.Value;
        var height = CalculatedHeight.Value;
        var flowDir = Parent?.ComputedStyle.flowDirection.Value;

        var previousSibling = PreviousSibling;
        while (previousSibling != null && previousSibling!.ComputedStyle.positionStyle == PositionStyle.Absolute)
        {
            previousSibling = previousSibling.PreviousSibling;
        }

        float top = ComputedStyle.alignSelfY.Value switch
        {
            Alignment.Start => ((flowDir == Direction.Vertical && ComputedStyle.positionStyle != PositionStyle.Absolute) ? previousSibling?.OuterBottom.Value + Parent?.ComputedStyle.gap.Value : Parent?.InnerTop ?? 0) ?? Parent?.InnerTop ?? 0,
            Alignment.Center => Parent?.CenterY.Value - Height.Value / 2 ?? 0,
            Alignment.End => Parent?.InnerBottom.Value - Height.Value ?? 0,
            _ => Parent?.InnerTop ?? 0
        };

        float left = ComputedStyle.alignSelfX.Value switch
        {
            Alignment.Start => ((flowDir == Direction.Horizontal && ComputedStyle.positionStyle != PositionStyle.Absolute) ? previousSibling?.OuterRight.Value + Parent?.ComputedStyle.gap.Value : Parent?.InnerLeft ?? 0) ?? Parent?.InnerLeft ?? 0,
            Alignment.Center => Parent?.CenterX.Value - Width.Value / 2 ?? 0,
            Alignment.End => Parent?.InnerRight.Value - Width.Value ?? 0,
            _ => Parent?.InnerLeft ?? 0
        };

        Vector2 topLeft = new Vector2(left, top)
                        + new Vector2(ComputedStyle.left, ComputedStyle.top);

        MarginBounds = new Rect(topLeft, new(width, height));
        
        Bounds = MarginBounds.Expand(-new Vector2(ComputedStyle.marginLeft.Value + ComputedStyle.marginRight.Value, ComputedStyle.marginTop.Value + ComputedStyle.marginBottom.Value));
        Bounds += new Vector2(ComputedStyle.marginLeft.Value, ComputedStyle.marginTop.Value);

        PaddedBounds = Bounds.Expand(-new Vector2(ComputedStyle.paddingX.Value, ComputedStyle.paddingY.Value) * 2)
                            + new Vector2(ComputedStyle.paddingX.Value, ComputedStyle.paddingY.Value);

        Box.Size = Bounds.size;
        Box.Position = Bounds.position;
        Box.FillColor = ComputedStyle.fillColor;
        Box.OutlineColor = ComputedStyle.outlineColor;
        Box.OutlineThickness = ComputedStyle.outlineWidth;
        Box.Radius = (ComputedStyle.radiusTopLeft, ComputedStyle.radiusTopRight, ComputedStyle.radiusBottomLeft, ComputedStyle.radiusBottomRight);

        if (lastBounds != Bounds)
        {
            lastBounds = Bounds;
            RebuildAllChildren();
        }
    }

    public void RebuildAllChildren()
    {
        foreach (var child in children)
        {
            child.BuildBox();
        }
    }

    public void SetBaseStyle(Style style)
    {
        styleStack.baseStyle = style;
    }

    public void AddStyle(Style style, int? importance = null)
    {
        style.importance = importance ?? style.importance;
        styleStack.AddStyle(style);
        BuildBox();
    }

    public void RemoveStyle(Style style)
    {
        if (!styleStack.Contains(style)) return;
        styleStack.RemoveStyle(style);
        BuildBox();
    }

    public float CalcWidth()
    {
        var width = ComputedStyle.width.Value;
        if (ComputedStyle.width.IsUnsetOrAuto)
        {
            width = (Parent?.CalculatedWidth ?? 0) - Parent?.ComputedStyle.paddingX.Value * 2 ?? 0;

            if ((Parent?.ComputedStyle.flowDirection ?? Direction.Vertical) == Direction.Horizontal)
            {
                var siblings = Siblings;
                var divide = 0f;

                for (var i = 0; i < siblings.Count; i++)
                {
                    var sibling = siblings[i];
                    if (sibling.ComputedStyle.positionStyle != PositionStyle.Absolute)
                    {
                        if (sibling != this)
                        {
                            width -= Parent?.ComputedStyle.gap.Value ?? 0;
                            if(!sibling.ComputedStyle.width.IsUnsetOrAuto)
                            {
                                width -= sibling.CalculatedWidth.Value;
                            }
                        }

                        if (sibling.ComputedStyle.width.IsUnsetOrAuto)
                            divide++;
                    }
                }

                width /= MathF.Max(divide, 1);
            }
            else
            {
                width -= ComputedStyle.left.Value + ComputedStyle.right.Value;
            }
        }

        if (ComputedStyle.marginFit == Fit.Ignore || ComputedStyle.marginFit == Fit.Maximum)
        {
            width += ComputedStyle.marginLeft.Value + ComputedStyle.marginRight.Value;
        }

        var fitWidth = 0.0f;

        if (ComputedStyle.flowDirection == Direction.Horizontal)
        {
            for (var i = 0; i < children.Count; i++)
            {
                var child = children[i];
                if (child.ComputedStyle.positionStyle == PositionStyle.Absolute) continue;
                fitWidth += child.CalculatedWidth.Value;
                if (i != 0) fitWidth += ComputedStyle.gap.Value;
            }
        }
        else
        {
            for (var i = 0; i < children.Count; i++)
            {
                var child = children[i];
                if (child.ComputedStyle.positionStyle == PositionStyle.Absolute) continue;
                fitWidth = Math.Max(fitWidth, child.CalculatedWidth.Value);
            }
        }

        width = ProtoMath.Clamp(width, ComputedStyle.minWidth.Value, ComputedStyle.maxWidth.Value);

        var fit = ComputedStyle.contentFitX.Value;
        width = fit switch
        {
            Fit.Minimum => MathF.Max(width, fitWidth),
            Fit.Maximum => MathF.Min(width, fitWidth),
            Fit.Fit => fitWidth + ComputedStyle.paddingX.Value * 2,
            Fit.Ignore => width,
            _ => throw new NotImplementedException()
        };

        width = MathF.Max(width, ComputedStyle.minWidth.Value);

        CalculatedWidth.Value = width;
        return width;
    }

    public float CalcHeight()
    {
        var height = ComputedStyle.height.Value;
        if (ComputedStyle.height.IsUnsetOrAuto)
        {
            height = (Parent?.CalculatedHeight ?? 0) - Parent?.ComputedStyle.paddingY.Value * 2 ?? 0;

            if ((Parent?.ComputedStyle.flowDirection ?? Direction.Vertical) == Direction.Vertical)
            {
                var siblings = Siblings;
                var divide = 0f;

                for (var i = 0; i < siblings.Count; i++)
                {
                    var sibling = siblings[i];
                    if (sibling.ComputedStyle.positionStyle != PositionStyle.Absolute)
                    {
                        if (sibling != this)
                        {
                            height -= Parent?.ComputedStyle.gap.Value ?? 0;
                            if(!sibling.ComputedStyle.height.IsUnsetOrAuto)
                            {
                                height -= sibling.CalculatedHeight.Value;
                            }
                        }

                        if (sibling.ComputedStyle.height.IsUnsetOrAuto)
                            divide++;
                    }
                }

                height /= MathF.Max(divide, 1);
            }
            else
            {
                height -= ComputedStyle.top.Value + ComputedStyle.bottom.Value;
            }
        }

        if (ComputedStyle.marginFit == Fit.Ignore || ComputedStyle.marginFit == Fit.Maximum)
        {
            height += ComputedStyle.marginTop.Value + ComputedStyle.marginBottom.Value;
        }

        var fitHeight = 0.0f;

        if (ComputedStyle.flowDirection == Direction.Vertical)
        {
            for (var i = 0; i < children.Count; i++)
            {
                var child = children[i];
                if (child.ComputedStyle.positionStyle == PositionStyle.Absolute) continue;
                fitHeight += child.CalculatedHeight.Value;
                if (i != 0) fitHeight += ComputedStyle.gap.Value;
            }
        }
        else
        {
            for (var i = 0; i < children.Count; i++)
            {
                var child = children[i];
                if (child.ComputedStyle.positionStyle == PositionStyle.Absolute) continue;
                fitHeight = Math.Max(fitHeight, child.CalculatedHeight.Value);
            }
        }

        height = ProtoMath.Clamp(height, ComputedStyle.minHeight.Value, ComputedStyle.maxHeight.Value);

        var fit = ComputedStyle.contentFitY.Value;
        height = fit switch
        {
            Fit.Minimum => MathF.Max(height, fitHeight) + ComputedStyle.paddingY.Value * 2,
            Fit.Maximum => MathF.Min(height, fitHeight) + ComputedStyle.paddingY.Value * 2,
            Fit.Fit => fitHeight + ComputedStyle.paddingY.Value * 2,
            Fit.Ignore => height,
            _ => throw new NotImplementedException()
        };

        height = MathF.Max(height, ComputedStyle.minHeight.Value);

        CalculatedHeight.Value = height;

        return height;
    }

    public Element? GetElementAtPosition(Vector2 position)
    {
        var element = this;
        if (Contains(position)) 
        {
            for (var i = children.Count - 1; i >= 0; i--)
            {
                var child = children[i];
                if (child.ComputedStyle.ignorePointerEvents) continue;
                var childElement = child.GetElementAtPosition(position);
                if (childElement != null) 
                {
                    element = childElement;
                    break;
                }
            }
        }
        else
        {
            return null;
        }
        return element;
    }

    public bool Contains(Vector2 position)
    {
        if (Bounds.Contains(position))
        {
            var cornerRadius = Box.radiusArrayClamped;
            if(cornerRadius.Max() > 0)
            {
                var corners = new Vector2[4]{Bounds.TopRight, Bounds.TopLeft, Bounds.BottomLeft, Bounds.BottomRight};
                var cornerCenters = Box.cornerCenters;
                for (var i = 0; i < corners.Length; i++)
                {
                    var corner = corners[i];
                    var cornerCenter = cornerCenters[i] + Bounds.TopLeft;
                    var radius = cornerRadius[i];
                
                    var cornerDist = Vector2.Distance(corner, position);
                    var cornerCenterDist = Vector2.Distance(cornerCenter, position);
                    if (cornerDist < radius && cornerCenterDist > radius) return false;
                }
            }
            return true;
        }
        return false;
    }

    public void Remove()
    {
        #pragma warning disable CS8625
        Parent = null;
    }

    public virtual void Draw(RenderTarget target, RenderStates states)
    {
        if(!ComputedStyle.visible) return;
        target.Draw(Box, states);
        
        foreach (var child in children)
        {
            target.Draw(child, states);
        }

        if (ComputedStyle.debugBox)
        {
            MarginBounds.Draw(Color.Blue, target);
            Box.DebugDraw(Color.Yellow, target);
            PaddedBounds.Draw(Color.Green, target);
        }
    }

    public List<Element> GetChildrenRecursive()
    {
        var list = new List<Element>();
        foreach (var child in children)
        {
            list.Add(child);
            list.AddRange(child.GetChildrenRecursive());
        }
        return list;
    }

}