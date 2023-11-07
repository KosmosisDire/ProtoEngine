
namespace ProtoEngine.UI;

public enum Alignment
{
    Start,
    Center,
    End
}

public class AlignmentProperty : Property<Alignment>
{
    public AlignmentProperty(Alignment alignment) : base(false)
    {
        Value = alignment;
    }

    public AlignmentProperty(FetchValue getValue) : base(false)
    {
        this.GetValue = getValue;
    }

    public AlignmentProperty() : base(true) {}

    public static implicit operator Alignment(AlignmentProperty prop) => prop.Value;
    public static implicit operator AlignmentProperty(Alignment prop) => new(prop);

    public AlignmentProperty TryOverride(AlignmentProperty prop)
    {
        return (AlignmentProperty)base.TryOverride(prop);
    }

    public AlignmentProperty InitWithDefault(Element appliedTo, FetchValue unsetValue)
    {
        this.appliedTo = appliedTo;
        this.UnsetValue = unsetValue;
        return this;
    }
}