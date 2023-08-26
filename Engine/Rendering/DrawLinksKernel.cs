using ComputeSharp;
namespace ProtoEngine.Rendering.Internal;


[AutoConstructor]
internal readonly partial struct DrawLinksKernel : IComputeShader
{
    public readonly ReadOnlyBuffer<int4> links;
    public readonly ReadWriteBuffer<float2> positions;
    public readonly ReadWriteBuffer<uint> bitmapDataInt;

    public readonly int2 resolution;
    public readonly RectGPU cameraRect;
    public readonly uint color;

    public void DrawLineFromID(int id)
    {
        int4 link = links[id];
        if(link.W == 0) return; // inactive link
        int a = link.X;
        int b = link.Y;

        float2 position1 = (positions[a] - cameraRect.TopLeft) / cameraRect.scale;
        float2 position2 = (positions[b] - cameraRect.TopLeft) / cameraRect.scale;
        float2 dist = position2 - position1;

        if (((Hlsl.Any(position1 < 0) || Hlsl.Any(position1 > resolution)) && (Hlsl.Any(position2 < 0) || Hlsl.Any(position2 > resolution))) || Hlsl.Dot(dist, dist) < 50)
            return;

        DrawLine(position1, position2, color);
    }

    public void Execute()
    {
        for (int i = 0; i < 10; i++)
        {
            DrawLineFromID(ThreadIds.X + links.Length / 10 * i);
        }
    }

    public void SetColor(int2 coord, uint c)
    {
        if (Hlsl.All(coord >= 0) && Hlsl.All(coord < resolution))
        {
            bitmapDataInt[coord.X + coord.Y * resolution.X] = c;
        }
    }

    public void DrawLine(float2 start, float2 end, uint color)
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