namespace ProtoEngine.UI3;


public class NumericProperty : Property<float>
{
    public bool IsTweening {get; protected set;} = false;
    public static NumericProperty Unset => new(true);
    public float pixels => GetPixels();
    protected virtual float GetPixels() => GetValue?.Invoke() ?? 0;

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

    public static implicit operator float(NumericProperty measure) => measure.GetPixels();
    public static implicit operator int(NumericProperty measure) => (int)measure.GetPixels();
    public static implicit operator uint(NumericProperty measure) => (uint)measure.GetPixels();
    public static implicit operator NumericProperty(float pixels) => new Px(pixels);
    public static implicit operator NumericProperty(int pixels) => new Px(pixels);
    public static implicit operator NumericProperty(uint pixels) => new Px(pixels);

    public NumericProperty TryOverride(NumericProperty overrideMeasure)
    {
        if (overrideMeasure.IsUnset) return this;
        return overrideMeasure;
    }

    public NumericProperty InitWithDefault(Element appliedTo, FetchValue unsetValue)
    {
        this.appliedTo = appliedTo;
        this.UnsetValue = unsetValue;
        return this;
    }

    public void Tween(float target, float time, TweenType tweenType = TweenType.Linear)
    {
        var start = pixels;
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
        var start = pixels;
        var end = start + add;
        Tween(end, time, tweenType);
    }
}

public class Px : NumericProperty
{
    public Px(float pixels) : base(false)
    {
        Value = pixels;
    }

    public Px(FetchValue getValue) : base(false)
    {
        this.GetValue = getValue;
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
    }

    protected override float GetPixels()
    {
        switch (combineType)
        {
            case CombineTypes.Add:
                return firstUnit.pixels + secondUnit.pixels;
            case CombineTypes.Multiply:
                return firstUnit.pixels * secondUnit.pixels;
            case CombineTypes.Divide:
                return firstUnit.pixels / secondUnit.pixels;
            case CombineTypes.Subtract:
                return firstUnit.pixels - secondUnit.pixels;
            default:
                return 0;
        }
    }
}

public class Em : NumericProperty
{
    public Em(float characters) : base(false)
    {
        GetValue = () => (appliedTo.Parent?.ComputedStyle.fontSize.pixels ?? appliedTo.ComputedStyle.fontSize.pixels) * characters;
    }

    public Em(FetchValue characters) : base(false)
    {
        GetValue = () => (appliedTo.Parent?.ComputedStyle.fontSize.pixels ?? appliedTo.ComputedStyle.fontSize.pixels) * characters.Invoke();
    }
}