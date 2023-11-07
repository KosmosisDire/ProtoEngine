
using ProtoEngine.Rendering;
using SFML.Graphics;

namespace ProtoEngine.UI3;

public class Panel : Element
{
    public Element topBar;
    public Element container;

    public Panel(Element parent, Style baseStyle, params Element[] children) : base()
    {
        SetBaseStyle(baseStyle);
        container = new Element(parent);
        topBar = new Element(container, baseStyle);
        Parent = container;

        container.Style.contentFitX = Fit.Fit;
        container.Style.contentFitY = Fit.Fit;
        container.Style.positionStyle = PositionStyle.Absolute;

        DefaultStyle.radiusTopLeft = 0;
        DefaultStyle.radiusTopRight = 0;
        DefaultStyle.contentFitY = Fit.Fit;
        DefaultStyle.paddingX = new Em(1);
        DefaultStyle.paddingY = new Em(1);

        var topBarHeight = new Em(1.5f);
        topBar.Style.height = topBarHeight;
        topBar.Style.bottom = Top;
        topBar.Style.left = 0;
        topBar.Style.width = Width;
        topBar.Style.radiusTopLeft = topBarHeight / 2;
        topBar.Style.radiusTopRight = topBarHeight / 2;
        topBar.Style.radiusBottomLeft = 0;
        topBar.Style.radiusBottomRight = 0;
        topBar.Style.fillColor = baseStyle.outlineColor;

        topBar.events.OnMouseDrag += (SFML.Window.MouseMoveEventArgs e, Window window) =>
        {
            container.Style.top.Value += window.globalEvents.MouseDelta.Y;
            container.Style.left.Value += window.globalEvents.MouseDelta.X;
            ClampPosition();
        };

        foreach (var child in children)
        {
            child.Parent = this;
        }
    }

    public void ClampPosition()
    {
        if (container is not null && container.Parent is not null)
        {
            container.Style.top.Value = ProtoMath.Clamp(container.Style.top.Value, 0, container.Parent.InnerHeight.pixels - container.Height.pixels);
            container.Style.left.Value = ProtoMath.Clamp(container.Style.left.Value, 0, container.Parent.InnerWidth.pixels - container.Width.pixels);
        }
    }

    public override void BuildBox()
    {
        base.BuildBox();
        ClampPosition();
    }
}