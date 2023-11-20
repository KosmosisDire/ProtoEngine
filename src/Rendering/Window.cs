using SFML.Graphics;
using SFML.Window;
using ComputeSharp;
using ProtoEngine.Rendering.Internal;
using ProtoEngine.UI;

namespace ProtoEngine.Rendering;

public class Window : Element
{
    private readonly RenderWindow _window;
    public RenderWindow SFMLWindow => _window;

    private ReadWriteTexture2D<uint> renderBuffer;
    private ReadBackTexture2D<uint> renderBufferRead;
    public ReadWriteTexture2D<uint> ScreenBuffer => renderBuffer;
    private byte[] renderBufferBytes;
    private bool renderBufferDirty = false;
    public Sprite RenderSprite { get; private set; }


    public Color fillColor;
    public Vector2 Size => new(Width, Height);
    public new NumericProperty Width
    {
        get => ComputedStyle.width;
        set
        {
            Style.width = value;
            ResizeScreen(new Vector2((float)value, (float)Height));
        }
    }
    public new NumericProperty Height
    {
        get => ComputedStyle.height;
        set
        {
            Style.height = value;
            ResizeScreen(new Vector2((float)Width, (float)value));
        }
    }

    public Vector2 WorldSize => ScreenToWorld(Size);
    public float WorldWidth => ScreenToWorld(Width);
    public float WorldHeight => ScreenToWorld(Height);
    public Events globalEvents = new();
    public Element? TargetElement { get; private set;}
    public List<Action> drawActions = new();

    public float Scale1920Ratio {get; private set;} = 1f;

    private Camera? _activeCamera;
    public Camera? ActiveCamera 
    { 
        get => _activeCamera; 
        set 
        {
            if(value == null) return;
            _activeCamera = value;
            if(_activeCamera.ViewingWindow != this) _activeCamera.ViewingWindow = this;
        }
    }

