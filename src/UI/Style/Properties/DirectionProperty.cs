namespace ProtoEngine.UI;

public enum Direction
{
    Horizontal,
    Vertical
}

public class DirectionProperty : Property<Direction>
{
    public DirectionProperty(Direction direction) : base(false)
    {
        Value = direction;
    }

    public DirectionProperty(FetchValue getValue) : base(false)
    {
        this.GetValue = getValue;
    }

    public DirectionProperty() : base(true) {}

    public static implicit operator Direction(DirectionProperty prop) => prop.Value;
    public static implicit operator DirectionProperty(Direction prop) => new(prop);

    public DirectionProperty TryOverride(DirectionProperty prop)
    {
        if (prop.IsUnset) return this;
        return prop;
    }

    public DirectionProperty InitWithDefault(Element appliedTo, FetchValue unsetValue)
    {
        this.appliedTo = appliedTo;
        this.UnsetValue = unsetValue;
        return this;
    }
}