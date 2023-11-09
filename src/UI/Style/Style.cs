using SFML.Graphics;

namespace ProtoEngine.UI;

public struct Style
{
    public int importance;
    public NumericProperty width;
    public NumericProperty height;
    public NumericProperty minWidth;
    public NumericProperty minHeight;
    public NumericProperty maxWidth;
    public NumericProperty maxHeight;

    public NumericProperty left;
    public NumericProperty right;
    public NumericProperty top;
    public NumericProperty bottom;

    public NumericProperty marginLeft;
    public NumericProperty marginRight;
    public NumericProperty marginTop;
    public NumericProperty marginBottom;
    public NumericProperty paddingX;
    public NumericProperty paddingY;
    public NumericProperty radiusTopLeft;
    public NumericProperty radiusTopRight;
    public NumericProperty radiusBottomLeft;
    public NumericProperty radiusBottomRight;
    public NumericProperty radius
    {
        set => radiusTopLeft = radiusTopRight = radiusBottomLeft = radiusBottomRight = value;
    }
    public NumericProperty gap;
    public NumericProperty outlineWidth;
    public NumericProperty fontSize;

    public DirectionProperty flowDirection;
    public FitProperty marginFit;
    public FitProperty contentFitX;
    public FitProperty contentFitY;
    public AlignmentProperty alignSelfX;
    public AlignmentProperty alignSelfY;
    public PositionStyleProperty positionStyle;

    public ColorProperty fillColor;
    public ColorProperty outlineColor;

    public BoolProperty debugBox;
    public BoolProperty ignorePointerEvents;
    public BoolProperty visible;

    public Style(int importance)
    {
        this.importance = importance;
        InitProperties();
    }

    public Style()
    {
        this.importance = 0;
        InitProperties();
    }

    public Style(Style other)
    {
        this.importance = other.importance;
        InitProperties();
        width = other.width;
        height = other.height;
        minWidth = other.minWidth;
        minHeight = other.minHeight;
        maxWidth = other.maxWidth;
        maxHeight = other.maxHeight;
        left = other.left;
        right = other.right;
        top = other.top;
        bottom = other.bottom;
        marginLeft = other.marginLeft;
        marginRight = other.marginRight;
        marginTop = other.marginTop;
        marginBottom = other.marginBottom;
        paddingX = other.paddingX;
        paddingY = other.paddingY;
        radiusTopLeft = other.radiusTopLeft;
        radiusTopRight = other.radiusTopRight;
        radiusBottomLeft = other.radiusBottomLeft;
        radiusBottomRight = other.radiusBottomRight;
        gap = other.gap;
        outlineWidth = other.outlineWidth;
        fontSize = other.fontSize;
        flowDirection = other.flowDirection;
        marginFit = other.marginFit;
        contentFitX = other.contentFitX;
        contentFitY = other.contentFitY;
        alignSelfX = other.alignSelfX;
        alignSelfY = other.alignSelfY;
        positionStyle = other.positionStyle;
        fillColor = other.fillColor;
        outlineColor = other.outlineColor;
        debugBox = other.debugBox;
        ignorePointerEvents = other.ignorePointerEvents;
        visible = other.visible;
    }

    public void InitProperties()
    {
        width = new();
        height = new();
        minWidth = new();
        minHeight = new();
        maxWidth = new();
        maxHeight = new();
        left = new();
        right = new();
        top = new();
        bottom = new();
        marginLeft = new();
        marginRight = new();
        marginTop = new();
        marginBottom = new();
        paddingX = new();
        paddingY = new();
        radiusTopLeft = new();
        radiusTopRight = new();
        radiusBottomLeft = new();
        radiusBottomRight = new();
        gap = new();
        outlineWidth = new();
        fontSize = new();
        flowDirection = new();
        marginFit = new();
        contentFitX = new();
        contentFitY = new();
        alignSelfX = new();
        alignSelfY = new();
        positionStyle = new();
        fillColor = new();
        outlineColor = new();
        debugBox = new();
        ignorePointerEvents = new();
        visible = new();
    }

