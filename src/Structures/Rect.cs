using System.Diagnostics.CodeAnalysis;
using SFML.Graphics;

namespace ProtoEngine;

public struct Rect : Drawable, IEquatable<Rect>
{
    public Vector2 position;
    public Vector2 size;

    public Vector2 Center => position + size / 2;
    public Vector2 TopLeft => position;
    public Vector2 TopRight => position + new Vector2(size.X, 0);
    public Vector2 BottomLeft => position + new Vector2(0, size.Y);
    public Vector2 BottomRight => position + size;

    public float Left => position.X;
    public float Right => position.X + size.X;
    public float Top => position.Y;
    public float Bottom => position.Y + size.Y;

    public float Width => size.X;
    public float Height => size.Y;

    public Rect(Vector2 size)
    {
        position = new Vector2(0, 0);
        this.size = size;
    }

    public Rect(Vector2 position, Vector2 size)
    {
        this.position = position;
        this.size = size;
    }

    public Rect(Vector2 position, Vector2 size, float scale)
    {
        this.position = position;
        this.size = size * scale;
    }

    public Rect(FloatRect rect)
    {
        position = new Vector2(rect.Left, rect.Top);
        size = new Vector2(rect.Width, rect.Height);
    }

    public Rect()
    {
        position = new Vector2(0, 0);
        size = new Vector2(0, 0);
    }

    public Rect Expand(Vector2 size) => new(position, this.size + size);
    public Rect ExpandAroundCenter(Vector2 size) => new(position - size / 2, this.size + size);
    public Rect ChangeWidth(float width) => new(position, new Vector2(width, size.Y));
    public Rect ChangeHeight(float height) => new(position, new Vector2(size.X, height));
    public Rect ChangePosition(Vector2 position) => new(position, size);
    public Rect ChangeCenter(Vector2 center) => new(center - size / 2, size);

    public Rect Clone()
    {
        return new(position, size);
    }

    public bool Contains(Vector2 point)
    {
        var contains = point.X >= Left && point.X <= Right && point.Y >= Top && point.Y <= Bottom;
        return contains;
    }

    private RectangleShape rect;
    private CircleShape center;

    public void Draw(RenderTarget target, RenderStates states)
    {
        Draw(Color.Red, target);
    }

    public void Draw(Color color, RenderTarget target)
    {
        rect ??= new RectangleShape();
        center ??= new CircleShape();

        rect.Position = position;
        rect.Size = size;
        rect.FillColor = Color.Transparent;
        rect.OutlineColor = color;
        rect.OutlineThickness = 1;

        center.Radius = 4;
        center.Position = Center - new Vector2(center.Radius, center.Radius);
        center.FillColor = color;

        target.Draw(rect);
        target.Draw(center);
    }

    public override bool Equals([NotNullWhen(true)] object? obj) => base.Equals(obj);

    public bool Equals(Rect other)
    {
        return position.Equals(other.position) && size.Equals(other.size);
    }

    public static bool operator ==(Rect left, Rect right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Rect left, Rect right)
    {
        return !(left == right);
    }

    public static implicit operator Rect(RectGPU rect) => new(rect.position, rect.size);

    /// <summary>
    /// moves the position of the rect by the vector
    /// </summary>
    public static Rect operator +(Rect rect, Vector2 vector) => new(rect.position + vector, rect.size);

    /// <summary>
    /// moves the position of the rect by the vector
    /// </summary>
    public static Rect operator -(Rect rect, Vector2 vector) => new(rect.position - vector, rect.size);

    /// <summary>
    /// multiplies the "scale" of the rect
    /// </summary>
    public static Rect operator *(Rect rect, float scalar) => new(rect.position, rect.size * scalar);

    /// <summary>
    /// divides the "scale" of the rect
    /// </summary>
    public static Rect operator /(Rect rect, float scalar) => new(rect.position, rect.size / scalar);
}

public struct RectGPU
{
    public float2 position;
    public float2 size;
    public float2 center;
    public float2 topLeft;
    public float2 topRight;
    public float2 bottomLeft;
    public float2 bottomRight;

    public float left;
    public float right;
    public float top;
    public float bottom;

    public RectGPU(float2 position, float2 size)
    {
        this.position = position;
        this.size = size;
        Set(position, size);
    }

    public RectGPU(float x, float y, float width, float height)
    {
        position = new float2(x, y);
        size = new float2(width, height);
        Set(position, size);
    }

    public void Set(float2 position, float2 size)
    {
        this.position = position;
        this.size = size;

        this.size = size;
        center = position + this.size / 2;
        topLeft = position;
        topRight = position + new float2(this.size.X, 0);
        bottomLeft = position + new float2(0, this.size.Y);
        bottomRight = position + this.size;

        left = position.X;
        right = position.X + this.size.X;
        top = position.Y;
        bottom = position.Y + this.size.Y;
    }

    public static implicit operator RectGPU(Rect rect) => new(rect.position, rect.size);
}