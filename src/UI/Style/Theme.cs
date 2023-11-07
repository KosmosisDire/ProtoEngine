using SFML.Graphics;
namespace ProtoEngine.UI;

public struct Theme
{
    public static Theme GlobalTheme;
    public static Font MainFont;

    static Theme()
    {
        MainFont = new Font(Properties.Resources.MPLUSRounded1c_Regular);
        GlobalTheme = new Theme();
    }

    public Font mainFont;
    public ColorProperty background;
    public ColorProperty surface1;
    public ColorProperty surface2;
    public ColorProperty surface3;
    public ColorProperty surface1Outline;
    public ColorProperty surface2Outline;
    public ColorProperty surface3Outline;
    public ColorProperty accent;
    public ColorProperty accentMuted;
    public ColorProperty controlFill;
    public ColorProperty controlFillMuted;
    public ColorProperty controlFillOutline;
    public ColorProperty controlSurface;
    public ColorProperty textColor;
    public ColorProperty textColorMuted;
    public NumericProperty controlRadius;

    public Theme()
    {
        mainFont = MainFont;
        background = new Color(20, 20, 20);
        surface1 = new Color(40, 40, 40);
        surface2 = new Color(60, 60, 60);
        surface3 = new Color(80, 80, 80);
        surface1Outline = new Color(55, 55, 55);
        surface2Outline = new Color(75, 75, 75);
        surface3Outline = new Color(95, 95, 95);
        accent = new Color(0, 122, 204);
        accentMuted = new Color(58, 111, 134, 200);
        controlFill = new Color(230, 230, 230);
        controlFillMuted = new Color(230, 230, 230, 200);
        controlFillOutline = new Color(255, 255, 255);
        controlSurface = new Color(58, 111, 134, 200);
        textColor = new Color(230, 230, 230);
        textColorMuted = new Color(230, 230, 230, 200);
        controlRadius = "0.5em";
    }

    public static void ApplyThemeOverride(Theme theme)
    {
        GlobalTheme.background = GlobalTheme.background.TryOverride(theme.background);
        GlobalTheme.surface1 = GlobalTheme.surface1.TryOverride(theme.surface1);
        GlobalTheme.surface2 = GlobalTheme.surface2.TryOverride(theme.surface2);
        GlobalTheme.surface3 = GlobalTheme.surface3.TryOverride(theme.surface3);
        GlobalTheme.surface1Outline = GlobalTheme.surface1Outline.TryOverride(theme.surface1Outline);
        GlobalTheme.surface2Outline = GlobalTheme.surface2Outline.TryOverride(theme.surface2Outline);
        GlobalTheme.surface3Outline = GlobalTheme.surface3Outline.TryOverride(theme.surface3Outline);
        GlobalTheme.accent = GlobalTheme.accent.TryOverride(theme.accent);
        GlobalTheme.accentMuted = GlobalTheme.accentMuted.TryOverride(theme.accentMuted);
        GlobalTheme.controlFill = GlobalTheme.controlFill.TryOverride(theme.controlFill);
        GlobalTheme.controlFillMuted = GlobalTheme.controlFillMuted.TryOverride(theme.controlFillMuted);
        GlobalTheme.controlFillOutline = GlobalTheme.controlFillOutline.TryOverride(theme.controlFillOutline);
        GlobalTheme.controlSurface = GlobalTheme.controlSurface.TryOverride(theme.controlSurface);
        GlobalTheme.textColor = GlobalTheme.textColor.TryOverride(theme.textColor);
        GlobalTheme.textColorMuted = GlobalTheme.textColorMuted.TryOverride(theme.textColorMuted);
        GlobalTheme.controlRadius = GlobalTheme.controlRadius.TryOverride(theme.controlRadius);
    }
}