    public Style TryOverride(Style other)
    {
        var result = Clone();
        result.width = width.TryOverride(other.width);
        result.height = height.TryOverride(other.height);
        result.minWidth = minWidth.TryOverride(other.minWidth);
        result.minHeight = minHeight.TryOverride(other.minHeight);
        result.maxWidth = maxWidth.TryOverride(other.maxWidth);
        result.maxHeight = maxHeight.TryOverride(other.maxHeight);
        result.left = left.TryOverride(other.left);
        result.right = right.TryOverride(other.right);
        result.top = top.TryOverride(other.top);
        result.bottom = bottom.TryOverride(other.bottom);
        result.marginLeft = marginLeft.TryOverride(other.marginLeft);
        result.marginRight = marginRight.TryOverride(other.marginRight);
        result.marginTop = marginTop.TryOverride(other.marginTop);
        result.marginBottom = marginBottom.TryOverride(other.marginBottom);
        result.paddingX = paddingX.TryOverride(other.paddingX);
        result.paddingY = paddingY.TryOverride(other.paddingY);
        result.radiusTopLeft = radiusTopLeft.TryOverride(other.radiusTopLeft);
        result.radiusTopRight = radiusTopRight.TryOverride(other.radiusTopRight);
        result.radiusBottomLeft = radiusBottomLeft.TryOverride(other.radiusBottomLeft);
        result.radiusBottomRight = radiusBottomRight.TryOverride(other.radiusBottomRight);
        result.gap = gap.TryOverride(other.gap);
        result.outlineWidth = outlineWidth.TryOverride(other.outlineWidth);
        result.fontSize = fontSize.TryOverride(other.fontSize);
        result.flowDirection = flowDirection.TryOverride(other.flowDirection);
        result.marginFit = marginFit.TryOverride(other.marginFit);
        result.contentFitX = contentFitX.TryOverride(other.contentFitX);
        result.contentFitY = contentFitY.TryOverride(other.contentFitY);
        result.alignSelfX = alignSelfX.TryOverride(other.alignSelfX);
        result.alignSelfY = alignSelfY.TryOverride(other.alignSelfY);
        result.positionStyle = positionStyle.TryOverride(other.positionStyle);
        result.fillColor = fillColor.TryOverride(other.fillColor);
        result.outlineColor = outlineColor.TryOverride(other.outlineColor);
        result.debugBox = debugBox.TryOverride(other.debugBox);
        result.ignorePointerEvents = ignorePointerEvents.TryOverride(other.ignorePointerEvents);
        result.visible = visible.TryOverride(other.visible);
        return result;
    }

    public Style Clone()
    {
        return (Style)this.MemberwiseClone();
    }
}

public class ComputedStyle
{
    public Style definition;
    public Element el;

    public NumericProperty width;
    public NumericProperty height;
    public NumericProperty minWidth;
    public NumericProperty minHeight;
    public NumericProperty maxWidth;
    public NumericProperty maxHeight;
    public NumericProperty left;
    public NumericProperty right;
    public NumericProperty top;
    public NumericProperty bottom;
    public NumericProperty marginLeft;
    public NumericProperty marginRight;
    public NumericProperty marginTop;
    public NumericProperty marginBottom;
    public NumericProperty paddingX;
    public NumericProperty paddingY;
    public NumericProperty radiusTopLeft;
    public NumericProperty radiusTopRight;
    public NumericProperty radiusBottomLeft;
    public NumericProperty radiusBottomRight;
    public NumericProperty gap;
    public NumericProperty outlineWidth;
    public NumericProperty fontSize;
    public DirectionProperty flowDirection;
    public FitProperty marginFit;
    public FitProperty contentFitX;
    public FitProperty contentFitY;
    public AlignmentProperty alignSelfX;
    public AlignmentProperty alignSelfY;
    public PositionStyleProperty positionStyle;
    public ColorProperty fillColor;
    public ColorProperty outlineColor;
    public BoolProperty debugBox;
    public BoolProperty ignorePointerEvents;
    public BoolProperty visible;

