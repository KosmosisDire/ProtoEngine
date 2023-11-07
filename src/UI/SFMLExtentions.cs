using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ProtoEngine.UI;

public enum CenterAxis
{
    X,
    Y,
    Both
}

public static class ProtoSFMLExtentions
{
    public static Dictionary<Shape, bool> dragging = new();
    public static bool IsBeingDragged(this Shape shape, Mouse.Button button, Window? onWindow)
    {
        if (MouseGestures.ButtonDown(button))
        {
            Vector2i mousePos = onWindow == null ? Mouse.GetPosition() : Mouse.GetPosition(onWindow);
            if (shape.GetGlobalBounds().Contains(mousePos.X, mousePos.Y))
            {
                if (!dragging.ContainsKey(shape))
                {
                    dragging.Add(shape, true);
                }
                else
                {
                    dragging[shape] = true;
                }
            }
        }

        if (MouseGestures.ButtonUp(button))
        {
            if (dragging.ContainsKey(shape))
            {
                dragging[shape] = false;
            }
        }

        if (dragging.ContainsKey(shape))
        {
            return dragging[shape];
        }
        else
        {
            return false;
        }
    }

    public static bool Clicked(this Shape shape, Mouse.Button button, Window? onWindow)
    {
        if (MouseGestures.ButtonDown(button))
        {
            Vector2i mousePos = onWindow == null ? Mouse.GetPosition() : Mouse.GetPosition(onWindow);

            if (shape.GetGlobalBounds().Contains(mousePos.X, mousePos.Y))
            {
                return true;
            }
        }

        return false;
    }

    public static Vector2 Center(this Shape shape)
    {
        return new Vector2(shape.Position.X + shape.GetGlobalBounds().Width / 2, shape.Position.Y + shape.GetGlobalBounds().Height / 2);
    }

    public static Vector2 Center(this SFML.Graphics.Text text)
    {
        return new Vector2(text.Position.X + text.GetGlobalBounds().Width / 2, text.Position.Y + text.GetGlobalBounds().Height / 2);
    }

    public static float Width(this SFML.Graphics.Text text)
    {
        return text.GetGlobalBounds().Width;
    }

    public static float Height(this SFML.Graphics.Text text)
    {
        return text.GetGlobalBounds().Height;
    }

    public static Color Lerp(this Color start, Color end, float percent)
    {
        return new Color(
            (byte)(start.R + (end.R - start.R) * percent),
            (byte)(start.G + (end.G - start.G) * percent),
            (byte)(start.B + (end.B - start.B) * percent),
            (byte)(start.A + (end.A - start.A) * percent)
        );
    }
}