    public Window(bool fullscreen, string title, Loop drawLoop, Vector2? size = null)
    {
        if (fullscreen) size = new Vector2(VideoMode.DesktopMode.Width, VideoMode.DesktopMode.Height);

        var width = new AbsPx(() => _window?.Size.X ?? size?.X ?? 800);
        var height = new AbsPx(() => _window?.Size.Y ?? size?.Y ?? 600);
        this.Width = width;
        this.Height = height;

        BuildBox();

        _window = new RenderWindow(new VideoMode((uint)this.Size.X, (uint)this.Size.Y), title, fullscreen ? Styles.Fullscreen : Styles.Default, new ContextSettings() { AntialiasingLevel = 8 });
        _window.SetFramerateLimit((uint)drawLoop.targetFPS);

        ResizeScreen(this.Size);
        InitializeEvents();

        globalEvents.Resized += (SizeEventArgs e, Window w) =>
        {
            ResizeScreen(new Vector2((float)e.Width, (float)e.Height));
        };

        globalEvents.MouseMoved += (MouseMoveEventArgs e, Window w) =>
        {
            var target = GetElementAtPosition(new Vector2(e.X, e.Y));
            target?.events.OnMouseMoved?.Invoke(e, this);
            
            if (TargetElement is not null && Events.IsMouseAnyDown && TargetElement.events.OnMouseDrag != null)
            {
                TargetElement.events.OnMouseDrag?.Invoke(e, this);
                TargetElement.events.isMouseDragging = true;
                return;
            }

            if (target != TargetElement)
            {
                if (TargetElement is not null) 
                {
                    TargetElement.events.OnMouseLeft?.Invoke(this);
                    TargetElement.events.isMouseOver = false;
                }

                if (target is not null)
                {
                    target.events.OnMouseEntered?.Invoke(this);
                    target.events.isMouseOver = true;
                }

                TargetElement = target;
            }
        };

        globalEvents.MouseButtonPressed += (MouseButtonEventArgs e, Window w) =>
        {
            var target = GetElementAtPosition(new Vector2(e.X, e.Y));
            if(target is not null)
            {
                target.events.OnMouseButtonPressed?.Invoke(e, this);
                switch (e.Button)
                {
                    case Mouse.Button.Left:
                        target.events.IsMouseLeftDown = true;
                        break;
                    case Mouse.Button.Right:
                        target.events.isMouseRightDown = true;
                        break;
                    case Mouse.Button.Middle:
                        target.events.isMouseMiddleDown = true;
                        break;
                }
                target.events.lastMousePress = e;
            }
        };

        globalEvents.MouseButtonReleased += (MouseButtonEventArgs e, Window w) =>
        {
            var target = GetElementAtPosition(new Vector2(e.X, e.Y));

            if (TargetElement is not null)
            {
                if (TargetElement.events.isMouseDragging)
                {
                    TargetElement.events.OnMouseDragEnd?.Invoke(e, this);
                    TargetElement.events.isMouseDragging = false;

                    if (target != TargetElement)
                    {
                        if (target is not null)
                        {
                            target.events.OnMouseEntered?.Invoke(this);
                        }

                        TargetElement.events.isMouseOver = false;
                        TargetElement = target;

                        return;
                    }
                }
            }

            if(target is not null)
            {
                target.events.OnMouseButtonReleased?.Invoke(e, this);
                switch (e.Button)
                {
                    case Mouse.Button.Left:
                        target.events.IsMouseLeftDown = false;
                        break;
                    case Mouse.Button.Right:
                        target.events.isMouseRightDown = false;
                        break;
                    case Mouse.Button.Middle:
                        target.events.isMouseMiddleDown = false;
                        break;
                }
                target.events.lastMouseRelease = e;
            }

            if (target != Input.FocusedInput && Input.FocusedInput is not null)
            {
                Input.FocusedInput.events.OnDefocus?.Invoke();
                switch (e.Button)
                {
                    case Mouse.Button.Left:
                        Input.FocusedInput.events.IsMouseLeftDown = false;
                        break;
                    case Mouse.Button.Right:
                        Input.FocusedInput.events.isMouseRightDown = false;
                        break;
                    case Mouse.Button.Middle:
                        Input.FocusedInput.events.isMouseMiddleDown = false;
                        break;
                }
            }
        };

        globalEvents.MouseWheelScrolled += (MouseWheelScrollEventArgs e, Window w) =>
        {
            var target = GetElementAtPosition(new Vector2(e.X, e.Y));
            target?.events.OnMouseWheelScrolled?.Invoke(e, this);
        };

        globalEvents.TouchBegan += (TouchEventArgs e, Window w) =>
        {
            var target = GetElementAtPosition(new Vector2(e.X, e.Y));
            if (target is not null)
            {
                target.events.OnTouchBegan?.Invoke(e, this);
                target.events.lastTouchPress = e;
            }
        };

        globalEvents.TouchMoved += (TouchEventArgs e, Window w) =>
        {
            var target = GetElementAtPosition(new Vector2(e.X, e.Y));
            target?.events.OnTouchMoved?.Invoke(e, this);
        };

        globalEvents.TouchEnded += (TouchEventArgs e, Window w) =>
        {
            var target = GetElementAtPosition(new Vector2(e.X, e.Y));
            if (target is not null)
            {
                target.events.OnTouchEnded?.Invoke(e, this);
                target.events.lastTouchRelease = e;
            }
        };

        // mouse delta
        var lastMousePos = new Vector2(0, 0);
        globalEvents.MouseMoved += (MouseMoveEventArgs e, Window w) =>
        {
            if (lastMousePos == new Vector2(0, 0)) lastMousePos = new Vector2(e.X, e.Y);
            globalEvents.MouseDelta = new Vector2(e.X, e.Y) - lastMousePos;
            lastMousePos = new Vector2(e.X, e.Y);
        };

        globalEvents.KeyPressed += (KeyEventArgs e, Window w) =>
        {
            Input.FocusedInput?.events.OnKeyPressed?.Invoke(e, this);
        };

        drawLoop.Connect(Update);
    }

    public void ScaleRenderTexture(float scale)
    {
        var newSize = new Vector2((float)Width * scale, (float)Height * scale);
        ResizeScreenTexture(newSize, 1/scale);
    }