    public ComputedStyle(Style style, Element applyTo)
    {
        this.definition = style;
        this.el = applyTo;
        InitStyle(applyTo);
    }

    public void InitStyle(Element el)
    {
        width = definition.width.InitWithDefault(el, () => 0);
        height = definition.height.InitWithDefault(el, () => 0);
        minWidth = definition.minWidth.InitWithDefault(el, () => 0);
        minHeight = definition.minHeight.InitWithDefault(el, () => 0);
        maxWidth = definition.maxWidth.InitWithDefault(el, () => el.Parent is not null && !el.Parent.ComputedStyle.width.IsUnsetOrAuto ? el.Parent?.PaddedBounds.Width ?? float.MaxValue : float.MaxValue);
        maxHeight = definition.maxHeight.InitWithDefault(el, () => el.Parent is not null && !el.Parent.ComputedStyle.height.IsUnsetOrAuto ? el.Parent?.PaddedBounds.Height ?? float.MaxValue : float.MaxValue);
        left = definition.left.InitWithDefault(el, () => 0);
        right = definition.right.InitWithDefault(el, () => 0);
        top = definition.top.InitWithDefault(el, () => 0);
        bottom = definition.bottom.InitWithDefault(el, () => 0);
        marginLeft = definition.marginLeft.InitWithDefault(el, () => 0);
        marginRight = definition.marginRight.InitWithDefault(el, () => 0);
        marginTop = definition.marginTop.InitWithDefault(el, () => 0);
        marginBottom = definition.marginBottom.InitWithDefault(el, () => 0);
        paddingX = definition.paddingX.InitWithDefault(el, () => 0);
        paddingY = definition.paddingY.InitWithDefault(el, () => 0);
        radiusTopLeft = definition.radiusTopLeft.InitWithDefault(el, () => 0);
        radiusTopRight = definition.radiusTopRight.InitWithDefault(el, () => 0);
        radiusBottomLeft = definition.radiusBottomLeft.InitWithDefault(el, () => 0);
        radiusBottomRight = definition.radiusBottomRight.InitWithDefault(el, () => 0);
        gap = definition.gap.InitWithDefault(el, () => 0);
        outlineWidth = definition.outlineWidth.InitWithDefault(el, () => 0);
        
        fontSize = definition.fontSize.InitWithDefault(el, () => 16);
        if (fontSize.IsUnsetOrAuto) fontSize = el.Parent?.ComputedStyle.fontSize ?? 16;

        flowDirection = definition.flowDirection.InitWithDefault(el, () => Direction.Vertical);
        marginFit = definition.marginFit.InitWithDefault(el, () => Fit.Fit);
        contentFitX = definition.contentFitX.InitWithDefault(el, () => Fit.Ignore);
        contentFitY = definition.contentFitY.InitWithDefault(el, () => Fit.Ignore);
        alignSelfX = definition.alignSelfX.InitWithDefault(el, () => Alignment.Start);
        alignSelfY = definition.alignSelfY.InitWithDefault(el, () => Alignment.Start);
        positionStyle = definition.positionStyle.InitWithDefault(el, () => PositionStyle.Relative);
        fillColor = definition.fillColor.InitWithDefault(el, () => Color.Transparent);
        outlineColor = definition.outlineColor.InitWithDefault(el, () => Color.Transparent);
        debugBox = definition.debugBox.InitWithDefault(el, () => false);
        ignorePointerEvents = definition.ignorePointerEvents.InitWithDefault(el, () => false);
        visible = definition.visible.InitWithDefault(el, () => true);
    }
}
