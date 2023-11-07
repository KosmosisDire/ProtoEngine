using SFML.Graphics;
using SFML.Window;

namespace ProtoEngine.UI;

public class Button : Control
{
    readonly RoundedRectangle background = new();
    readonly RectangleShape clickBox = new();
    public Action? onClick;

    public Button(string label, Panel panel, Action onClick) : base(label, panel)
    {
        drawLabel = false;
        this.onClick = onClick;
    }

    public Button(string label, Panel panel) : base(label, panel)
    {
        drawLabel = false;
    }

    public void SetOnClick(Action onClick)
    {
        this.onClick = onClick;
    }

    protected override void Update()
    {
        if (clickBox.IsBeingDragged(Mouse.Button.Left, window))
        {
            if(!isMouseCaptured)
            {
                isMouseCaptured = true;
            }
        }
        else
        {
            if(isMouseCaptured)
            {
                onClick?.Invoke();
            }

            isMouseCaptured = false;
        }
    }

    public override void Draw(float y)
    {
        Update();

        base.Draw(y);

        clickBox.Size = OuterBounds.size;
        clickBox.Position = OuterBounds.position;

        background.Size = OuterBounds.size;
        background.Position = OuterBounds.position;
        background.FillColor = style.accentColor.Darken(isMouseCaptured ? 0.2f : 0.0f);
        background.OutlineColor = Theme.strokeColor;
        background.OutlineThickness = style.OutlineThickness;
        background.Radius = style.CornerRadius;
        window.Draw(background);

        label.ApplyStyle(style).Center(OuterBounds, CenterAxis.Both).Draw(window);
    }
}
