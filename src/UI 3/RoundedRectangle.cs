using ProtoEngine.Rendering;
using SFML.Graphics;
using SFML.System;

namespace ProtoEngine.UI3;

public class RoundedRectangle : Shape
{
    private Vector2 _size;

    public Vector2 Size 
    {
        get => _size;
        set
        {
            _size = value;
            Update();
        }
    }

    public float Width 
    {
        get => _size.X;
        set
        {
            _size.X = value;
            Update();
        }
    }

    public float Height 
    {
        get => _size.Y;
        set
        {
            _size.Y = value;
            Update();
        }
    }

    public float[] radiusArrayClamped;
    private float[] radiusArray;
    public (float TL, float TR, float BL, float BR) Radius 
    {
        get => (radiusArrayClamped[0], radiusArrayClamped[1], radiusArrayClamped[2], radiusArrayClamped[3]);
        set
        {
            var array = new float[] { value.TR, value.TL, value.BL, value.BR };
            if (array.SequenceEqual(radiusArray)) return;
            radiusArray = array;
            cornerResolution = radiusArray.Select((r) => (uint)MathF.Max(MathF.Min(MathF.Sqrt(r * 2), 16), 4)).Max();
            totalPoints = cornerResolution * 4;
            Update();
        }
    }

    public Vector2[] cornerCenters;

    private uint cornerResolution;
    private uint totalPoints = 0;

    public RoundedRectangle()
    {
        _size = new Vector2(10, 10);
        cornerCenters = Array.Empty<Vector2>();
        radiusArrayClamped = new float[] { 0, 0, 0, 0 };
        radiusArray = new float[] { 0, 0, 0, 0 };
        Radius = (0, 0, 0, 0);
        Update();
    }

    public new void Update()
    {
        radiusArrayClamped = radiusArray.Select((r) => MathF.Min(r, MathF.Min(_size.X, _size.Y) / 2)).ToArray();
        GetCornerCenters();
        base.Update();
    }

    public override Vector2f GetPoint(uint index)
    {
        if (index >= totalPoints)
            return new Vector2f(0, 0);

        int centerIndex = (int)(index / cornerResolution);
        float deltaAngle = 90.0f / (cornerResolution - 1);
        const float pi = 3.141592654f;

        Vector2f center = cornerCenters[centerIndex];
        float radius = radiusArrayClamped[centerIndex];

        return new Vector2f(
            radius * MathF.Cos(deltaAngle * (index - centerIndex) * pi / 180f) + center.X,
            -radius * MathF.Sin(deltaAngle * (index - centerIndex) * pi / 180f) + center.Y
        );
    }

    private Vector2[] GetCornerCenters()
    {
        cornerCenters = new Vector2[] {
            new(_size.X - radiusArrayClamped[0], radiusArrayClamped[0]), // top right
            new(radiusArrayClamped[1], radiusArrayClamped[1]), // top left
            new(radiusArrayClamped[2], _size.Y - radiusArrayClamped[2]), // bottom left
            new(_size.X - radiusArrayClamped[3], _size.Y - radiusArrayClamped[3]) // bottom right
        };
        return cornerCenters;
    }

    public void DebugDraw(Color color, RenderTarget window)
    {
        var rect = new Rect(Position, Size);
        
        if (cornerCenters.Length == 4)
        {
            var corner1 = new CircleShape(MathF.Max(radiusArrayClamped[0], 1.1f));
            var corner2 = new CircleShape(MathF.Max(radiusArrayClamped[1], 1.1f));
            var corner3 = new CircleShape(MathF.Max(radiusArrayClamped[2], 1.1f));
            var corner4 = new CircleShape(MathF.Max(radiusArrayClamped[3], 1.1f));
            corner1.FillColor = corner2.FillColor = corner3.FillColor = corner4.FillColor = Color.Transparent;
            corner1.OutlineColor = corner2.OutlineColor = corner3.OutlineColor = corner4.OutlineColor = color;
            corner1.OutlineThickness = corner2.OutlineThickness = corner3.OutlineThickness = corner4.OutlineThickness = 1;
            corner1.Position = Position + cornerCenters[0] - new Vector2(corner1.Radius);
            corner2.Position = Position + cornerCenters[1] - new Vector2(corner2.Radius);
            corner3.Position = Position + cornerCenters[2] - new Vector2(corner3.Radius);
            corner4.Position = Position + cornerCenters[3] - new Vector2(corner4.Radius);
            window.Draw(corner1);
            window.Draw(corner2);
            window.Draw(corner3);
            window.Draw(corner4);
        }

        // rect.Draw(color, window);
    }

    public override uint GetPointCount() => totalPoints;
}