    private void InitializeEvents()
    {
        _window.Closed += (obj, e) => globalEvents.Closed?.Invoke(this);
        _window.Resized += (obj, e) => globalEvents.Resized?.Invoke(e, this);
        _window.KeyPressed += (obj, e) => globalEvents.KeyPressed?.Invoke(e, this);
        _window.KeyReleased += (obj, e) => globalEvents.KeyReleased?.Invoke(e, this);
        _window.TextEntered += (obj, e) => globalEvents.TextEntered?.Invoke(e, this);
        _window.MouseWheelScrolled += (obj, e) => globalEvents.MouseWheelScrolled?.Invoke(e, this);
        _window.MouseButtonPressed += (obj, e) => globalEvents.MouseButtonPressed?.Invoke(e, this);
        _window.MouseButtonReleased += (obj, e) => globalEvents.MouseButtonReleased?.Invoke(e, this);
        _window.MouseMoved += (obj, e) => globalEvents.MouseMoved?.Invoke(e, this);
        _window.MouseEntered += (obj, e) => globalEvents.MouseEntered?.Invoke(this);
        _window.MouseLeft += (obj, e) => globalEvents.MouseLeft?.Invoke(this);
        _window.JoystickButtonPressed += (obj, e) => globalEvents.JoystickButtonPressed?.Invoke(e, this);
        _window.JoystickButtonReleased += (obj, e) => globalEvents.JoystickButtonReleased?.Invoke(e, this);
        _window.JoystickMoved += (obj, e) => globalEvents.JoystickMoved?.Invoke(e, this);
        _window.JoystickConnected += (obj, e) => globalEvents.JoystickConnected?.Invoke(e, this);
        _window.JoystickDisconnected += (obj, e) => globalEvents.JoystickDisconnected?.Invoke(e, this);
        _window.TouchBegan += (obj, e) => globalEvents.TouchBegan?.Invoke(e, this);
        _window.TouchMoved += (obj, e) => globalEvents.TouchMoved?.Invoke(e, this);
        _window.TouchEnded += (obj, e) => globalEvents.TouchEnded?.Invoke(e, this);
        _window.SensorChanged += (obj, e) => globalEvents.SensorChanged?.Invoke(e, this);

        globalEvents.Closed += (win) => _window.Close();
    }

    public Vector2 ScreenToWorld(Vector2 screenPos)
    {
        return (screenPos * ActiveCamera?.scale) + (ActiveCamera?.centerWorld - ActiveCamera?.WorldSize/2) ?? screenPos;
    }

    public float ScreenToWorld(float scalar)
    {
        return scalar * ActiveCamera?.scale ?? scalar;
    }

    public Vector2 WorldToScreen(Vector2 worldPos)
    {
        worldPos = ((worldPos - (ActiveCamera?.centerWorld - ActiveCamera?.WorldSize/2)) / ActiveCamera?.scale) ?? worldPos;
        return worldPos;
    }

    public float WorldToScreen(float scalar)
    {
        scalar = scalar / ActiveCamera?.scale ?? scalar;
        return scalar;
    }

    public void Draw(Drawable drawable, bool disposeObject = false)
    {
        drawActions.Add(() => 
        {
            _window.Draw(drawable);
            if (disposeObject && drawable is IDisposable disposable)
            {
                disposable.Dispose();
            }
        });
    }

    private int elementSection = 0;
    private int elementSectionCount = 50;

    public void Update(float dt)
    {
        _window.DispatchEvents();
        _window.Clear(fillColor);

        UpdateScreenTexture();
        _window.Draw(RenderSprite);

        foreach (var action in drawActions) action();
        drawActions.Clear();

        BuildBox();
        var children = GetChildrenRecursive();
        if (children.Count > 0)
        {
            elementSectionCount = (int)MathF.Max(MathF.Ceiling(children.Count / 50f), 1);
            int sectionSize = (int)MathF.Max(MathF.Ceiling(children.Count / (float)elementSectionCount), 1);
            for (int i = sectionSize * elementSection; i < sectionSize * (elementSection + 1); i++)
            {
                if (i >= children.Count) break;
                var child = children[i];
                child.BuildBox();
            }

            elementSection = (elementSection + 1) % elementSectionCount;
        }

        if (animatedElements.Count > 0)
        {
            for (int i = 0; i < animatedElements.Count; i++)
            {
                if (i >= animatedElements.Count) break;
                var element = animatedElements[i];
                element.BuildBox();
            }
        }
        
        _window.Draw(this);
        _window.Display();
    }

    public void Close()
    {
        globalEvents.Closed?.Invoke(this);
    }

    private void ResizeScreen(Vector2 size)
    {
        if (size.X == 0) size.X = 1;
        if (size.Y == 0) size.Y = 1;

        if (_window != null) 
        {
            _window.Size = size;
            _window.SetView(new View(new FloatRect(0, 0, size.X, size.Y)));
        }

        ResizeScreenTexture(size);

        BuildBox();
        GetChildrenRecursive().ForEach((child) => child.BuildBox());

        Scale1920Ratio = size.X / 1920f;
    }

    private void ResizeScreenTexture(Vector2 size, float scale = 1)
    {
        renderBuffer?.Dispose();
        renderBuffer = Application.GPU.AllocateReadWriteTexture2D<uint>((int)size.X, (int)size.Y, AllocationMode.Clear);
        RenderSprite?.Dispose();
        RenderSprite = new Sprite(new Texture((uint)size.X, (uint)size.Y));
        renderBufferBytes = new byte[(int)size.X * (int)size.Y * 4];
        renderBufferRead?.Dispose();
        renderBufferRead = Application.GPU.AllocateReadBackTexture2D<uint>((int)size.X, (int)size.Y);

        RenderSprite.Scale = new Vector2(scale, scale);
    }

