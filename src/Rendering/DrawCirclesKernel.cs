using ComputeSharp;
namespace ProtoEngine.Rendering.Internal;

[AutoConstructor]
internal readonly partial struct DrawCirclesKernel : IComputeShader
{
    public readonly ReadWriteBuffer<float2> positions;
    public readonly ReadWriteBuffer<uint> colors;
    public readonly ReadWriteTexture2D<uint> bitmapDataInt;
    public readonly ReadWriteBuffer<int> active;
    public readonly int2 resolution;
    public readonly RectGPU cameraRect;
    public readonly float cameraScale;
    public readonly float radius;
    public readonly bool outlineOnly;
    public readonly RGBA fillColor;
    
    public void SetColor(int2 coord, RGBA c)
    {
        if (Hlsl.All(coord >= 0) && Hlsl.All(coord < resolution))
        {
            RGBA background = RGBA.FromPackedRGBA(bitmapDataInt[coord]);
            if (!Hlsl.Any(background.value)) background = fillColor;
            bitmapDataInt[coord] = background.Blend(c).ToPackedRGBA();
        }
    }


    public void DrawCircleOutline(float2 pos, float radius, RGBA color)
    {
        if (radius <= 1)
        {
            SetColor((int2)pos, color);
            return;
        }

        if (radius <= 2)
        {
            SetColor((int2)pos, color);
            SetColor((int2)pos + new int2(1, 0), color);
            SetColor((int2)pos + new int2(0, 1), color);
            SetColor((int2)pos + new int2(1, 1), color);
            return;
        }

        // draws the outline of a circle using Bresenham's circle algorithm

        int x = (int)pos.X;
        int y = (int)pos.Y;
        int r = (int)radius;
        int d = 3 - 2 * r;

        int x1 = 0;
        int y1 = r;

        while (y1 >= x1)
        {
            SetColor(new int2(x + x1, y + y1), color);
            SetColor(new int2(x + y1, y + x1), color);
            SetColor(new int2(x - x1, y + y1), color);
            SetColor(new int2(x - y1, y + x1), color);
            SetColor(new int2(x + x1, y - y1), color);
            SetColor(new int2(x + y1, y - x1), color);
            SetColor(new int2(x - x1, y - y1), color);
            SetColor(new int2(x - y1, y - x1), color);

            if (d < 0)
                d += 4 * x1 + 6;
            else
            {
                d += 4 * (x1 - y1) + 10;
                y1--;
            }
            x1++;
        }

    }

    private bool HasBit(int packed, int bit)
    {
        return (packed & (1u << bit)) != 0;
    }

    public void DrawCircleFromID(int id)
    {
        if(id >= positions.Length) return;

        bool isActive = HasBit(active[id / 32], id % 32);
        if (!isActive) return;

        float2 position = (positions[id] - cameraRect.topLeft) / cameraScale;

        if (Hlsl.Any(position < 0) || Hlsl.Any(position > resolution))
            return;
        
        float radiusAdjusted = radius / cameraScale;
        float2 upper = position + radiusAdjusted + 1;
        float rsquared = radiusAdjusted * radiusAdjusted;
        int startY = (int)(position.Y - radiusAdjusted);
        RGBA color = RGBA.FromPackedRGBA(colors[id]);

        if(outlineOnly || radiusAdjusted <= 2 || radiusAdjusted > 130)
        {
            DrawCircleOutline(position, radiusAdjusted, color);
            return;
        }

        for (int u = (int)(position.X - radiusAdjusted); u < upper.X; u++)
        {

            for (int v = startY; v < upper.Y; v++)
            {
                int2 coord = new int2(u, v);
                float2 dist = position - coord;
                if (Hlsl.Dot(dist, dist) < rsquared) 
                {
                    float distNorm = Hlsl.Length(dist) / radiusAdjusted;
                    color.A((uint)((255-(Hlsl.Pow(distNorm, 4) * 255)) * (distNorm / 2 + 0.5f)));
                    SetColor(coord, color);
                }
            }
        }
    }

    public void Execute()
    {
        DrawCircleFromID(ThreadIds.X);
    }
}
