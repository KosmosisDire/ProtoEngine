
using ProtoEngine.Rendering.Internal;
using SFML.Graphics;

namespace ProtoEngine;

public static class Extentions
{
    public static uint ToUInt32(this Color c)
    {
        return RGBAToInt(c.R, c.G, c.B, c.A);
    }

    public static RGBA ToRGBA(this Color c)
    {
        return RGBA.New(c.R, c.G, c.B, c.A);
    }
    
    private const double Epsilon = 1e-10;
    public static bool IsZero(this float d)
    {
        return Math.Abs(d) < Epsilon;
    }

    public static uint RGBAToInt(uint r, uint g, uint b, uint a)
    {
        return ( r << 0 ) | ( g << 8 ) | ( b << 16 ) | ( a << 24 );
    }

    public static uint RGBAToInt(byte r, byte g, byte b, byte a)
    {
        return (uint)( r << 0 ) | (uint)( g << 8 ) | (uint)( b << 16 ) | (uint)( a << 24 );
    }

    public static int SingleToInt32Bits(float value)
    {
        return BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
    }

    public static Color Darken(this Color c, float ratio)
    {
        return new Color((byte)(c.R * (1-ratio)), (byte)(c.G * (1-ratio)), (byte)(c.B * (1-ratio)), c.A);
    }

    public static Color Lighten(this Color c, float ratio)
    {
        return new Color((byte)(c.R + (255 - c.R) * (1-ratio)), (byte)(c.G + (255 - c.G) * (1-ratio)), (byte)(c.B + (255 - c.B) * (1-ratio)), c.A);
    }

    public static Color Invert(this Color c)
    {
        return new Color((byte)(255 - c.R), (byte)(255 - c.G), (byte)(255 - c.B), c.A);
    }

    public static Color Multiply(this Color c, Color c2)
    {
        return new Color((byte)(c.R * c2.R / 255), (byte)(c.G * c2.G / 255), (byte)(c.B * c2.B / 255), (byte)(c.A * c2.A / 255));
    }

    public static Color Add(this Color c, Color c2)
    {
        return new Color((byte)(c.R + c2.R), (byte)(c.G + c2.G), (byte)(c.B + c2.B), (byte)(c.A + c2.A));
    }

    public static Color Subtract(this Color c, Color c2)
    {
        return new Color((byte)(c.R - c2.R), (byte)(c.G - c2.G), (byte)(c.B - c2.B), (byte)(c.A - c2.A));
    }

    public static Color Divide(this Color c, Color c2)
    {
        return new Color((byte)(c.R / c2.R), (byte)(c.G / c2.G), (byte)(c.B / c2.B), (byte)(c.A / c2.A));
    }

    public static Color Lerp(this Color c, Color c2, float t)
    {
        return new Color((byte)(c.R * (1 - t) + c2.R * t), (byte)(c.G * (1 - t) + c2.G * t), (byte)(c.B * (1 - t) + c2.B * t), (byte)(c.A * (1 - t) + c2.A * t));
    }

    public static void Swap<T>(this List<T> list, int index1, int index2)
    {
        var temp = list[index1];
        list[index1] = list[index2];
        list[index2] = temp;
    }

    public static void Swap<T>(this T[] list, int index1, int index2)
    {
        var temp = list[index1];
        list[index1] = list[index2];
        list[index2] = temp;
    }

    public static T GetRandom<T>(this IEnumerable<T> list)
    {
        var random = new Random();
        var index = random.Next(list.Count());
        return list.ElementAt(index);
    }

    public static bool HasDuplicates<T>(this IEnumerable<T> list)
    {
        return list.Distinct().Count() != list.Count();
    }

    public static List<T> GetDuplicates<T>(this IEnumerable<T> list)
    {
        return list.GroupBy(x => x)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();
    }

}

