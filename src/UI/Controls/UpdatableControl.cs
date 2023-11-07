using SFML.Graphics;
using SFML.Window;

namespace ProtoEngine.UI;

// This class is so the type with a value can be matched without having to deal with generics
public abstract class ValuedControl : Control
{
    public delegate string GetDisplayValueDelegate();

    protected object? _value;
    protected Text valueText = new();
    protected RectangleShape valueBackground = new();

    public GetDisplayValueDelegate? getDisplayValue;
    public bool drawValue = true;

    public float ValueTextWidth => !drawValue ? Theme.Margin : MathF.Max(Theme.FontSize * 5, valueText.Bounds.Width + Theme.FontSize * 2 + Theme.Margin * 2);
    protected float MaxPanelValueWidth => panel.maxValueWidth;

    public ValuedControl(string label, Panel panel) : base(label, panel) {}
}

public class UpdatableControl<T> : ValuedControl where T : IEquatable<T>
{
    public delegate T SetValue();
    public delegate void OnChanged(T value);

    public SetValue? setValue;
    public OnChanged? onChanged;

    protected T? defaultValue;
    protected T? _typedValue;
    public T? Value
    {
        get => _typedValue;
        set
        {
            if(value != null && !value.Equals(_typedValue)) onChanged?.Invoke(value);
            this._value = value;
            this._typedValue = value;
        }
    }

    public new Rect OuterBounds => innerBounds.Expand(new(Padding.X * 2 + LabelWidth + ValueTextWidth, Padding.Y * 2)) - new Vector2(Padding.X + LabelWidth, Padding.Y);


    public UpdatableControl(string label, Panel panel, SetValue setValue) : base(label, panel)
    {
        this.setValue = setValue;
        this._value = setValue();
        this._typedValue = (T?)_value;
        defaultValue = (T?)_value;
    }

    public UpdatableControl(string label, Panel panel, OnChanged? onChanged, T defaultValue) : base(label, panel)
    {
        this.onChanged = onChanged;
        this.defaultValue = defaultValue;
        this._value = defaultValue;
        this._typedValue = defaultValue;
    }

    protected override void Update()
    {
        if (setValue != null) 
        {
            Value = setValue();
        }

        if (valueBackground.Clicked(Mouse.Button.Left, window))
        {
            Clipboard.Contents = Value?.ToString();
        }
    }

    public override void Draw(float y)
    {
        Update();

        base.Draw(y);

        if(drawValue) 
        {
            innerBounds = innerBounds.Expand(new(-ValueTextWidth, 0));
            
            var text = getDisplayValue?.Invoke() ?? Value?.ToString() ?? "";
            valueText.SetText(text);
            valueText.ApplyStyle(style).Center(OuterBounds, CenterAxis.Y).RightAlign(OuterBounds).Draw(window);

            valueBackground.Position = valueText.Position;
            valueBackground.Size = new Vector2(valueText.Width, valueText.Height);
        }
    }
}
