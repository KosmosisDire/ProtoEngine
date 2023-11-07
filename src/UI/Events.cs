using SFML.Window;
using Window = ProtoEngine.Rendering.Window;

namespace ProtoEngine.UI;

public struct ElementEvents
{
    public Action<MouseWheelScrollEventArgs, Window> OnMouseWheelScrolled;
    public Action<MouseButtonEventArgs, Window> OnMouseButtonPressed;
    public Action<MouseButtonEventArgs, Window> OnMouseButtonReleased;
    public Action<MouseMoveEventArgs, Window> OnMouseDrag;
    public Action<MouseButtonEventArgs, Window> OnMouseDragEnd;
    public Action<MouseMoveEventArgs, Window> OnMouseMoved;
    public Action<Window> OnMouseEntered;
    public Action<Window> OnMouseLeft;
    public Action<TouchEventArgs, Window> OnTouchBegan;
    public Action<TouchEventArgs, Window> OnTouchMoved;
    public Action<TouchEventArgs, Window> OnTouchEnded;
    public Action<KeyEventArgs, Window> OnKeyPressed;
    public Action OnFocus;
    public Action OnDefocus;

    public bool isMouseOver;
    public bool isMouseDragging;
    public bool IsMouseLeftDown;
    public bool isMouseRightDown;
    public bool isMouseMiddleDown;
    public readonly bool IsMouseAnyDown => IsMouseLeftDown || isMouseRightDown || isMouseMiddleDown;

    public MouseButtonEventArgs lastMousePress;
    public MouseButtonEventArgs lastMouseRelease;
    public TouchEventArgs lastTouchPress;
    public TouchEventArgs lastTouchRelease;

    public readonly Vector2 LastMousePressPosition => new(lastMousePress.X, lastMousePress.Y);
    public readonly Vector2 LastMouseReleasePosition => new(lastMouseRelease.X, lastMouseRelease.Y);
    public readonly Vector2 LastTouchPressPosition => new(lastTouchPress.X, lastTouchPress.Y);
    public readonly Vector2 LastTouchReleasePosition => new(lastTouchRelease.X, lastTouchRelease.Y);
}

public struct InputEvents<T>
{
    public Action<T> OnChange;
    public Action<T> Submit;
}

public struct Events
{
    public Action<Window> Closed;
    public Action<SizeEventArgs, Window> Resized;
    public Action<KeyEventArgs, Window> KeyPressed;
    public Action<KeyEventArgs, Window> KeyReleased;
    public Action<TextEventArgs, Window> TextEntered;
    public Action<MouseWheelScrollEventArgs, Window> MouseWheelScrolled;
    public Action<MouseButtonEventArgs, Window> MouseButtonPressed;
    public Action<MouseButtonEventArgs, Window> MouseButtonReleased;
    public Action<MouseMoveEventArgs, Window> MouseMoved;
    public Action<Window> MouseEntered;
    public Action<Window> MouseLeft;
    public Action<JoystickButtonEventArgs, Window> JoystickButtonPressed;
    public Action<JoystickButtonEventArgs, Window> JoystickButtonReleased;
    public Action<JoystickMoveEventArgs, Window> JoystickMoved;
    public Action<JoystickConnectEventArgs, Window> JoystickConnected;
    public Action<JoystickConnectEventArgs, Window> JoystickDisconnected;
    public Action<TouchEventArgs, Window> TouchBegan;
    public Action<TouchEventArgs, Window> TouchMoved;
    public Action<TouchEventArgs, Window> TouchEnded;
    public Action<SensorEventArgs, Window> SensorChanged;

    public static bool IsMouseLeftDown => Mouse.IsButtonPressed(Mouse.Button.Left);
    public static bool IsMouseRightDown => Mouse.IsButtonPressed(Mouse.Button.Right);
    public static bool IsMouseMiddleDown => Mouse.IsButtonPressed(Mouse.Button.Middle);
    public static bool IsMouseAnyDown => IsMouseLeftDown || IsMouseRightDown || IsMouseMiddleDown;
    public static Vector2 MousePosition => Mouse.GetPosition();
    public Vector2 MouseDelta;
}