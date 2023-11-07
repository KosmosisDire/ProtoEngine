using ProtoEngine.Rendering;
using SFML.Graphics;
namespace ProtoEngine.UI3;

public class Button : Element
{
    public Button(Element parent, Style baseStyle, Action<Button> OnClick) : base(parent, baseStyle)
    {
        events.OnMouseButtonReleased += (SFML.Window.MouseButtonEventArgs e, Window window) =>
        {
            if (e.Button == SFML.Window.Mouse.Button.Left)
            {
                OnClick(this);
            }
        };

        Style.height = new Em(2);

        HoverStyle = new Style
        {
            fillColor = new (() => baseStyle.fillColor.Value.Lighten(0.95f)),
        };
        PressedStyle = new Style
        {
            fillColor = new (() => baseStyle.fillColor.Value.Darken(0.1f)),
        };
    }
}