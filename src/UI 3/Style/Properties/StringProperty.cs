
namespace ProtoEngine.UI3;

public class StringProperty : Property<string>
{
    public StringProperty(string value) : base(false)
    {
        Value = value;
    }

    public StringProperty(FetchValue getValue) : base(false)
    {
        this.GetValue = getValue;
    }

    public StringProperty() : base(true) {}

    public static implicit operator string(StringProperty prop) => prop.Value;
    public static implicit operator StringProperty(string prop) => new(prop);

    public StringProperty TryOverride(StringProperty overrideMeasure)
    {
        if (overrideMeasure.IsUnset) return this;
        return overrideMeasure;
    }

    public StringProperty GetSelfWithDefault(FetchValue unsetValue)
    {
        this.UnsetValue = unsetValue;
        return this;
    }
}