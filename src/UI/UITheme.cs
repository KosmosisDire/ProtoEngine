using SFML.Graphics;

namespace ProtoEngine.UI;

public struct UITheme
{
    public Color backgroundColor = new(35, 39, 46);
    public Color barColor = new(30, 34, 39);
    public Color fillColor = new(93, 103, 122);
    public Color strokeColor = new(37, 84, 91);
    public Color textColor = new(250, 250, 250);
    public Color textBackgroundColor = new(144, 96, 169);
    public Color accentColor = new(38, 163, 121);
    public Color xAxisColor = new(224, 108, 117);
    public Color yAxisColor = new(108, 224, 117);

    public Font font;
    public float scale = 1;
    private uint _fontSize = 12;
    public uint FontSize { get => (uint)(_fontSize * scale); set => _fontSize = value; }
    private uint _lineHeight = 32;
    public uint LineHeight { get => (uint)(_lineHeight * scale); set => _lineHeight = value; }
    private float _margin = 5;
    public float Margin { get => _margin * scale; set => _margin = value; }
    private float _nobSize = 5;
    public float NobSize { get => _nobSize * scale; set => _nobSize = value; }
    public float LineThickness = 1;
    public float OutlineThickness = 1;


    public UITheme()
    {
        font = new Font("C:/Main Documents/Projects/Coding/C#/ProtoEngine/src/Resources/MPLUSRounded1c-Regular.ttf");
    }

    public UITheme(UITheme theme)
    {
        backgroundColor = theme.backgroundColor;
        barColor = theme.barColor;
        fillColor = theme.fillColor;
        strokeColor = theme.strokeColor;
        textColor = theme.textColor;
        textBackgroundColor = theme.textBackgroundColor;
        accentColor = theme.accentColor;
        xAxisColor = theme.xAxisColor;
        yAxisColor = theme.yAxisColor;

        font = theme.font;
        FontSize = theme.FontSize;
        LineHeight = theme.LineHeight;
        Margin = theme.Margin;
        NobSize = theme.NobSize;
        LineThickness = theme.LineThickness;
        OutlineThickness = theme.OutlineThickness;
    }


    public UITheme SetBackgroundColor(Color color)
    {
        return new UITheme(this) { backgroundColor = color };
    }

    public UITheme SetBarColor(Color color)
    {
        return new UITheme(this) { barColor = color };
    }

    public UITheme SetFillColor(Color color)
    {
        return new UITheme(this) { fillColor = color };
    }

    public UITheme SetStrokeColor(Color color)
    {
        return new UITheme(this) { strokeColor = color };
    }

    public UITheme SetTextColor(Color color)
    {
        return new UITheme(this) { textColor = color };
    }

    public UITheme SetTextBackgroundColor(Color color)
    {
        return new UITheme(this) { textBackgroundColor = color };
    }

    public UITheme SetAccentColor(Color color)
    {
        return new UITheme(this) { accentColor = color };
    }

    public UITheme SetXAxisColor(Color color)
    {
        return new UITheme(this) { xAxisColor = color };
    }

    public UITheme SetYAxisColor(Color color)
    {
        return new UITheme(this) { yAxisColor = color };
    }

    public UITheme SetFont(Font font)
    {
        return new UITheme(this) { font = font };
    }

    public UITheme SetFontSize(uint fontSize)
    {
        return new UITheme(this) { FontSize = fontSize };
    }

    public UITheme SetLineHeight(uint lineHeight)
    {
        return new UITheme(this) { LineHeight = lineHeight };
    }

    public UITheme SetMargin(float margin)
    {
        return new UITheme(this) { Margin = margin };
    }

    public UITheme SetNobSize(float nobSize)
    {
        return new UITheme(this) { NobSize = nobSize };
    }

    public UITheme SetLineThickness(float lineThickness)
    {
        return new UITheme(this) { LineThickness = lineThickness };
    }

    public UITheme SetOutlineThickness(float outlineThickness)
    {
        return new UITheme(this) { OutlineThickness = outlineThickness };
    }

    
}