
namespace ProtoEngine.UI3;

public enum PositionStyle
{
    Relative,
    Absolute,
}

public class PositionStyleProperty : Property<PositionStyle>
{
    public PositionStyleProperty(PositionStyle position) : base(false)
    {
        Value = position;
    }

    public PositionStyleProperty(FetchValue getValue) : base(false)
    {
        this.GetValue = getValue;
    }

    public PositionStyleProperty() : base(true) {}

    public static implicit operator PositionStyle(PositionStyleProperty prop) => prop.Value;
    public static implicit operator PositionStyleProperty(PositionStyle prop) => new(prop);

    public PositionStyleProperty TryOverride(PositionStyleProperty overrideMeasure)
    {
        if (overrideMeasure.IsUnset) return this;
        return overrideMeasure;
    }

    public PositionStyleProperty InitWithDefault(Element appliedTo, FetchValue unsetValue)
    {
        this.appliedTo = appliedTo;
        this.UnsetValue = unsetValue;
        return this;
    }
}