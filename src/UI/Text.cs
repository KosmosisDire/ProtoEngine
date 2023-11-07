using SFML.Graphics;

namespace ProtoEngine.UI;


public class Text
{
    public SFML.Graphics.Text textObject;
    public string text
    {
        get => textObject.DisplayedString;
        set => textObject.DisplayedString = value;
    }

    public Vector2 Position
    {
        get => textObject.Position;
        set => textObject.Position = value;
    }

    public Rect Bounds => new(textObject.GetGlobalBounds());

    public float Width => Bounds.Width;
    public float Height => Bounds.Height;

    public bool visible;
    public bool enabled;

    public Text(string text)
    {
        textObject = new();
        this.text = text;
        ApplyStyle(Style.defaultStyle);
    }

    public Text()
    {
        textObject = new();
        ApplyStyle(Style.defaultStyle);
    }

    public Text ApplyStyle(Style style)
    {
        textObject.FillColor = style.textColor;
        textObject.CharacterSize = style.FontSize;
        textObject.Font = style.font;
        visible = style.visible;
        enabled = style.enabled;
        return this;
    }

    public Text Center(Rect inside, CenterAxis axis = CenterAxis.Both)
    {
        var bounds = Bounds;
        var topOffset = bounds.Top - textObject.Position.Y;
        var leftOffset = bounds.Left - textObject.Position.X;
        var x = inside.Center.X - bounds.Width / 2 - leftOffset;
        var y = inside.Center.Y - textObject.CharacterSize / 2 - topOffset;
        var position = new Vector2(x, y);

        if (axis == CenterAxis.X)
        {
            position.X = textObject.Position.X;
        }
        else if (axis == CenterAxis.Y)
        {
            position.Y = textObject.Position.Y;
        }

        textObject.Position = position;

        return this;
    }

    public Text LeftAlign(Rect inside)
    {
        var bounds = Bounds;
        var topOffset = bounds.Top - textObject.Position.Y;
        var leftOffset = bounds.Left - textObject.Position.X;
        var x = inside.Left - leftOffset;
        var y = inside.Center.Y - textObject.CharacterSize / 2 - topOffset;
        var position = new Vector2(x, y);

        textObject.Position = position;

        return this;
    }

    public Text RightAlign(Rect inside)
    {
        var bounds = Bounds;
        var topOffset = bounds.Top - textObject.Position.Y;
        var leftOffset = bounds.Left - textObject.Position.X;
        var x = inside.Right - bounds.Width - leftOffset;
        var y = inside.Center.Y - textObject.CharacterSize / 2 - topOffset;
        var position = new Vector2(x, y);

        textObject.Position = position;

        return this;
    }

    public Text TopAlign(Rect inside)
    {
        var bounds = Bounds;
        var topOffset = bounds.Top - textObject.Position.Y;
        var leftOffset = bounds.Left - textObject.Position.X;
        var x = inside.Center.X - bounds.Width / 2 - leftOffset;
        var y = inside.Top - topOffset;
        var position = new Vector2(x, y);

        textObject.Position = position;

        return this;
    }

    public Text BottomAlign(Rect inside)
    {
        var bounds = Bounds;
        var topOffset = bounds.Top - textObject.Position.Y;
        var leftOffset = bounds.Left - textObject.Position.X;
        var x = inside.Center.X - bounds.Width / 2 - leftOffset;
        var y = inside.Bottom - bounds.Height - topOffset;
        var position = new Vector2(x, y);

        textObject.Position = position;

        return this;
    }

    public Text SetText(string text)
    {
        this.text = text;
        return this;
    }

    public Text SetFont(Font font)
    {
        textObject.Font = font;
        return this;
    }

    public Text SetFontSize(uint size)
    {
        textObject.CharacterSize = size;
        return this;
    }

    public Text SetFillColor(Color color)
    {
        textObject.FillColor = color;
        return this;
    }

    public Text SetOutlineColor(Color color)
    {
        textObject.OutlineColor = color;
        return this;
    }

    public Text SetOutlineThickness(float thickness)
    {
        textObject.OutlineThickness = thickness;
        return this;
    }

    public Text SetFontStyle(SFML.Graphics.Text.Styles style)
    {
        textObject.Style = style;
        return this;
    }

    public Text Draw(RenderWindow target)
    {
        if (!visible || !enabled) return this;

        target.Draw(textObject);
        return this;
    }
}