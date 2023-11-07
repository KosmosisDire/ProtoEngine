
namespace ProtoEngine.UI;

public enum TweenType
{
    Linear,
    EaseIn,
    EaseOut,
    EaseInOut
}

public static class TweenExtensions
{
    public static float Apply(this TweenType type, float percent)
    {
        return type switch
        {
            TweenType.Linear => percent,
            TweenType.EaseIn => EaseIn(percent),
            TweenType.EaseOut => EaseOut(percent),
            TweenType.EaseInOut => EaseInOut(percent),
            _ => percent,
        };
    }

    public static float EaseIn(float percent)
    {
        return (float)Math.Pow(percent, 2);
    }

    public static float EaseOut(float percent)
    {
        return 1 - (float)Math.Pow(1 - percent, 2);
    }

    public static float EaseInOut(float percent)
    {
        return percent < 0.5f ? EaseIn(percent * 2) / 2 : EaseOut(percent * 2 - 1) / 2 + 0.5f;
    }
}