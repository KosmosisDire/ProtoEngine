namespace ProtoEngine;

public struct Rect
{
    public Vector2 position;
    public Vector2 unscaledSize;
    public float scale;

    public Vector2 Size => unscaledSize * scale;
    public Vector2 Center => position + Size / 2;
    public Vector2 TopLeft => position;
    public Vector2 TopRight => position + new Vector2(Size.X, 0);
    public Vector2 BottomLeft => position + new Vector2(0, Size.Y);
    public Vector2 BottomRight => position + Size;

    public float Left => position.X;
    public float Right => position.X + Size.X;
    public float Top => position.Y;
    public float Bottom => position.Y + Size.Y;

    public Rect(Vector2 position, Vector2 size)
    {
        this.position = position;
        this.unscaledSize = size;
        scale = 1;
    }

    public Rect(float x, float y, float width, float height)
    {
        position = new Vector2(x, y);
        unscaledSize = new Vector2(width, height);
        scale = 1;
    }

    public Rect(Vector2 position, Vector2 size, float scale)
    {
        this.position = position;
        this.unscaledSize = size;
        this.scale = scale;
    }
}

public struct RectGPU
{
    public float2 position;
    public float2 unscaledSize;
    public float scale;

    public float2 Size => unscaledSize * scale;
    public float2 Center => position + Size / 2;
    public float2 TopLeft => position;
    public float2 TopRight => position + new float2(Size.X, 0);
    public float2 BottomLeft => position + new float2(0, Size.Y);
    public float2 BottomRight => position + Size;

    public float Left => position.X;
    public float Right => position.X + Size.X;
    public float Top => position.Y;
    public float Bottom => position.Y + Size.Y;

    public RectGPU(float2 position, float2 size)
    {
        this.position = position;
        this.unscaledSize = size;
        scale = 1;
    }

    public RectGPU(float x, float y, float width, float height)
    {
        position = new float2(x, y);
        unscaledSize = new float2(width, height);
        scale = 1;
    }

    public RectGPU(float2 position, float2 size, float scale)
    {
        this.position = position;
        this.unscaledSize = size;
        this.scale = scale;
    }

    public static implicit operator RectGPU(Rect rect) => new(rect.position, rect.unscaledSize, rect.scale);
}