namespace ProtoEngine.UI;

public class LabeledElement : Element
{
    public TextElement Label { get; set;}
    public Element Content { get; set;}

    public LabeledElement(Element parent, string labelText, Element content) : base(parent)
    {
        Content = content;

        Style.flowDirection = Direction.Horizontal;
        Style.contentFitY = Fit.Fit;
        Style.gap = "1em";

        Label = new TextElement(this, labelText);
        Label.Style.alignSelfX = Alignment.Start;
        Label.Style.alignSelfY = Alignment.Center;

        Content.Style.alignSelfX = Alignment.End;
        Content.Style.alignSelfY = Alignment.Center;
        Content.Parent = this;
    }
}