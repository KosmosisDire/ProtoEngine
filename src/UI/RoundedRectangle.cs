using ProtoEngine.Rendering;
using SFML.Graphics;
using SFML.System;

namespace ProtoEngine.UI.Internal;

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
            var array = new float[] { value.TL, value.TR, value.BR, value.BL };
            if (array[0] == radiusArray[0] && array[1] == radiusArray[1] && array[2] == radiusArray[2] && array[3] == radiusArray[3])
                return;

            radiusArray = array;
            cornerResolution = radiusArray.Select((r) => (int)MathF.Max(MathF.Min(MathF.Sqrt(r * 4), 16), 1)).ToArray();
            cornerResSums = new int[] 
            { 
                0, 
                cornerResolution[0], 
                cornerResolution[0] + cornerResolution[1], 
                cornerResolution[0] + cornerResolution[1] + cornerResolution[2]
            };
            totalPoints = cornerResolution.Sum();
            Update();
        }
    }

    public Vector2[] cornerCenters;

    private int[] cornerResolution;
    private int[] cornerResSums;
    private int totalPoints = 0;

    public int IndexToCorner(uint index)
    {
        if (index >= totalPoints)
            return 0;

        return index >= cornerResSums[3] ? 3 : index >= cornerResSums[2] ? 2 : index >= cornerResSums[1] ? 1 : 0;
    }

    public RoundedRectangle()
    {
        _size = new Vector2(10, 10);
        cornerCenters = Array.Empty<Vector2>();
        radiusArrayClamped = new float[] { -10, -10, -10, -10 };
        radiusArray = new float[] { -10, -10, -10, -10 };
        Radius = (0, 0, 0, 0);
        Update();
    }

    public new void Update()
    {
        radiusArrayClamped = radiusArray.Select((r) => MathF.Min(r, MathF.Min(_size.X, _size.Y) / 2)).ToArray();
        GetCornerCenters();
        base.Update();
    }

    float angle = 180;
    public override Vector2f GetPoint(uint index)
    {
        if (index == 0) angle = 180;

        if (index >= totalPoints)
            return new Vector2f(0, 0);

        int centerIndex = IndexToCorner(index);
        Vector2f center = cornerCenters[centerIndex];
        float radius = radiusArrayClamped[centerIndex];

        if (radius <= 0.01f)
        {
            angle -= 90;
            return center;
        }

        var (Sin, Cos) = MathF.SinCos(angle * MathF.PI / 180f);
        var vector = new Vector2f(
            radius * Cos + center.X,
            -radius * Sin + center.Y
        );
        
        var cornerRes = cornerResolution[centerIndex];
        float deltaAngle = 90.0f / cornerRes;
        angle -= deltaAngle;

        return vector;
    }

    private Vector2[] GetCornerCenters()
    {
        cornerCenters = new Vector2[] {
            new(radiusArrayClamped[0], radiusArrayClamped[0]), // top left
            new(_size.X - radiusArrayClamped[1], radiusArrayClamped[1]), // top right
            new(_size.X - radiusArrayClamped[2], _size.Y - radiusArrayClamped[2]), // bottom right
            new(radiusArrayClamped[3], _size.Y - radiusArrayClamped[3]), // bottom left
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

    public override uint GetPointCount() => (uint)totalPoints;
}
