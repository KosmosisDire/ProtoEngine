
using ComputeSharp;

namespace ProtoEngine.Rendering.Internal;

public struct RGBA
{
    public uint4 value;
    
    public uint R(){return value.X;}
    public uint G(){return value.Y;}
    public uint B(){return value.Z;}
    public uint A(){return value.W;}

    public void R(uint h) {value.X = h;}
    public void G(uint s) {value.Y = s;}
    public void B(uint l) {value.Z = l;}
    public void A(uint a) {value.W = a;}

    public static RGBA New(uint r, uint g, uint b, uint a)
    {
        var rgba = new RGBA();
        rgba.value = new uint4(r, g, b, a);
        return rgba;
    }

    public static RGBA New(uint lightness, uint alpha)
    {
        var rgba = new RGBA();
        rgba.value = new uint4(lightness, lightness, lightness, alpha);
        return rgba;
    }

    public static RGBA FromPackedRGBA(uint packedRGBA)
    {
        uint r = (packedRGBA >> 0) & 0xFF;
        uint g = (packedRGBA >> 8) & 0xFF;
        uint b = (packedRGBA >> 16) & 0xFF;
        uint a = (packedRGBA >> 24) & 0xFF;
        RGBA rgba = New(r, g, b, a);
        return rgba;
    }

    public uint ToPackedRGBA()
    {
        uint r = R();
        uint g = G();
        uint b = B();
        uint a = A();
        uint packedRGBA = (r << 0) | (g << 8) | (b << 16) | (a << 24);
        return packedRGBA;
    }

    public RGBA Lerp(RGBA other, float t)
    {
        RGBA result = new RGBA();
        result.value = (uint4)Hlsl.Lerp(value, other.value, t);
        return result;
    }

    public RGBA Blend(RGBA other)
    {
        var result = Lerp(other, other.A() / 255f);
        result.A(255);
        return result;
    }

    public static RGBA operator *(RGBA a, float b)
    {
        RGBA result = new RGBA
        {
            value = (uint4)Hlsl.Mul(a.value, b)
        };
        return result;
    }
    


}
