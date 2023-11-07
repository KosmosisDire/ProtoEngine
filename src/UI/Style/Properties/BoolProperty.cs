namespace ProtoEngine.UI;

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

    public BoolProperty TryOverride(BoolProperty prop)
    {
        if (prop.IsUnset) return this;
        return prop;
    }

    public BoolProperty InitWithDefault(Element appliedTo, FetchValue unsetValue)
    {
        this.appliedTo = appliedTo;
        this.UnsetValue = unsetValue;
        return this;
    }
}