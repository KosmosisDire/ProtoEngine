
using SFML.Graphics;

namespace ProtoEngine.UI3;

public class ColorProperty : Property<Color>
{
    public ColorProperty(Color color) : base(false)
    {
        Value = color;
    }

    public ColorProperty(FetchValue getValue) : base(false)
    {
        this.GetValue = getValue;
    }

    public ColorProperty() : base(true) {}

    public static implicit operator Color(ColorProperty prop) => prop.Value;
    public static implicit operator ColorProperty(Color prop) => new(prop);

    public ColorProperty TryOverride(ColorProperty overrideMeasure)
    {
        if (overrideMeasure.IsUnset) return this;
        return overrideMeasure;
    }

    public ColorProperty InitWithDefault(Element appliedTo, FetchValue unsetValue)
    {
        this.appliedTo = appliedTo;
        this.UnsetValue = unsetValue;
        return this;
    }

    public void Lerp(Color target, float time)
    {
        var start = Value;
        var end = target;
        var startTime = DateTime.Now;
        var endTime = startTime + TimeSpan.FromSeconds(time);

        GetValue = () =>
        {
            var now = DateTime.Now;
            var percent = (float)((now - startTime) / (endTime - startTime));
            if (percent == 1) GetValue = () => end;
            return start.Lerp(end, percent);
        };
    }
}