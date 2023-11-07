using ComputeSharp;

namespace ProtoEngine.Rendering.Internal;

[AutoConstructor]
internal readonly partial struct DrawLinesKernel : IComputeShader
{
    public readonly ReadOnlyBuffer<float2> starts;
    public readonly ReadOnlyBuffer<float2> ends;
    public readonly ReadWriteTexture2D<uint> bitmapDataInt;

    public readonly int2 resolution;
    public readonly RectGPU cameraRect;
    public readonly float cameraScale;
    public readonly uint color;

    public void DrawLineFromID(int id)
    {
        float2 start = (starts[id] - cameraRect.topLeft) / cameraScale;
        float2 end = (ends[id] - cameraRect.topLeft) / cameraScale;

        // clamp into screen
        start = Hlsl.Clamp(start, 0, resolution);
        end = Hlsl.Clamp(end, 0, resolution);
        
        RGBA c = RGBA.FromPackedRGBA(color);
        DrawLine(start, end, c);
    }

    public void Execute()
    {
        DrawLineFromID(ThreadIds.X);
    }

    public void SetColor(int2 coord, RGBA c)
    {
        if (Hlsl.All(coord >= 0) && Hlsl.All(coord < resolution))
        {
            RGBA background = RGBA.FromPackedRGBA(bitmapDataInt[coord]);
            background.A(255);
            bitmapDataInt[coord] = background.Blend(c).ToPackedRGBA();
        }
    }

    public void DrawLine(float2 start, float2 end, RGBA color)
    {
        float2 diff = end - start;
        float length = Hlsl.Length(diff);

        float2 delta = diff / length;

        for (int i = 0; i < length; i++)
        {
            SetColor((int2)(start + delta * i), color);
        }
    }

}