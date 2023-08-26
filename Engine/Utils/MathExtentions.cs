namespace ProtoEngine;

public class ProtoMath
{
    public static int RoundToMagnitude(float value, int magnitude)
    {
        return (int)(MathF.Round(value / magnitude) * magnitude);
    }

    public static int CeilToMagnitude(float value, int magnitude)
    {
        return (int)(MathF.Ceiling(value / magnitude) * magnitude);
    }

    public static int FloorToMagnitude(float value, int magnitude)
    {
        return (int)(MathF.Floor(value / magnitude) * magnitude);
    }

    public static float RoundToMagnitude(float value, float magnitude)
    {
        return (float)(Math.Round((decimal)value / (decimal)magnitude) * (decimal)magnitude);
    }

    public static float CeilToMagnitude(float value, float magnitude)
    {
        return (float)(Math.Ceiling((decimal)value / (decimal)magnitude) * (decimal)magnitude);
    }

    public static float FloorToMagnitude(float value, float magnitude)
    {
        return (float)(Math.Floor((decimal)value / (decimal)magnitude) * (decimal)magnitude);
    }
}