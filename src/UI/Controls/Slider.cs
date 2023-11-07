using SFML.Graphics;
using SFML.Window;

namespace ProtoEngine.UI;

public class Slider : UpdatableControl<float>
{
    public float min;
    public float max;
    public float step;
    protected float tempRealValue; // this value is used during sliding and is not rounded, whereas the Value is rounded to the nearest step

    readonly RectangleShape background = new();
    readonly CircleShape slider = new();

    public Slider(string label, Panel panel, OnChanged? onChanged, float defaultValue, float min, float max, float step) : base(label, panel, onChanged, defaultValue)
    {
        this.min = min;
        this.max = max;
        this.step = step;
        this.tempRealValue = defaultValue;
    }

    protected override void Update()
    {
        base.Update();

        if (background.IsBeingDragged(Mouse.Button.Left, window) || slider.IsBeingDragged(Mouse.Button.Left, window))
        {
            if(!isMouseCaptured)
            {
                tempRealValue = Value;
                isMouseCaptured = true;
            }

            var percentage = (Mouse.GetPosition(window).X - background.Position.X) / background.Size.X; 
            tempRealValue = min + percentage * (max - min);
        }
        else
        {
            isMouseCaptured = false;
        }

        if (slider.Clicked(Mouse.Button.Right, window))
            tempRealValue = defaultValue;
        

        Value = ProtoMath.RoundToMagnitude(Math.Min(Math.Max(tempRealValue, min), max), step);
        var percent = (Value - min) / (max - min);
        slider.Position = new Vector2(background.Position.X + background.Size.X * percent - slider.Radius, background.Position.Y + background.Size.Y / 2 - slider.Radius);
    }

    public override void Draw(float y)
    {
        base.Draw(y);

        background.Size = new(innerBounds.Width, Theme.NobSize / 2f);
        background.Position = new Vector2(innerBounds.Left, innerBounds.Center.Y - background.Size.Y / 2);
        background.FillColor = Theme.barColor;
        background.OutlineColor = Theme.strokeColor;
        background.OutlineThickness = Theme.OutlineThickness;
        window.Draw(background);

        slider.Radius = Theme.NobSize;
        slider.FillColor = Theme.accentColor;
        var percent = (Value - min) / (max - min);
        slider.Position = new Vector2(background.Position.X + background.Size.X * percent - slider.Radius, background.Position.Y + background.Size.Y / 2 - slider.Radius);
        window.Draw(slider);
    }
}
