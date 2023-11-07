namespace ProtoEngine.UI3;

public enum Fit
{
    Maximum,
    Fit,
    Minimum,
    Ignore
}

public class FitProperty : Property<Fit>
{
    public FitProperty(Fit contain) : base(false)
    {
        Value = contain;
    }

    public FitProperty(FetchValue getValue) : base(false)
    {
        this.GetValue = getValue;
    }

    public FitProperty() : base(true) {}

    public static implicit operator Fit(FitProperty prop) => prop.Value;
    public static implicit operator FitProperty(Fit prop) => new(prop);

    public FitProperty TryOverride(FitProperty overrideMeasure)
    {
        if (overrideMeasure.IsUnset) return this;
        return overrideMeasure;
    }

    public FitProperty InitWithDefault(Element appliedTo, FetchValue unsetValue)
    {
        this.appliedTo = appliedTo;
        this.UnsetValue = unsetValue;
        return this;
    }
}