using ProtoEngine.Rendering;
namespace ProtoEngine.UI;

public class Button : Element
{
    public TextElement label;
    public Action<Button>? OnClick;

    public Button(Element parent, string label, Action<Button>? OnClick = null) : base(parent)
    {
        this.OnClick = OnClick;
        Init();
        this.label!.Text = label;
    }

    public Button(Element parent, Action<Button>? OnClick = null) : base(parent)
    {
        this.OnClick = OnClick;
        Init();
    }

    public Button(Action<Button> OnClick) : base()
    {
        this.OnClick = OnClick;
        Init();
    }

    public Button() : base()
    {
        Init();
    }

    void Init()
    {
        events.OnMouseButtonReleased += (SFML.Window.MouseButtonEventArgs e, Window window) =>
        {
            if (e.Button == SFML.Window.Mouse.Button.Left)
            {
                OnClick?.Invoke(this);
            }
        };

        DefaultStyle.height = "1.25em";
        DefaultStyle.radius = Theme.GlobalTheme.controlRadius;
        DefaultStyle.outlineColor = new (() => ComputedStyle.fillColor.Value.Lighten(0.8f));
        DefaultStyle.outlineWidth = "1px";
        DefaultStyle.fillColor = Theme.GlobalTheme.controlSurface;

        label = new TextElement(this, "");
        label.Style.fontSize = "0.7em";
        label.Style.alignSelfX = Alignment.Center;
        label.Style.alignSelfY = Alignment.Center;
        label.Style.positionStyle = PositionStyle.Absolute;

        HoverStyle = new Style
        {
            fillColor = new ColorMod((color) => color.Lighten(0.8f)),
        };
        PressedStyle = new Style
        {
            fillColor = new ColorMod((color) => color.Darken(0.6f)),
        };
    }
}