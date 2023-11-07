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

    public TextElement(Property<string>.FetchValue getText) : base()
    {
        this.text = new StringProperty(getText);
        Init();
    }

    private void Init()
    {
        DefaultStyle.fillColor = Theme.GlobalTheme.textColor;
        DefaultStyle.width = new AbsPx(() => TextBounds.Width);
        DefaultStyle.height = new AbsPx(() => textShape.CharacterSize);
        DefaultStyle.minHeight = new AbsPx(() => textShape.CharacterSize);
    }

    public Text textShape = new();

    public StringProperty text;
    public Rect TextBounds => new(textShape.GetGlobalBounds());

    private void Center(Rect inside, Direction axis)
    {
        var offset = axis == Direction.Horizontal ? inside.Center.X - TextBounds.Center.X : inside.Center.Y - TextBounds.Center.Y;
        textShape.Position += axis == Direction.Horizontal ? new Vector2(offset, 0) : new Vector2(0, offset);
    }

    private void LeftAlign(Rect inside)
    {
        var bounds = TextBounds;
        // var topOffset = bounds.Top - textShape.Position.Y;
        var leftOffset = bounds.Left - textShape.Position.X;
        var x = inside.Left - leftOffset;
        // var y = inside.Center.Y - textShape.CharacterSize / 2 - topOffset;
        var position = new Vector2(x, textShape.Position.Y);

        textShape.Position = position;
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

        textShape.Font = Theme.MainFont;
        textShape.CharacterSize = ComputedStyle.fontSize;
        textShape.FillColor = ComputedStyle.fillColor;
        textShape.OutlineColor = ComputedStyle.outlineColor;
        textShape.OutlineThickness = ComputedStyle.outlineWidth;
        textShape.DisplayedString = text.Value;

        var alignX = ComputedStyle.alignSelfX.Value;
        var alignY = ComputedStyle.alignSelfY.Value;

        switch (alignX)
        {
            case Alignment.Start:
                LeftAlign(PaddedBounds);
                break;
            case Alignment.Center:
                Center(PaddedBounds, Direction.Horizontal);
                break;
            case Alignment.End:
                RightAlign(PaddedBounds);
                break;
            default:
                LeftAlign(PaddedBounds);
                break;
        }

        switch (alignY)
        {
            case Alignment.Start:
                TopAlign(PaddedBounds);
                break;
            case Alignment.Center:
                Center(PaddedBounds, Direction.Vertical);
                break;
            case Alignment.End:
                BottomAlign(PaddedBounds);
                break;
            default:
                TopAlign(PaddedBounds);
                break;
        }
    }

    public override void Draw(RenderTarget target, RenderStates states)
    {
        base.Draw(target, states);
        
        if (ComputedStyle.debugBox) TextBounds.Draw(Color.Red, target);
        target.Draw(textShape, states);
    }
}