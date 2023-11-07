using SFML.Graphics;

namespace ProtoEngine.UI3;

public class Slider : Input<float>
{
    private float _value;
    public float Value
    {
        get => _value;
        set => Set(value);
    }

    public Element sliderFill;

    public float min;
    public float max;
    public float step;

    public float Percentage => (Value - min) / (max - min);

    public Slider(Element parent, float defaultValue, float min, float max, float step = 1, Style? style = null) : base(parent)
    {
        this.min = min;
        this.max = max;
        this.step = step;
        Value = defaultValue;
        Init(style);
    }

    public Slider(float defaultValue, float min, float max, float step = 1, Style? style = null) : base()
    {
        this.min = min;
        this.max = max;
        this.step = step;
        Value = defaultValue;
        Init(style);
    }

    private void Init(Style? style = null)
    {
        if(style.HasValue) SetBaseStyle(style.Value);

        DefaultStyle.height = new Em(1);
        // DefaultStyle.width = new Em(15);
        DefaultStyle.radius = DefaultStyle.height / 2f;
        DefaultStyle.outlineColor = Color.White;
        DefaultStyle.outlineWidth = 2;
        DefaultStyle.paddingX = 4;
        DefaultStyle.paddingY = 4;
        sliderFill = new Element(this);
        sliderFill.Style.radiusBottomLeft = ComputedStyle.radiusBottomLeft;
        sliderFill.Style.radiusBottomRight = ComputedStyle.radiusBottomRight;
        sliderFill.Style.radiusTopLeft = ComputedStyle.radiusTopLeft;
        sliderFill.Style.radiusTopRight = ComputedStyle.radiusTopRight;
        sliderFill.Style.fillColor = Color.White;
        sliderFill.Style.width = new Px(() => Percentage * InnerWidth);
        sliderFill.Style.ignorePointerEvents = true;

        events.OnMouseDrag += (e, w) =>
        {
            SetByPos(e.X);
        };

        events.OnMouseButtonReleased += (e, w) =>
        {
            SetByPos(e.X);
        };
    }

    private void Set(float value)
    {
        value = ProtoMath.RoundToMagnitude(ProtoMath.Clamp(value, min, max), step);
        if (value == _value) return;
        _value = value;
        inputEvents.OnChange?.Invoke(value);
    }

    private void SetByPos(float posX)
    {
        var percentage = (posX - Left.Value) / InnerWidth;
        Value = min + (max - min) * percentage;
    }

}