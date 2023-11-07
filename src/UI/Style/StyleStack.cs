using System.Runtime.Serialization;
using ProtoEngine.UI;

public class StyleStack
{
    private readonly Element element;
    public List<Style> styles = new();
    public Style defaultStyle = new(0);
    public Style baseStyle = new(1);
    public Style uniqueStyle = new(2);

    public StyleStack(Element element)
    {
        this.element = element;
    }

    public void AddStyle(Style style)
    {
        styles.Add(style);
        styles.Sort((a, b) => a.importance.CompareTo(b.importance));
    }

    public void RemoveStyle(Style style)
    {
        styles.Remove(style);
    }

    public bool Contains(Style style)
    {
        return styles.Contains(style);
    }

    public ComputedStyle GetStyle()
    {
        var styleDefinition = defaultStyle;
        styleDefinition = styleDefinition.TryOverride(baseStyle);
        styleDefinition = styleDefinition.TryOverride(uniqueStyle);

        for (int i = 0; i < styles.Count; i++)
        {
            styleDefinition = styleDefinition.TryOverride(styles[i]);
        }
        
        return new ComputedStyle(styleDefinition, element);
    }
}