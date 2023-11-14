using SFML.Graphics;

namespace ProtoEngine.UI;

public class TextElement : Element
{
    public TextElement(Element parent, StringProperty text) : base(parent)
    {
        this.text = text;
        Init();
    }

    public TextElement(Element parent, StringProperty text, Style style) : base(parent, style)
    {
        this.text = text;
        Init();
    }

    public TextElement(Element parent, StringProperty text, NumericProperty fontSize, AlignmentProperty alignment) : base(parent)
    {
        this.text = text;
        Style.fontSize = fontSize;

        if (Parent.ComputedStyle.flowDirection == Direction.Horizontal)
        {
            Style.alignSelfY = alignment;
        }
        else
        {
            Style.alignSelfX = alignment;
        }

        Init();
    }

    public TextElement(Element parent, Property<string>.FetchValue getText) : base(parent)
    {
        this.text = new StringProperty(getText);
        Init();
    }

    public TextElement(StringProperty text) : base()
    {
        this.text = text;
        Init();
    }

    public TextElement(Property<string>.FetchValue getText) : base()
    {
        this.text = new StringProperty(getText);
        Init();
    }

    private void Init()
    {
        textShape.Font = Theme.MainFont;
        DefaultStyle.fillColor = Theme.GlobalTheme.textColor;
        DefaultStyle.width = new AbsPx(() => TextBounds.Width);
        DefaultStyle.height = new AbsPx(() => textShape.CharacterSize);
        DefaultStyle.minHeight = new AbsPx(() => textShape.CharacterSize);
    }

    public Text textShape = new();

    protected StringProperty text;
    public string Text
    {
        get => text.Value;
        set => text.Value = value;
    }
    public Rect TextBounds {get; private set;}

    private void Center(Rect inside, Direction axis)
    {
        var offset = axis == Direction.Horizontal ? inside.Center.X - TextBounds.Center.X : inside.Center.Y - TextBounds.Center.Y;
        var offsetVector = axis == Direction.Horizontal ? new Vector2(offset, 0) : new Vector2(0, offset);
        textShape.Position += offsetVector;
        TextBounds += offsetVector;
    }

    private void LeftAlign(Rect inside)
    {
        var bounds = TextBounds;
        var leftOffset = bounds.Left - textShape.Position.X;
        var x = inside.Left - leftOffset;
        var position = new Vector2(x, textShape.Position.Y);

        textShape.Position = position;
        TextBounds = TextBounds.ChangePosition(position);
    }

    private void RightAlign(Rect inside)
    {
        var bounds = TextBounds;
        var topOffset = bounds.Top - textShape.Position.Y;
        var leftOffset = bounds.Left - textShape.Position.X;
        var x = inside.Right - bounds.Width - leftOffset;
        var y = inside.Center.Y - textShape.CharacterSize / 2 - topOffset;
        var position = new Vector2(x, y);

        textShape.Position = position;
        TextBounds = TextBounds.ChangePosition(position);
    }

    private void TopAlign(Rect inside)
    {
        var bounds = TextBounds;
        var topOffset = bounds.Top - textShape.Position.Y;
        var leftOffset = bounds.Left - textShape.Position.X;
        var x = inside.Center.X - bounds.Width / 2 - leftOffset;
        var y = inside.Top - topOffset;
        var position = new Vector2(x, y);

        textShape.Position = position;
        TextBounds = TextBounds.ChangePosition(position);
    }

    private void BottomAlign(Rect inside)
    {
        var bounds = TextBounds;
        var topOffset = bounds.Top - textShape.Position.Y;
        var leftOffset = bounds.Left - textShape.Position.X;
        var x = inside.Center.X - bounds.Width / 2 - leftOffset;
        var y = inside.Bottom - bounds.Height - topOffset;
        var position = new Vector2(x, y);

        textShape.Position = position;
        TextBounds = TextBounds.ChangePosition(position);
    }

    public override void BuildBox()
    {
        base.BuildBox();

        if (text == null) return; 

        // hide box
        Box.FillColor = new Color(0, 0, 0, 0);
        Box.OutlineColor = new Color(0, 0, 0, 0);
        Box.OutlineThickness = 0;
        Box.Position = new Vector2(0, 0);
        Box.Size = new Vector2(0, 0);

        textShape.CharacterSize = ComputedStyle.fontSize;
        textShape.FillColor = ComputedStyle.fillColor;
        textShape.OutlineColor = ComputedStyle.outlineColor;
        textShape.OutlineThickness = ComputedStyle.outlineWidth;
        textShape.DisplayedString = text.Value;

        TextBounds = new(textShape.GetGlobalBounds());

        var alignX = ComputedStyle.alignSelfX.Value;
        var alignY = ComputedStyle.alignSelfY.Value;

        var boundingBox = Parent?.PaddedBounds ?? PaddedBounds;

        switch (alignX)
        {
            case Alignment.Start:
                LeftAlign(boundingBox);
                break;
            case Alignment.Center:
                Center(boundingBox, Direction.Horizontal);
                break;
            case Alignment.End:
                RightAlign(boundingBox);
                break;
            default:
                LeftAlign(boundingBox);
                break;
        }

        switch (alignY)
        {
            case Alignment.Start:
                TopAlign(boundingBox);
                break;
            case Alignment.Center:
                Center(boundingBox, Direction.Vertical);
                break;
            case Alignment.End:
                BottomAlign(boundingBox);
                break;
            default:
                TopAlign(boundingBox);
                break;
        }
    }

    public override void Draw(RenderTarget target, RenderStates states)
    {
        if (!ComputedStyle.visible) return;

        base.Draw(target, states);
        
        if (ComputedStyle.debugBox) TextBounds.Draw(Color.Red, target);
        target.Draw(textShape, states);
    }
}