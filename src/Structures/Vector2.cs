using SFML.System;
namespace ProtoEngine;

public struct Vector2
{
    private float x;
    private float y;

    public float X
    {
        get => x;
        set => x = value;
    }

    public float Y
    {
        get => y;
        set => y = value;
    }

    public Vector2 XX => new(x, x);
    public Vector2 XY => new(x, y);
    public Vector2 YX => new(y, x);
    public Vector2 YY => new(y, y);

    public Vector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    // ---------- Vector Functions ----------

    public float Magnitude => MathF.Sqrt(x * x + y * y);

    public float SqrMagnitude => x * x + y * y;

    public float Dot(Vector2 other) => x * other.x + y * other.y;

    public float Cross(Vector2 other) => x * other.y - y * other.x;

    public float Distance(Vector2 other) => (this - other).Magnitude;

    public float SqrDistance(Vector2 other) => (this - other).SqrMagnitude;

    public Vector2 Normalized
    { 
        get
        {
            var sqrMagnitude = SqrMagnitude;
            if (sqrMagnitude.IsZero() || sqrMagnitude == 1) return this;

            var magnitude = MathF.Sqrt(sqrMagnitude);

            return new Vector2(x / magnitude, y / magnitude);
        } 
    }

    public Vector2 Normalize()
    {
        var sqrMagnitude = SqrMagnitude;
        if (sqrMagnitude.IsZero() || sqrMagnitude == 1) return this;

        var magnitude = MathF.Sqrt(sqrMagnitude);

        x /= magnitude;
        y /= magnitude;

        return this;
    }

    // ---------- Numeric Functions ----------

    public Vector2 Abs() => new(MathF.Abs(x), MathF.Abs(y));

    public Vector2 Floor() => new(MathF.Floor(x), MathF.Floor(y));

    public Vector2 Ceil() => new(MathF.Ceiling(x), MathF.Ceiling(y));

    public Vector2 Round() => new(MathF.Round(x), MathF.Round(y));

    public Vector2 Clamp(Vector2 min, Vector2 max) => new(Math.Clamp(x, min.x, max.x), Math.Clamp(y, min.y, max.y));

    public Vector2 Clamp(float min, float max) => new(Math.Clamp(x, min, max), Math.Clamp(y, min, max));

    public Vector2 Clamp01() => new(Math.Clamp(x, 0, 1), Math.Clamp(y, 0, 1));

    public Vector2 Lerp(Vector2 other, float t) => this + (other - this) * t;

    public Vector2 LerpUnclamped(Vector2 other, float t) => this + (other - this) * t;

    // ---------- Trig Functions ----------

    public Vector2 Rotate(float degrees)
    {
        var radians = degrees * MathF.PI / 180;
        var sin = MathF.Sin(radians);
        var cos = MathF.Cos(radians);
        return new Vector2(x * cos - y * sin, x * sin + y * cos);
    }

    public Vector2 RotateAroundDeg(Vector2 point, float degrees) => RotateAroundRad(point, degrees * MathF.PI / 180);

    public Vector2 RotateAroundRad(Vector2 point, float radians)
    {
        var sin = MathF.Sin(radians);
        var cos = MathF.Cos(radians);
        var x = this.x - point.x;
        var y = this.y - point.y;
        return new Vector2(x * cos - y * sin + point.x, x * sin + y * cos + point.y);
    }

    public Vector2 Reflect(Vector2 normal) => this - 2 * Dot(normal) * normal.Normalized;

    public Vector2 Reflect(Vector2 normal, float bounciness) => this - (1 + bounciness) * Dot(normal) * normal.Normalized;

    public Vector2 Reflect(Vector2 normal, float bounciness, float minDotNormal = 0)
    {
        var dot = Dot(normal);
        if (dot < minDotNormal) return this;
        return this - (1 + bounciness) * dot * normal.Normalized;
    }

    public Vector2 Project(Vector2 onNormal) => onNormal * (Dot(onNormal) / onNormal.SqrMagnitude);

