using SFML.Graphics;
using SFML.Window;
using ComputeSharp;
using ProtoEngine.UI;
using ProtoEngine.UI2;
using ProtoEngine.Rendering.Internal;

namespace ProtoEngine.Rendering;

public class Screen
{
    protected int renderWidth;
    protected int renderHeight;

    protected int windowWidth;
    protected int windowHeight;

    public Vector2 RenderScale2D => new((float)renderWidth / windowWidth, (float)renderHeight / windowHeight);
    public float RenderScale => (float)renderWidth / windowWidth;
    public Vector2 Resolution => new(renderWidth, renderHeight);
    public Rect RenderBounds => new(new Vector2(0,0) - Resolution / 2, new(renderWidth, renderHeight));

    public bool IsMouseOnScreen { get; private set; }
    public Vector2 MousePosition { get; private set; }
    private Vector2 lastMousePosition;
    public Vector2 MouseDelta { get; private set; }
    public Vector2 MouseDeltaWorld { get; private set; }
    public float WheelDelta { get; private set; }
    public event Action OnClose;

    public RenderWindow Window { get; private set;}
    private Camera? activeCamera;
    public Camera? ActiveCamera 
    { 
        get => activeCamera; 
        set 
        {
            if(value == null) return;
            activeCamera = value;
            if(activeCamera.ViewingScreen != this) activeCamera.ViewingScreen = this;
        }
    }

    uint[] bitmapDataInt;
    readonly byte[] bitmapData;
    readonly uint[] clearBuffer;
    readonly ReadWriteTexture2D<uint> bitmapBuffer;
    readonly Sprite renderSprite;

    public Screen(string name, Loop drawLoop, int width = 1920, int height = 1080, bool fullscreen = false)
    {
        this.renderWidth = width;
        this.renderHeight = height;
        
        this.windowWidth = fullscreen ? (int)VideoMode.DesktopMode.Width : width;
        this.windowHeight = fullscreen ? (int)VideoMode.DesktopMode.Height : height;

        Window = new RenderWindow(new VideoMode((uint)this.windowWidth, (uint)this.windowHeight), name, fullscreen ? Styles.Fullscreen : Styles.Default);
        Window.SetVerticalSyncEnabled(true);
        Window.SetFramerateLimit(144);

        OnClose += () => Window.Close();

        Window.Closed += (obj, e) => 
        { 
            Close();
        };

        Window.MouseWheelScrolled += (sender, e) => 
        {
            WheelDelta = e.Delta;
        };

        renderSprite = new Sprite(new Texture(new Image((uint)renderWidth, (uint)renderHeight)))
        {
            Scale = 1 / RenderScale2D
        };

        bitmapData = new byte[renderWidth * renderHeight * 4];
        bitmapDataInt = new uint[renderWidth * renderHeight];
        clearBuffer = new uint[renderWidth * renderHeight];

        bitmapBuffer = GraphicsDevice.GetDefault().AllocateReadWriteTexture2D<uint>(renderWidth, renderHeight, AllocationMode.Default);
        bitmapBuffer.CopyFrom(clearBuffer);
        
        drawLoop.RunAction(() => drawLoop.Connect(UpdateScreen));
        
        GUIManager.window = Window;
    }

    public Vector2 ScreenToWorld(Vector2 screenPos)
    {
        screenPos *= RenderScale2D;
        return (screenPos * activeCamera?.scale) + (activeCamera?.centerWorld - activeCamera?.WorldSize/2) ?? screenPos;
    }

    public float ScreenToWorld(float scalar)
    {
        scalar *= RenderScale;
        return scalar * activeCamera?.scale ?? scalar;
    }

    public Vector2 WorldToScreen(Vector2 worldPos)
    {
        worldPos = ((worldPos - (activeCamera?.centerWorld - activeCamera?.WorldSize/2)) / activeCamera?.scale) ?? worldPos;
        return worldPos / RenderScale2D;
    }

    public float WorldToScreen(float scalar)
    {
        scalar = scalar / activeCamera?.scale ?? scalar;
        return scalar / RenderScale;
    }

    public Vector2 RenderScreenToWorld(Vector2 screenPos)
    {
        return (screenPos * activeCamera?.scale) + (activeCamera?.centerWorld - activeCamera?.WorldSize/2) ?? screenPos;
    }

    public float RenderScreenToWorld(float scalar)
    {
        return scalar * activeCamera?.scale ?? scalar;
    }

    public Vector2 WorldToRenderScreen(Vector2 worldPos)
    {
        worldPos = ((worldPos - (activeCamera?.centerWorld - activeCamera?.WorldSize/2)) / activeCamera?.scale) ?? worldPos;
        return worldPos;
    }

    public float WorldToRenderScreen(float scalar)
    {
        scalar = scalar / activeCamera?.scale ?? scalar;
        return scalar;
    }


