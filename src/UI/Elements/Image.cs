using ProtoEngine.UI;
using SFML.Graphics;

namespace ProtoEngine;

public class Image : Element
{
    public Sprite sprite;
    public Color color = Color.White;

    public bool SmoothScaling
    {
        get => sprite.Texture.Smooth;
        set => sprite.Texture.Smooth = value;
    }

    public Image(Element parent, Texture texture) : base(parent)
    {
        sprite = new Sprite(texture);
    }

    public Image(Texture texture) : base()
    {
        sprite = new Sprite(texture);
    }

    public Image(Element parent, byte[] imageData) : base(parent)
    {
        sprite = new Sprite(new Texture(imageData));
    }

    public Image(byte[] imageData) : base()
    {
        sprite = new Sprite(new Texture(imageData));
    }

    public override void Draw(RenderTarget target, RenderStates states)
    {
        sprite.Position = Bounds.TopLeft;
        sprite.Color = color;
        sprite.Scale = Bounds.size / sprite.Texture.Size;
        if (!ComputedStyle.visible) return;
        target.Draw(sprite);
    }
}
