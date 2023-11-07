using SFML.Window;
using ProtoEngine.UI;

namespace ProtoEngine.Rendering;

public class Camera
{
    public Vector2 centerWorld;
    public float scale;
    private Screen? viewingScreen;
    public Screen? ViewingScreen
    {
        get => viewingScreen;
        set
        {
            if(value == null) return;
            viewingScreen = value;
            if(viewingScreen.ActiveCamera != this) viewingScreen.ActiveCamera = this;
        }
    }

    public Vector2 WorldSize => (viewingScreen?.Resolution ?? new Vector2(0,0)) * scale;
    public Rect RectBoundsWorld => new Rect(centerWorld, WorldSize).ChangeCenter(centerWorld);

    public Camera(Vector2 center, float scale = 1, Screen? viewingScreen = null)
    {
        this.centerWorld = center;
        this.scale = scale;
        ViewingScreen = viewingScreen;
    }

    public void UpdatePanning(Mouse.Button button)
    {
        if(viewingScreen == null) return;

        if (Mouse.IsButtonPressed(button) && !GUIManager.IsMouseCapturedByUI())
        {
            centerWorld -= viewingScreen.MouseDelta * scale;
        }
    }

    public void UpdateZooming()
    {
        if(viewingScreen == null) return;
        
        //camera zooming
        if (viewingScreen.WheelDelta != 0)
        {
            scale -= viewingScreen.WheelDelta * scale * 0.1f;
        }
    }

    public void FitToRect(Rect fitTo)
    {
        scale = 1/Math.Min(viewingScreen!.Resolution.X / fitTo.size.X, viewingScreen.Resolution.Y / fitTo.size.Y);
        centerWorld = fitTo.Center;
    }
}