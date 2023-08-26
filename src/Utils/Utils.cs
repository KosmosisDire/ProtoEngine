
using SFML.Graphics;

namespace ProtoEngine;

public static class Extentions
{
    public static uint ToUInt32(this Color c)
    {
        return RGBAToInt(c.R, c.G, c.B, c.A);
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
    
}