    private void UpdateScreenTexture()
    {
        if (!renderBufferDirty) return;

        renderBufferRead.CopyFrom(renderBuffer);
        unsafe
        {
            fixed (byte* bytesPtr = &renderBufferBytes[0])
            {
                uint* arrayPtr = renderBufferRead.View.DangerousGetAddressAndByteStride(out _);
                Buffer.MemoryCopy(arrayPtr, bytesPtr, renderBufferBytes.Length, renderBufferBytes.Length);
            }
        }
        RenderSprite.Texture.Update(renderBufferBytes);
        
        var computeContext = Application.GPU.CreateComputeContext();
        computeContext.Clear(renderBuffer);
        renderBufferDirty = false;
        computeContext.Dispose();
    }

    public void DrawCircles(ReadWriteBuffer<float2> positions, ReadWriteBuffer<uint> colors, ReadWriteBuffer<int> active, float radius)
    {
        int forCount = positions.Length;
        int threadCount = (int)Math.Min(forCount, 1024);
        Application.GPU.For(forCount, 1, 1, threadCount, 1, 1, new DrawCirclesKernel(positions, colors, renderBuffer, active, (int2)Size, ActiveCamera!.RectBoundsWorld, (ActiveCamera?.scale * RenderSprite.Scale.X) ?? 1, radius, false, fillColor.ToRGBA()));
        renderBufferDirty = true;
    }

    public void DrawLines(Vector2[] starts, Vector2[] ends, Color color)
    {
        if(starts.Length != ends.Length) throw new Exception("Starts and ends must be the same length");

        var startsArray = starts.Select(v => new float2(v.X, v.Y)).ToArray();
        var endsArray = ends.Select(v => new float2(v.X, v.Y)).ToArray();

        var startsBuffer = Application.GPU.AllocateReadOnlyBuffer(startsArray);
        var endsBuffer = Application.GPU.AllocateReadOnlyBuffer(endsArray);

        int forCount = starts.Length;
        int threadCount = (int)Math.Min(forCount, 1024);
        GraphicsDevice.GetDefault().For(forCount, 1, 1, threadCount, 1, 1, new DrawLinesKernel(startsBuffer, endsBuffer, renderBuffer, (int2)Size, ActiveCamera!.RectBoundsWorld, (ActiveCamera?.scale * RenderSprite.Scale.X) ?? 1, color.ToUInt32()));

        startsBuffer.Dispose();
        endsBuffer.Dispose();

        renderBufferDirty = true;
    }

    public void DrawLine(Vector2 start, Vector2 end, Color color, float thickness = 1)
    {
        if (thickness <= 1)
        {
            var line = new VertexArray(PrimitiveType.Lines, 2);
            line[0] = new Vertex(WorldToScreen(start), color);
            line[1] = new Vertex(WorldToScreen(end), color);
            Draw(line, true);
        }
        else // we must draw rectangles instead
        {
            var startScreen = WorldToScreen(start);
            var endScreen = WorldToScreen(end);

            var x1 = (int)startScreen.X;
            var y1 = (int)startScreen.Y;
            var x2 = (int)endScreen.X;
            var y2 = (int)endScreen.Y;

            var dx = x2 - x1;
            var dy = y2 - y1;

            var length = MathF.Sqrt(dx * dx + dy * dy);
            var angle = MathF.Atan2(dy, dx);

            var rect = new RectangleShape(new Vector2(length, thickness))
            {
                Position = new Vector2(x1, y1),
                FillColor = color,
                Rotation = angle * 180 / MathF.PI,
                Origin = new Vector2(0, thickness / 2)
            };

            Draw(rect, true);
        }
        
    }

    public void DrawCircle(Vector2 pos, float radius, Color color)
    {
        var circle = new CircleShape(radius)
        {
            Position = WorldToScreen(pos),
            FillColor = color,
            Origin = new(radius, radius)
        };
        Draw(circle, true);
    }

    public void DrawText(string text, Vector2 pos, Color color, uint size = 30)
    {

        var textObj = new Text(text, Theme.GlobalTheme.mainFont, size)
        {
            Position = WorldToScreen(pos),
            FillColor = color,
            Origin = new(0, size)
        };
        Draw(textObj, true);
        
    }
}