using SFML.Graphics;
using SFML.Window;

namespace ProtoEngine.UI;

public class Toggle : UpdatableControl<bool>
{
    readonly RectangleShape background = new();
    readonly RectangleShape slider = new();

    public Toggle(string label, Panel panel, OnChanged? onChanged, bool defaultValue) : base(label, panel, onChanged, defaultValue)
    {
        drawValue = false;
    }

    protected override void Update()
    {
        base.Update();

        if (background.Clicked(Mouse.Button.Left, window))
        {
            if(!isMouseCaptured)
            {
                Value = !Value;
                isMouseCaptured = true;
            }
        }
        else
        {
            isMouseCaptured = false;
        }

        if (slider.Clicked(Mouse.Button.Right, window))
            Value = defaultValue;
    }

    public override void Draw(float y)
    {
        base.Draw(y);

        background.Size = new Vector2(Theme.FontSize * 3, Theme.FontSize);
        background.Position = new Vector2(innerBounds.Left, innerBounds.Center.Y - background.Size.Y / 2);
        background.FillColor = Theme.barColor;
        background.OutlineColor = Theme.strokeColor;
        background.OutlineThickness = Theme.OutlineThickness;
        window.Draw(background);

        slider.Size = new Vector2(Theme.FontSize * 1.5f, Theme.FontSize);
        slider.Position = new Vector2(background.Position.X + (Value ? background.Size.X - slider.Size.X : 0), background.Position.Y);
        slider.FillColor = Theme.accentColor;
        slider.OutlineColor = Theme.strokeColor;
        slider.OutlineThickness = Theme.OutlineThickness;
        window.Draw(slider);
    }
}
