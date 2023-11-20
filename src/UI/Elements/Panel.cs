
using ProtoEngine.Rendering;
using SFML.Graphics;

namespace ProtoEngine.UI;

public class Panel : Element
{
    public Element topBar;
    public Element sideGrabber;
    public Element container;

    private bool hasStartedDragging = false;

    public Panel(Element parent, bool resizable = false) : base()
    {
        container = new Element(parent);
        topBar = new Element(container);
        Parent = container;

        container.Style.contentFitY = Fit.Fit;
        container.Style.contentFitX = Fit.Fit;
        container.Style.positionStyle = PositionStyle.Absolute;

        DefaultStyle.radiusTopLeft = "0";
        DefaultStyle.radiusTopRight = "0";
        DefaultStyle.paddingX = "1em";
        DefaultStyle.paddingY = "1em";
        DefaultStyle.outlineWidth = "1px";
        DefaultStyle.gap = "1em";
        DefaultStyle.contentFitY = Fit.Fit;
        DefaultStyle.fillColor = Theme.GlobalTheme.surface1;
        DefaultStyle.outlineColor = Theme.GlobalTheme.surface1Outline;
        DefaultStyle.radiusBottomLeft = "0.5em";
        DefaultStyle.radiusBottomRight = "0.5em";

        NumericProperty topBarHeight = "1em";
        topBar.Style.height = topBarHeight;
        topBar.Style.radiusTopLeft = topBarHeight / 2f;
        topBar.Style.radiusTopRight = topBar.Style.radiusTopLeft;
        topBar.Style.radiusBottomLeft = "0";
        topBar.Style.radiusBottomRight = "0";
        topBar.Style.left = "0";
        topBar.Style.width = Width;
        topBar.Style.fillColor = new (() => ComputedStyle.outlineColor);
        topBar.Style.outlineColor = topBar.Style.fillColor;
        topBar.Style.outlineWidth = "1px";

        if (resizable)
        {
            sideGrabber = new Element(container);
            sideGrabber.Style.positionStyle = PositionStyle.Absolute;
            sideGrabber.Style.width = "10px";
            sideGrabber.Style.height = container.CalculatedHeight;
            sideGrabber.Style.alignSelfX = Alignment.End;
            sideGrabber.Style.marginLeft = "3px";
            sideGrabber.Style.marginRight = "3px";
            sideGrabber.Style.marginTop = container.CalculatedHeight * 0.25f;
            sideGrabber.Style.marginBottom = sideGrabber.Style.marginTop;
            sideGrabber.Style.radius = "5px";
            sideGrabber.Style.left = "-5px";
            sideGrabber.Style.fillColor = Theme.GlobalTheme.surface2;

            sideGrabber.events.OnMouseDrag += (SFML.Window.MouseMoveEventArgs e, Window window) =>
            {
                Style.width.Value += window.globalEvents.MouseDelta.X;
                BuildBox();
                ClampPosition();
                hasStartedDragging = true;
            };
        }

        topBar.events.OnMouseDrag += (SFML.Window.MouseMoveEventArgs e, Window window) =>
        {
            container.Style.top.Value += window.globalEvents.MouseDelta.Y;
            container.Style.left.Value += window.globalEvents.MouseDelta.X;
            ClampPosition();

            if(!hasStartedDragging)
            {
                Element.animatedElements.Add(container);
            }

            hasStartedDragging = true;
        };

        topBar.events.OnMouseDragEnd += (SFML.Window.MouseButtonEventArgs e, Window window) =>
        {
            Element.animatedElements.Remove(container);
            hasStartedDragging = false;
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
            container.Style.top.Value = ProtoMath.Clamp(container.Style.top.Value, 0, container.Parent.InnerHeight.Value - container.Height.Value);
            container.Style.left.Value = ProtoMath.Clamp(container.Style.left.Value, 0, container.Parent.InnerWidth.Value - container.Width.Value);
        }
    }

    public override void BuildBox()
    {
        base.BuildBox();
        if(hasStartedDragging) ClampPosition();
    }
}