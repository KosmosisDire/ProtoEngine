using SFML.Graphics;
using SFML.Window;

namespace ProtoEngine.UI;

public abstract class Control
{
    public Text label;
    public float LabelWidth => !drawLabel ? Theme.Margin : MathF.Max(Theme.FontSize * 5, label.Bounds.Width + Theme.FontSize * 2 + Theme.Margin * 2);
    protected float MaxPanelLabelWidth => panel.maxLabelWidth;
    public int zIndex = 0;

    public bool drawLabel = true;
    private Vector2 _padding = new(0,0);
    public Vector2 Padding { get => _padding * Theme.scale; set => _padding = value; }
    private UITheme? themeOverride = null;
    public UITheme Theme
    {
        get => themeOverride ?? panel.theme;
        set => themeOverride = value;
    }
    private float? lineHeightOverride = null;
    public float LineHeight
    {
        get => lineHeightOverride ?? Theme.LineHeight;
        set => lineHeightOverride = value;
    }

    public Rect innerBounds = new();
    public Rect OuterBounds => innerBounds.Expand(new(Padding.X * 2 + LabelWidth, Padding.Y * 2)) - new Vector2(Padding.X + LabelWidth, Padding.Y);
    public Rect OuterBoundsWithMargin => OuterBounds.Expand(new(Theme.Margin * 2)) - new Vector2(Theme.Margin);


    public Panel panel;
    public RenderWindow window => panel.window;

    protected bool isMouseCaptured = false;
    public bool IsMouseCaptured() => isMouseCaptured;

    public Style style;

    protected Control(string label, Panel panel)
    {
        this.label = new Text(label).ApplyStyle(style);
        this.panel = panel;
        panel.AddControl(this);
        style = new();
    }

    protected abstract void Update();

    public void InitBounds(float y)
    {
        innerBounds = panel.DrawingArea.ChangeHeight(LineHeight).Expand(new(-LabelWidth - Padding.X - Theme.Margin * 2, -Padding.Y - Theme.Margin * 2)) + new Vector2(LabelWidth + Padding.X + Theme.Margin, Padding.Y + Theme.Margin + y);
    }

    public virtual void Click(MouseButtonEvent buttonEvent) {}
    public virtual void Release(MouseButtonEvent buttonEvent) {}
    public virtual void MouseMove(Vector2 mousePos) {}
    public virtual void MouseScroll(MouseWheelScrollEvent scrollEvent) {}
    public virtual void KeyPress(KeyEventArgs keyEvent) {}
    public virtual void KeyRelease(KeyEventArgs keyEvent) {}
    public virtual void MouseEnter() {}
    public virtual void MouseLeave() {}

    public virtual void Draw(float y)
    {
        InitBounds(y);

        if(drawLabel)
        {
            label.ApplyStyle(style).Center(OuterBounds, CenterAxis.Y).LeftAlign(OuterBounds).Draw(window);
        }
    }
}
