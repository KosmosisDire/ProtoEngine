using ProtoEngine.UI.Internal;

namespace ProtoEngine.UI;

public abstract class Input : Element
{
    public static Input? FocusedInput { get; protected set; }
    public bool IsFocused { get; protected set; }
    public Input(Element parent) : base(parent) {}
    public Input(Element parent, Style baseStyle) : base(parent, baseStyle) {}
    public Input(Style baseStyle) : base(baseStyle) {}
    public Input() {}
}

public abstract class Input<T> : Input
{
    public InputEvents<T> inputEvents = new();

    public Input(Element parent) : base(parent) {Init();}
    public Input(Element parent, Style baseStyle) : base(parent, baseStyle) {Init();}
    public Input(Style baseStyle) : base(baseStyle) {Init();}
    public Input() {Init();}

    protected virtual void Init()
    {
        events.OnDefocus += () => 
        {
            IsFocused = false;
            if (FocusedInput == this) FocusedInput = null;
        };

        events.OnFocus += () =>
        {
            IsFocused = true;
            FocusedInput = this;
        };
    }
}