namespace ProtoEngine.UI;


public class NumericProperty : Property<float>
{
    public bool IsTweening {get; protected set;} = false;
    public static NumericProperty Unset => new(true);

    public NumericProperty() : base(true) 
    {
        UnsetValue = () => 0;
    }
    public NumericProperty(bool unset) : base(unset) 
    {
        if (unset) UnsetValue = () => 0;
    }
    public NumericProperty(FetchValue unsetValue) : base(true) 
    {
        this.UnsetValue = unsetValue;
    }

    public static Calc operator +(NumericProperty first, NumericProperty second) => new(first, second, Calc.CombineTypes.Add);
    public static Calc operator *(NumericProperty first, NumericProperty second) => new(first, second, Calc.CombineTypes.Multiply);
    public static Calc operator /(NumericProperty first, NumericProperty second) => new(first, second, Calc.CombineTypes.Divide);
    public static Calc operator -(NumericProperty first, NumericProperty second) => new(first, second, Calc.CombineTypes.Subtract);

    public static implicit operator float(NumericProperty measure) => measure.Value;
    public static implicit operator int(NumericProperty measure) => (int)measure.Value;
    public static implicit operator uint(NumericProperty measure) => (uint)measure.Value;
    public static implicit operator NumericProperty(float pixels) => new AbsPx(pixels);
    public static implicit operator NumericProperty(int pixels) => new AbsPx(pixels);
    public static implicit operator NumericProperty(uint pixels) => new AbsPx(pixels);

    public static implicit operator NumericProperty(string parse)
    {
        parse = parse.Trim();
        if (parse == "0") return new AbsPx(0);

        var value = parse[..^2];
        var unit = parse[^2..];
        return unit switch
        {
            "ap" => new AbsPx(float.Parse(value)),
            "px" => new Px(float.Parse(value)),
            "em" => new Em(float.Parse(value)),
            _ => throw new Exception("Invalid unit")
        };
    }

    public virtual NumericProperty TryOverride(NumericProperty prop)
    {
        if (prop is NumberMod mod) return mod.TryOverride(this);
        if (prop.IsUnset) return this;
        return prop;
    }

    public NumericProperty InitWithDefault(Element appliedTo, FetchValue unsetValue)
    {
        this.appliedTo = appliedTo;
        this.UnsetValue = unsetValue;
        return this;
    }

    public void Tween(float target, float time, TweenType tweenType = TweenType.Linear)
    {
        var start = Value;
        var end = target;
        var startTime = DateTime.Now;
        var endTime = startTime + TimeSpan.FromSeconds(time);
        IsTweening = true;

        GetValue = () =>
        {
            var now = DateTime.Now;
            var percent = (float)((now - startTime) / (endTime - startTime));
            if (percent >= 1)
            {
                GetValue = () => end;
                IsTweening = false;
                return end;
            }
            return ProtoMath.Lerp(start, end, tweenType.Apply(percent));
        };
    }

    public void TweenAdd(float add, float time, TweenType tweenType = TweenType.Linear)
    {
        var start = Value;
        var end = start + add;
        Tween(end, time, tweenType);
    }
}

public class AbsPx : NumericProperty
{
    public AbsPx(float pixels) : base(false)
    {
        Value = pixels;
    }

    public AbsPx(FetchValue getValue) : base(false)
    {
        this.GetValue = getValue;
    }
}

public class Px : NumericProperty
{
    public Px(float pixels) : base(false)
    {
        this.GetValue = () => (Application.currentApplication?.window.Scale1920Ratio ?? 1) * pixels;
    }

    public Px(FetchValue getValue) : base(false)
    {
        this.GetValue = () => (Application.currentApplication?.window.Scale1920Ratio ?? 1) * getValue.Invoke();
    }
}

public class Calc : NumericProperty
{
    public enum CombineTypes
    {
        Add,
        Multiply,
        Divide,
        Subtract
    }

    public NumericProperty firstUnit;
    public NumericProperty secondUnit;
    public CombineTypes combineType;

    public Calc(NumericProperty first, NumericProperty second, CombineTypes combineType) : base(false)
    {
        firstUnit = first;
        secondUnit = second;
        this.combineType = combineType;
        GetValue = GetPixels;
    }

    public Calc(NumericProperty first, float second, CombineTypes combineType) : base(false)
    {
        firstUnit = first;
        secondUnit = new(() => second);
        this.combineType = combineType;
        GetValue = GetPixels;
    }

    protected float GetPixels()
    {
        switch (combineType)
        {
            case CombineTypes.Add:
                return firstUnit.Value + secondUnit.Value;
            case CombineTypes.Multiply:
                return firstUnit.Value * secondUnit.Value;
            case CombineTypes.Divide:
                return firstUnit.Value / secondUnit.Value;
            case CombineTypes.Subtract:
                return firstUnit.Value - secondUnit.Value;
            default:
                return 0;
        }
    }
}

public class Em : NumericProperty
{
    public Em(float characters) : base(false)
    {
        GetValue = () => (appliedTo?.Parent?.ComputedStyle.fontSize.Value ?? appliedTo?.ComputedStyle.fontSize.Value ?? 16f) * characters;
    }

    public Em(FetchValue characters) : base(false)
    {
        GetValue = () => (appliedTo?.Parent?.ComputedStyle.fontSize.Value ?? appliedTo?.ComputedStyle.fontSize.Value ?? 16f) * characters.Invoke();
    }
}