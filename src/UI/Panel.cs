using ProtoEngine.Rendering;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ProtoEngine.UI;

public class Panel 
{
    public RectangleShape topBar = new();
    public RectangleShape background = new();

    public float radius = 0;
    public float borderWidth = 1;

    private float _topBarHeight = 15;
    private float TopBarHeight {get => _topBarHeight * theme.scale; set => _topBarHeight = value;}

    public Vector2 position;
    private Vector2 _size;
    public Vector2 Size
    {
        get
        {
            if (_size.Y == 0) 
            {
                _size.Y = CalculateHeight();
            }
            return _size * theme.scale;
        }
        set => _size = value;
    }

    public Rect DrawingArea => new(position + new Vector2(0, TopBarHeight + theme.OutlineThickness), Size - new Vector2(0, TopBarHeight + theme.OutlineThickness));

    public float maxLabelWidth = 0;
    public float maxValueWidth = 0;

    public UITheme theme;
    readonly List<Control> controls = new();

    public RenderWindow window;

    public Panel(Vector2 position, float width, RenderWindow window, UITheme? colorTheme = null)
    {
        theme = colorTheme ?? GUIManager.globalTheme;
        this.window = window;
        Size = new(width, 0);
        this.position = position;
        
        GUIManager.AddPanel(this);
        UpdateRects();
    }

    public void AddControl(Control control)
    {
        controls.Add(control);
        Size = new Vector2(_size.X, CalculateHeight() / theme.scale);
    }

    public void UpdateRects()
    {
        if(topBar.IsBeingDragged(Mouse.Button.Left, window))
        {
            position += MouseGestures.mouseDelta;
        }
        
        background.Position = new Vector2(position.X, position.Y + TopBarHeight);
        background.Size = new Vector2(Size.X, Size.Y - TopBarHeight);
        // background.Radius = radius;
        topBar.Position = new Vector2(position.X, position.Y);
        topBar.Size = new Vector2(Size.X, TopBarHeight);

        background.FillColor = theme.backgroundColor;
        background.OutlineColor = theme.strokeColor;
        background.OutlineThickness = theme.OutlineThickness;

        topBar.FillColor = theme.barColor;
        topBar.OutlineColor = theme.strokeColor;
        topBar.OutlineThickness = theme.OutlineThickness;
    }

    public void Draw()
    {
        window.Draw(background);
        window.Draw(topBar);

        float yOffset = radius;
        float maxLabelWidthTemp = 0;
        float maxValueWidthTemp = 0;

        List<Control> controls = this.controls.OrderBy(control => control.zIndex).ToList();
        
        foreach (Control control in controls)
        {
            control.Draw(yOffset);
            yOffset += control.OuterBoundsWithMargin.Height;

            if (control.LabelWidth > maxLabelWidthTemp && control is not Label) maxLabelWidthTemp = control.LabelWidth;
            if (control is ValuedControl valuedControl && valuedControl.ValueTextWidth > maxValueWidthTemp) maxValueWidthTemp = valuedControl.ValueTextWidth;
        }

        maxLabelWidth = maxLabelWidthTemp;
        maxValueWidth = maxValueWidthTemp;
        Size = new Vector2(_size.X, (yOffset + TopBarHeight + radius) / theme.scale);
    }

    private float CalculateHeight()
    {
        float yOffset = radius;

        foreach (Control control in controls)
        {
            control.InitBounds(yOffset);
            yOffset += control.OuterBoundsWithMargin.Height;
        }

        return yOffset + TopBarHeight + radius;
    }

    public bool ContainsPoint(Vector2 point)
    {
        return point.X > position.X && point.X < position.X + Size.X && point.Y > position.Y && point.Y < position.Y + Size.Y;
    }

    public bool ContainsPoint(Vector2i point)
    {
        return point.X > position.X && point.X < position.X + Size.X && point.Y > position.Y && point.Y < position.Y + Size.Y;
    }

    public bool IsMouseCaptured()
    {
        if(topBar.IsBeingDragged(Mouse.Button.Left, window)) return true;
        
        for (int i = 0; i < controls.Count; i++)
        {
            if(controls[i].IsMouseCaptured())
            {
                return true;
            }
        }

        return false;
    }

    public void Destroy()
    {
        GUIManager.RemovePanel(this);
    }
}