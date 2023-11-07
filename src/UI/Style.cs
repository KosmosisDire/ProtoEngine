using SFML.Graphics;

namespace ProtoEngine.UI;

public struct Style
{
    // colors
    public static Color defaultBackgroundColor = new(35, 39, 46);
    public Color backgroundColor = defaultBackgroundColor;
    public static Color defaultAccentColor = new(28, 135, 99);
    public Color accentColor = defaultAccentColor;
    public static Color defaultTextColor = new(250, 250, 250);
    public Color textColor = defaultTextColor;

    // numerical values
    public static float DefaultScale => GUIManager.globalTheme.scale;
    public float scale = DefaultScale;
    public static float defaultCornerRadius = 5;
    private float _cornerRadius = defaultCornerRadius;
    public float CornerRadius { get => _cornerRadius * scale; set => _cornerRadius = value; }
    public static uint defaultOutlineThickness = 1;
    private uint _outlineThickness = defaultOutlineThickness;
    public uint OutlineThickness { get => (uint)(_outlineThickness * scale); set => _outlineThickness = value; }
    public static uint defaultFontSize = 12;
    private uint _fontSize = defaultFontSize;
    public uint FontSize { get => (uint)(_fontSize * scale); set => _fontSize = value; }


    // boolean values
    public bool visible = true;
    public bool enabled = true;

    // object references
    public static Font defaultFont = new ("C:/Main Documents/Projects/Coding/C#/ProtoEngine/src/Resources/MPLUSRounded1c-Regular.ttf");
    public Font font = defaultFont;

    public static Style defaultStyle;
    static Style()
    {
        defaultStyle = new();
    }

    public Style()
    {

    }
    
}