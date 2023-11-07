
namespace ProtoEngine;

public struct LineSegment
{
    public Vector2 start;
    public Vector2 end;
    public Vector2 Direction => end - start;
    public Vector2 NormalizedDirection => Direction.Normalized;
    public float Length => Direction.Magnitude;
    public float LengthSquared => Direction.SqrMagnitude;
    public float Slope => Direction.Y / Direction.X;
    public float YIntercept => start.Y - Slope * start.X;
    public float XIntercept => -YIntercept / Slope;


    public LineSegment(Vector2 start, Vector2 end)
    {
        this.start = start;
        this.end = end;
    }

    public float SqrDistanceTo(Vector2 point)
    {
        var lengthSqr = LengthSquared;
        if (lengthSqr == 0) return (point - start).SqrMagnitude;

        var t = ((point.X - start.X) * (end.X - start.X) + (point.Y - start.Y) * (end.Y - start.Y)) / lengthSqr;
        t = MathF.Max(0, MathF.Min(1, t));

        return Vector2.SqrDistance(point, new Vector2(start.X + t * (end.X - start.X),
                                                       start.Y + t * (end.Y - start.Y)));
    }

    public float DistanceTo(Vector2 point)
    {
        return MathF.Sqrt(SqrDistanceTo(point));
    }

    public static Vector2? Intersects(LineSegment a, LineSegment b, bool considerCollinearOverlapAsIntersect = false)
    {
        Vector2? intersection;
        Math2D.LineSegmentsIntersect(a.start, a.end, b.start, b.end, out intersection, considerCollinearOverlapAsIntersect);
        return intersection;
    }

    public void Shorten(float amount)
    {
        start += NormalizedDirection * amount;
        end -= NormalizedDirection * amount;
    }

    public void ShortenPercent(float percent)
    {
        Shorten(Length * percent);
    }

}