namespace ProtoEngine.UI3;

public class BoolProperty : Property<bool>
{
    public BoolProperty(bool contain) : base(false)
    {
        Value = contain;
    }

    public BoolProperty(FetchValue getValue) : base(false)
    {
        this.GetValue = getValue;
    }

    public BoolProperty() : base(true) {}


    public static implicit operator bool(BoolProperty prop) => prop.Value;
    public static implicit operator BoolProperty(bool prop) => new(prop);

    public BoolProperty TryOverride(BoolProperty overrideMeasure)
    {
        if (overrideMeasure.IsUnset) return this;
        return overrideMeasure;
    }

    public BoolProperty InitWithDefault(Element appliedTo, FetchValue unsetValue)
    {
        this.appliedTo = appliedTo;
        this.UnsetValue = unsetValue;
        return this;
    }
}