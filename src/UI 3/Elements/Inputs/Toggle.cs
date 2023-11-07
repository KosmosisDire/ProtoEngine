

namespace ProtoEngine.UI3;
using SFML.Graphics;

public class Toggle : Input<bool>
{
    private bool _value;
    public bool Value
    {
        get => _value;
        set => Set(value);
    }

    public Element toggleHandle;

    private Style? _enabledStyle;
    public Style? EnabledStyle
    {
        get => _enabledStyle;
        set
        {
            if (value == null) return;
            _enabledStyle = value;

            void SetEnabledStyle(bool changeTo)
            {
                if (changeTo)
                {
                    AddStyle(value.Value);
                }
                else
                {
                    RemoveStyle(value.Value);
                }
            }
            
            inputEvents.OnChange += SetEnabledStyle;
            SetEnabledStyle(Value);
        }
    }

    private Style? _enabledStyleHandle;
    public Style? EnabledStyleHandle
    {
        get => _enabledStyleHandle ?? EnabledStyle ?? new Style();
        set
        {
            if (value == null) return;
            _enabledStyleHandle = value;

            void SetEnabledStyle(bool changeTo)
            {
                if (changeTo)
                {
                    toggleHandle.AddStyle(value.Value);
                }
                else
                {
                    toggleHandle.RemoveStyle(value.Value);
                }
            }
            
            inputEvents.OnChange += SetEnabledStyle;
            SetEnabledStyle(Value);
        }
    }

    public Toggle(Element parent) : base(parent)
    {
        Init(false, null);
    }
    
    public Toggle(Element parent, Style style) : base(parent)
    {
        Init(false, style);
    }

    public Toggle(bool value, Style style) : base()
    {
        Init(value, style);
    }

    public Toggle(bool value) : base()
    {
        Init(value, null);
    }

    public Toggle(bool value, Element parent) : base(parent)
    {
        Init(value, null);
        _value = value;
    }

    public Toggle(bool value, Element parent, Style style) : base(parent)
    {
        Init(value, style);
        _value = value;
    }

    private Calc padding => Height * 0.1f;
    private float TranslationDist => Width - toggleHandle.Width - ComputedStyle.paddingX * 2 - padding;

    private void Init(bool value, Style? style = null)
    {
        if(style.HasValue) SetBaseStyle(style.Value);
        
        DefaultStyle.width = new Em(4);
        DefaultStyle.height = new Em(2);
        DefaultStyle.radius = Style.height / 2f;
        var defaultColor = new Color(255, 255, 255, 150);
        DefaultStyle.outlineColor = style.HasValue ? style.Value.outlineColor.IsUnset ? defaultColor : style.Value.outlineColor : defaultColor;
        DefaultStyle.outlineWidth = padding;
        DefaultStyle.marginLeft = padding;
        DefaultStyle.marginTop = padding;
        DefaultStyle.paddingX = 0;
        DefaultStyle.paddingY = 0;
        BuildBox();

        toggleHandle = new Element(this);
        toggleHandle.Style.fillColor = Color.White;
        toggleHandle.Style.width = Height - padding * 2;
        toggleHandle.Style.height = toggleHandle.Style.width;
        toggleHandle.Style.radius = toggleHandle.Style.width / 2f;
        toggleHandle.Style.alignSelfY = Alignment.Center;
        toggleHandle.BuildBox();
        toggleHandle.Style.left = value ? TranslationDist : padding;
    
        events.OnMouseButtonPressed += (e, win) =>
        {
            Set(!Value);
        };

        HoverStyle = new Style
        {
            outlineColor = new Color(255, 255, 255, 255),
        };

        EnabledStyle = new Style
        {
           fillColor = Color.White,
           outlineColor = new Color(255, 255, 255, 255),
        };
        EnabledStyleHandle = new Style
        {
           fillColor = Color.Black,
        };

        Set(value);
    }

    public override void BuildBox()
    {
        base.BuildBox();

        if (toggleHandle is not null) 
        {
            toggleHandle.BuildBox();
            var dist = Value ? TranslationDist : padding;
            if (!toggleHandle.Style.left.IsTweening) toggleHandle.Style.left.Value = dist;
        }
    }

    private void Set(bool value)
    {
        if (value == Value) return;

        _value = value;
        var dist = value ? TranslationDist : padding;
        toggleHandle.Style.left.Tween(dist, 0.1f);
        inputEvents.OnChange?.Invoke(value);
    }
}