    public List<Action> drawActions = new();
    void UpdateScreen(float dt) 
    {
        WheelDelta = 0;
        Window.DispatchEvents();

        MousePosition = Window.MapPixelToCoords(Mouse.GetPosition(Window)) * RenderScale2D;
        IsMouseOnScreen = MousePosition.X >= 0 && MousePosition.X < renderWidth && MousePosition.Y >= 0 && MousePosition.Y < renderHeight;
        MouseDelta = MousePosition - lastMousePosition;
        MouseDeltaWorld = ScreenToWorld(MousePosition) - ScreenToWorld(lastMousePosition);
        lastMousePosition = MousePosition;

        Window.Clear();
        ApplyBitmap();
        Window.Draw(renderSprite);
        drawActions.ForEach(a => a.Invoke());
        drawActions.Clear();
        GUIManager.Update(dt);
        Window.Display();
    }

    public void Close()
    {
        OnClose?.Invoke();
    }

    private Color _fillColor;
    public Color FillColor
    {
        get => _fillColor;
        set {
            var c = value.ToUInt32();
            for (int i = 0; i < clearBuffer.Length; i++)
            {
                clearBuffer[i] = c;
            }
            _fillColor = value;
        }
    }

    public void ApplyBitmap()
    {
        Buffer.BlockCopy(bitmapDataInt, 0, bitmapData, 0, bitmapData.Length);
        renderSprite.Texture.Update(bitmapData);
    }

    public void Clear()
    {
        clearBuffer.CopyTo(bitmapDataInt, 0);
        bitmapBuffer.CopyFrom(clearBuffer);
    }

    public void ApplyGPUDraw()
    {
        bitmapBuffer.CopyTo(bitmapDataInt);
    }

    public void Draw(Drawable drawable)
    {
        drawActions.Add(() => Window.Draw(drawable));
    }

    public void DrawCircles(ReadWriteBuffer<float2> positions, ReadWriteBuffer<uint> colors, ReadWriteBuffer<int> active, float radius)
    {
        GraphicsDevice.GetDefault().For(positions.Length, 1, 1, 1024, 1, 1, new DrawCirclesKernel(positions, colors, bitmapBuffer, active, new int2(renderWidth, renderHeight), activeCamera?.RectBoundsWorld ?? RenderBounds, activeCamera?.scale ?? 1, radius, false, FillColor.ToRGBA()));
    }

    public void DrawLines(Vector2[] starts, Vector2[] ends, Color color)
    {
        if(starts.Length != ends.Length) throw new Exception("Starts and ends must be the same length");

        var startsArray = starts.Select(v => new float2(v.X, v.Y)).ToArray();
        var endsArray = ends.Select(v => new float2(v.X, v.Y)).ToArray();

        var startsBuffer = Application.GPU.AllocateReadOnlyBuffer(startsArray);
        var endsBuffer = Application.GPU.AllocateReadOnlyBuffer(endsArray);

        GraphicsDevice.GetDefault().For(starts.Length, new DrawLinesKernel(startsBuffer, endsBuffer, bitmapBuffer, new int2(renderWidth, renderHeight), activeCamera?.RectBoundsWorld ?? RenderBounds, activeCamera?.scale ?? 1, color.ToUInt32()));

        startsBuffer.Dispose();
        endsBuffer.Dispose();
    }
    
    public void DrawLine(Vector2 start, Vector2 end, Color color, float thickness = 1)
    {
        if (thickness <= 1)
        {
            drawActions.Add(() => 
            {
                var line = new VertexArray(PrimitiveType.Lines, 2);
                line[0] = new Vertex(WorldToRenderScreen(start), color);
                line[1] = new Vertex(WorldToRenderScreen(end), color);
                Window.Draw(line);
                line.Dispose();
            });
        }
        else // we must draw rectangles instead
        {
            var startScreen = WorldToRenderScreen(start);
            var endScreen = WorldToRenderScreen(end);

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

            drawActions.Add(() => 
            {
                Window.Draw(rect);
                rect.Dispose();
            });
        }
        
    }

    public void DrawCircle(Vector2 pos, float radius, Color color)
    {
        drawActions.Add(() => 
        {
            var circle = new CircleShape(radius)
            {
                Position = WorldToRenderScreen(pos),
                FillColor = color,
                Origin = new(radius, radius)
            };
            Window.Draw(circle);
            circle.Dispose();
        });
    }

    public void DrawText(string text, Vector2 pos, Color color, uint size = 30)
    {
        drawActions.Add(() => 
        {
            var textObj = new SFML.Graphics.Text(text, GUIManager.globalTheme.font, size)
            {
                Position = WorldToRenderScreen(pos),
                FillColor = color,
                Origin = new(0, size)
            };
            Window.Draw(textObj);
            textObj.Dispose();
        });
    }
}
