// using ProtoEngine;
// using SFML.Graphics;
// using SFML.System;

// public class RoundedRectangle : Shape, Drawable
// {
//     // public new Vector2 Position { get; set; }
//     public Vector2 Size { get; set; }
//     public float Radius { get; set; }
//     // public new Color FillColor { get; set; }
//     // public new Color OutlineColor { get; set; }
//     // public new uint OutlineThickness { get; set; }

//     public RoundedRectangle(Vector2 position, Vector2 size, float radius, Color fillColor, Color outlineColor, uint outlineThickness)
//     {
//         Position = position;
//         Size = size;
//         Radius = radius;
//         FillColor = fillColor;
//         OutlineColor = outlineColor;
//         OutlineThickness = outlineThickness;
//     }

//     public RoundedRectangle()
//     {
//         Position = new Vector2(0, 0);
//         Size = new Vector2(0, 0);
//         Radius = 0;
//         FillColor = Color.White;
//         OutlineColor = Color.White;
//         OutlineThickness = 0;
//     }

//     public void Draw(RenderTarget target, RenderStates states)
//     {
//         var centerFill = new RectangleShape(new Vector2(Size.X - Radius * 2, Size.Y - Radius * 2));
//         var topFill = new RectangleShape(new Vector2(Size.X - Radius * 2, Radius));
//         var bottomFill = new RectangleShape(new Vector2(Size.X - Radius * 2, Radius));
//         var leftFill = new RectangleShape(new Vector2(Radius, Size.Y - Radius * 2));
//         var rightFill = new RectangleShape(new Vector2(Radius, Size.Y - Radius * 2));

//         var topLeft = new CircleShape(Radius);
//         var topRight = new CircleShape(Radius);
//         var bottomLeft = new CircleShape(Radius);
//         var bottomRight = new CircleShape(Radius);

//         centerFill.Position = Position + new Vector2(Radius, Radius);
//         topFill.Position = Position + new Vector2(Radius, 0);
//         bottomFill.Position = Position + new Vector2(Radius, Size.Y - Radius);
//         leftFill.Position = Position + new Vector2(0, Radius);
//         rightFill.Position = Position + new Vector2(Size.X - Radius, Radius);

//         topLeft.Position = Position;
//         topRight.Position = Position + new Vector2(Size.X - Radius * 2, 0);
//         bottomLeft.Position = Position + new Vector2(0, Size.Y - Radius * 2);
//         bottomRight.Position = Position + new Vector2(Size.X - Radius * 2, Size.Y - Radius * 2);

//         centerFill.FillColor = FillColor;
//         topFill.FillColor = FillColor;
//         bottomFill.FillColor = FillColor;
//         leftFill.FillColor = FillColor;
//         rightFill.FillColor = FillColor;

//         topLeft.FillColor = FillColor;
//         topRight.FillColor = FillColor;
//         bottomLeft.FillColor = FillColor;
//         bottomRight.FillColor = FillColor;
        
//         target.Draw(centerFill, states);
//         target.Draw(topFill, states);
//         target.Draw(bottomFill, states);
//         target.Draw(leftFill, states);
//         target.Draw(rightFill, states);

//         target.Draw(topLeft, states);
//         target.Draw(topRight, states);
//         target.Draw(bottomLeft, states);
//         target.Draw(bottomRight, states);

//     }

//     public override uint GetPointCount() => 4;
//     public override Vector2f GetPoint(uint index)
//     {
//         Vector2[] points = new Vector2[4];
//         points[0] = new Vector2(Position.X, Position.Y);
//         points[1] = new Vector2(Position.X + Size.X, Position.Y);
//         points[2] = new Vector2(Position.X + Size.X, Position.Y + Size.Y);
//         points[3] = new Vector2(Position.X, Position.Y + Size.Y);
        
//         return points[index];
//     }

//     public new FloatRect GetGlobalBounds()
//     {
//         return new FloatRect(Position.X, Position.Y, Size.X, Size.Y);
//     }
// }

using System;
using System.Reflection.Metadata.Ecma335;
using SFML.Graphics;
using SFML.System;


public class RoundedRectangle : Shape
{
    private Vector2f _size;

    public Vector2f Size 
    {
        get => _size;
        set
        {
            _size = value;
            Update();
        }
    }

    private float _radius;
    public float Radius 
    {
        get => _radius;
        set
        {
            _radius = value;
            Update();
        }
    }

    private uint cornerResolution;

    public RoundedRectangle(Vector2f size, float radius, uint cornerPointCount)
    {
        _size = size;
        _radius = radius;
        cornerResolution = cornerPointCount;
        Update();
    }

    public RoundedRectangle()
    {
        _size = new Vector2f(10, 10);
        _radius = 5;
        cornerResolution = 4;
        Update();
    }

    public void SetCornersRadius(float radius)
    {
        _radius = radius;
        Update();
    }

    public float GetCornersRadius()
    {
        return _radius;
    }

    public void SetCornerPointCount(uint count)
    {
        cornerResolution = count;
        Update();
    }

    public override uint GetPointCount()
    {
        return cornerResolution * 4;
    }

    public override Vector2f GetPoint(uint index)
    {
        if (index >= cornerResolution * 4)
            return new Vector2f(0, 0);

        float deltaAngle = 90.0f / (cornerResolution - 1);
        Vector2f center = new Vector2f();
        uint centerIndex = index / cornerResolution;
        const float pi = 3.141592654f;

        switch (centerIndex)
        {
            case 0:
                center.X = _size.X - _radius;
                center.Y = _radius;
                break;
            case 1:
                center.X = _radius;
                center.Y = _radius;
                break;
            case 2:
                center.X = _radius;
                center.Y = _size.Y - _radius;
                break;
            case 3:
                center.X = _size.X - _radius;
                center.Y = _size.Y - _radius;
                break;
        }

        return new Vector2f(
            _radius * MathF.Cos(deltaAngle * (index - centerIndex) * pi / 180) + center.X,
            -_radius * MathF.Sin(deltaAngle * (index - centerIndex) * pi / 180) + center.Y
        );
    }
}
