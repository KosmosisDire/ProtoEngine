using SFML.Graphics;
using SFML.Window;

namespace ProtoEngine.UI;

public class Vector2Slider : UpdatableControl<Vector2>
{
    public Vector2 min;
    public Vector2 max;
    public float step;
    protected Vector2 tempRealValue; // this value is used during sliding and is not rounded, whereas the Value is rounded to the nearest step

    protected readonly RectangleShape background = new();
    protected readonly RectangleShape xAxis = new();
    protected readonly RectangleShape yAxis = new();

    protected readonly CircleShape xSlider = new();
    protected readonly CircleShape ySlider = new();
    protected readonly CircleShape xySlider = new();

    

    public Vector2Slider(string label, Panel panel, OnChanged? onChanged, Vector2 defaultValue, Vector2 min, Vector2 max, float step) : base(label, panel, onChanged, defaultValue)
    {
        this.min = min;
        this.max = max;
        this.step = step;
        this.tempRealValue = defaultValue;
    }

    protected override void Update()
    {
        base.Update();

        var mod = Keyboard.IsKeyPressed(Keyboard.Key.LShift) ? 1 / 10f : 1;
        var delta = new Vector2(MouseGestures.mouseDelta.X * mod, MouseGestures.mouseDelta.Y * mod);

        if (xySlider.IsBeingDragged(Mouse.Button.Left, window))
        {
            if(!isMouseCaptured)
            {
                tempRealValue = Value;
                isMouseCaptured = true;
            }

            tempRealValue = new
            (
                tempRealValue.X + (delta.X / xAxis.Size.X * (max.X - min.X)),
                tempRealValue.Y + (delta.Y / yAxis.Size.Y * (max.Y - min.Y))
            );
        }
        else if (ySlider.IsBeingDragged(Mouse.Button.Left, window))
        {
            if(!isMouseCaptured)
            {
                tempRealValue = Value;
                isMouseCaptured = true;
            }

            tempRealValue = new(tempRealValue.X, tempRealValue.Y + (delta.Y / yAxis.Size.Y * (max.Y - min.Y)));
        }
        else if (xSlider.IsBeingDragged(Mouse.Button.Left, window))
        {
            if(!isMouseCaptured)
            {
                tempRealValue = Value;
                isMouseCaptured = true;
            }

            tempRealValue = new(tempRealValue.X + (delta.X / xAxis.Size.X * (max.X - min.X)), tempRealValue.Y);
        }
        else
        {
            isMouseCaptured = false;
        }

        if (xSlider.Clicked(Mouse.Button.Right, window))
            tempRealValue = new(defaultValue.X, Value.Y);

        if (ySlider.Clicked(Mouse.Button.Right, window))
            tempRealValue = new(Value.X, defaultValue.Y);

        if (xySlider.Clicked(Mouse.Button.Right, window))
            tempRealValue = defaultValue;

        Value = new
        (
            ProtoMath.RoundToMagnitude(MathF.Min(MathF.Max(tempRealValue.X, min.X), max.X), step), 
            ProtoMath.RoundToMagnitude(MathF.Min(MathF.Max(tempRealValue.Y, min.Y), max.Y), step)
        );

        var xPercent = (Value.X - min.X) / (max.X - min.X);
        var yPercent = (Value.Y - min.Y) / (max.Y - min.Y);
        xSlider.Position = new Vector2(xAxis.Position.X + xAxis.Size.X * xPercent - xSlider.Radius, xAxis.Position.Y + xAxis.Size.Y / 2 - xSlider.Radius);
        ySlider.Position = new Vector2(yAxis.Position.X + yAxis.Size.X / 2 - ySlider.Radius, yAxis.Position.Y + yAxis.Size.Y * yPercent - ySlider.Radius);
        xySlider.Position = new Vector2(xSlider.Position.X, ySlider.Position.Y);
    }

    public override void Draw(float y)
    {
        base.Draw(y);

        innerBounds = innerBounds.ChangeHeight(innerBounds.Width / 2f);

        background.Size = new Vector2(innerBounds.Height);
        background.Position = innerBounds.position + new Vector2(innerBounds.Width / 2 - background.Size.X / 2, innerBounds.Height / 2 - background.Size.Y / 2);
        background.FillColor = Theme.barColor;
        background.OutlineColor = Theme.accentColor;
        background.OutlineThickness = Theme.OutlineThickness;
        window.Draw(background);

        xAxis.Size = new Vector2(background.Size.X, Theme.LineThickness);
        xAxis.Position = new Vector2(background.Position.X, background.Position.Y + background.Size.Y / 2 - xAxis.Size.Y / 2);
        xAxis.FillColor = Theme.xAxisColor;
        window.Draw(xAxis);

        yAxis.Size = new Vector2(Theme.LineThickness, background.Size.Y);
        yAxis.Position = new Vector2(background.Position.X + background.Size.X / 2 - yAxis.Size.X / 2, background.Position.Y);
        yAxis.FillColor = Theme.yAxisColor;
        window.Draw(yAxis);

        var xPercent = (Value.X - min.X) / (max.X - min.X);
        var yPercent = (Value.Y - min.Y) / (max.Y - min.Y);
        xSlider.Position = new Vector2(xAxis.Position.X + xAxis.Size.X * xPercent - xSlider.Radius, xAxis.Position.Y + xAxis.Size.Y / 2 - xSlider.Radius);
        ySlider.Position = new Vector2(yAxis.Position.X + yAxis.Size.X / 2 - ySlider.Radius, yAxis.Position.Y + yAxis.Size.Y * yPercent - ySlider.Radius);
        xySlider.Position = new Vector2(xSlider.Position.X, ySlider.Position.Y);

        xSlider.Radius = Theme.NobSize;
        xSlider.FillColor = Theme.xAxisColor;
        window.Draw(xSlider);

        ySlider.Radius = Theme.NobSize;
        ySlider.FillColor = Theme.yAxisColor;
        window.Draw(ySlider);

        xySlider.Radius = Theme.NobSize;
        xySlider.FillColor = Theme.accentColor;
        window.Draw(xySlider);
    }
}