    public Vector2 ProjectOnPlane(Vector2 planeNormal) => this - Project(planeNormal);

    // ---------- Special Functions ----------

    public Vector2 MoveTowards(Vector2 target, float maxDistanceDelta)
    {
        var vector2 = target - this;
        var magnitude = vector2.Magnitude;
        if (magnitude <= maxDistanceDelta || magnitude.IsZero())
            return target;
        return this + vector2 / magnitude * maxDistanceDelta;
    }

    public Vector2 PerpendicularClockwise => new(-y, x);
    public Vector2 PerpendicularCounterClockwise => new(y, -x);

    // ---------- Operations ----------


    public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.x + b.x, a.y + b.y);

    public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.x - b.x, a.y - b.y);

    public static Vector2 operator *(Vector2 a, Vector2 b) => new(a.x * b.x, a.y * b.y);

    public static Vector2 operator /(Vector2 a, Vector2 b) => new(a.x / b.x, a.y / b.y);

    public static Vector2 operator *(Vector2 a, float b) => new(a.x * b, a.y * b);

    public static Vector2 operator *(float b, Vector2 a) => new(a.x * b, a.y * b);

    public static Vector2 operator /(Vector2 a, float b) => new(a.x / b, a.y / b);

    public static Vector2 operator -(Vector2 a) => new(-a.x, -a.y);


    // ---------- Comparisons ----------

    public static bool operator ==(Vector2 a, Vector2 b) => a.x == b.x && a.y == b.y;

    public static bool operator !=(Vector2 a, Vector2 b) => a.x != b.x || a.y != b.y;

    public override bool Equals(object? obj) => obj == null ? false : (obj is Vector2 other && Equals(other));

    public bool Equals(Vector2 other) => x.Equals(other.x) && y.Equals(other.y);

    public override int GetHashCode() => HashCode.Combine(x, y);

    // ---------- Conversions ----------

    public static implicit operator Vector2((float x, float y) tuple) => new(tuple.x, tuple.y);

    public static implicit operator Vector2(Vector2f v) => new(v.X, v.Y);

    public static implicit operator Vector2f(Vector2 v) => new(v.x, v.y);

    public static implicit operator Vector2(Vector2i v) => new(v.X, v.Y);

    public static implicit operator Vector2i(Vector2 v) => new((int)v.x, (int)v.y);

    public static implicit operator Vector2(Vector2u v) => new(v.X, v.Y);

    public static implicit operator Vector2u(Vector2 v) => new((uint)v.x, (uint)v.y);

    public static implicit operator Vector2(float2 v) => new(v.X, v.Y);

    public static implicit operator float2(Vector2 v) => new(v.x, v.y);

    public static implicit operator Vector2(int2 v) => new(v.X, v.Y);

    public static implicit operator int2(Vector2 v) => new((int)v.x, (int)v.y);

    public static implicit operator Vector2(uint2 v) => new(v.X, v.Y);

    public static implicit operator uint2(Vector2 v) => new((uint)v.x, (uint)v.y);

    // ---------- Static Properties / Functions ----------

    public static Vector2 Zero => new(0, 0);
    public static Vector2 One => new(1, 1);
    public static Vector2 Up => new(0, 1);
    public static Vector2 Down => new(0, -1);
    public static Vector2 Left => new(-1, 0);
    public static Vector2 Right => new(1, 0);

    public static Vector2 Min(Vector2 a, Vector2 b) => new(Math.Min(a.x, b.x), Math.Min(a.y, b.y));

    public static Vector2 Max(Vector2 a, Vector2 b) => new(Math.Max(a.x, b.x), Math.Max(a.y, b.y));

    public static float Distance(Vector2 a, Vector2 b) => (a - b).Magnitude;

    public static float SqrDistance(Vector2 a, Vector2 b) => (a - b).SqrMagnitude;

    public static float Dot(Vector2 a, Vector2 b) => a.x * b.x + a.y * b.y;

    public static float Cross(Vector2 a, Vector2 b) => a.x * b.y - a.y * b.x;

}