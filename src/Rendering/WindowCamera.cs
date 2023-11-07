using SFML.Window;
using ProtoEngine.UI;

namespace ProtoEngine.Rendering;

public class WindowCamera
{
    public Vector2 centerWorld;
    public float scale;
    private Window? _viewingWindow;
    public Window? ViewingWindow
    {
        get => _viewingWindow;
        set
        {
            if(value == null) return;
            _viewingWindow = value;
            if(_viewingWindow.ActiveCamera != this) _viewingWindow.ActiveCamera = this;
        }
    }

    public Vector2 WorldSize => (_viewingWindow?.Size ?? new Vector2(0,0)) * scale;
    public Rect RectBoundsWorld => new Rect(centerWorld, WorldSize).ChangeCenter(centerWorld);

    public WindowCamera(Vector2 center, float scale = 1, Window? viewingWindow = null)
    {
        this.centerWorld = center;
        this.scale = scale;
        ViewingWindow = viewingWindow;
    }

    public void Pan(Vector2 delta)
    {
        if (!GUIManager.IsMouseCapturedByUI())
        {
            centerWorld -= delta * scale;
        }
    }

    public void Zoom(float delta)
    {
        scale -= delta * scale * 0.1f;
    }

    public void FitToRect(Rect fitTo)
    {
        scale = 1/Math.Min(ViewingWindow!.WorldWidth / fitTo.size.X, ViewingWindow.WorldHeight / fitTo.size.Y);
        centerWorld = fitTo.Center;
    }
}