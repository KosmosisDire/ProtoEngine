using SFML.Graphics;

namespace ProtoEngine.UI;

public interface PropertyTransformation {}

public class ColorMod : ColorProperty, PropertyTransformation
{
    public delegate Color Transformation(Color color);
    public Transformation transformation;

    public ColorMod(Transformation transformation) : base()
    {
        this.transformation = transformation;
    }

    public override ColorProperty TryOverride(ColorProperty prop)
    {
        if (prop is ColorMod) throw new Exception("Cannot override a ColorMod with another ColorMod");
        var c = new ColorProperty(() => transformation.Invoke(prop.Value));
        return c;
    }
}

public class NumberMod : NumericProperty, PropertyTransformation
{
    public delegate float Transformation(float value);
    public Transformation transformation;

    public NumberMod(Transformation transformation) : base(false)
    {
        this.transformation = transformation;
    }

    public override NumericProperty TryOverride(NumericProperty prop)
    {
        if (prop is NumberMod) throw new Exception("Cannot override a NumberMod with another NumberMod");
        GetValue = () => transformation.Invoke(prop.Value);
        return this;
    }
}