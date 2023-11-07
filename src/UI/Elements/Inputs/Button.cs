using ProtoEngine.Rendering;
using SFML.Graphics;
namespace ProtoEngine.UI;

public class Button : Element
{
    public TextElement label;
    public Action<Button>? OnClick;

    public Button(Element parent, string label, Action<Button>? OnClick = null) : base(parent)
    {
        this.OnClick = OnClick;

        events.OnMouseButtonReleased += (SFML.Window.MouseButtonEventArgs e, Window window) =>
        {
            if (e.Button == SFML.Window.Mouse.Button.Left)
            {
                this.OnClick?.Invoke(this);
            }
        };

        DefaultStyle.height = "1.25em";
        DefaultStyle.radius = Theme.GlobalTheme.controlRadius;
        DefaultStyle.outlineColor = new (() => ComputedStyle.fillColor.Value.Lighten(0.8f));
        DefaultStyle.outlineWidth = "1px";
        DefaultStyle.fillColor = Theme.GlobalTheme.controlSurface;

        this.label = new TextElement(this, label);
        this.label.Style.fontSize = "0.7em";
        this.label.Style.alignSelfX = Alignment.Center;
        this.label.Style.alignSelfY = Alignment.Center;

        HoverStyle = new Style
        {
            fillColor = new ColorMod((color) => color.Lighten(0.8f)),
        };
        PressedStyle = new Style
        {
            fillColor = new ColorMod((color) => color.Darken(0.2f)),
        };
    }
}