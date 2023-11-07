using SFML.Graphics;
using SFML.Window;
using Window = ProtoEngine.Rendering.Window;

namespace ProtoEngine.UI3;

public class TextInput : Input<string>
{
    public TextElement textElement;
    private Text measureText;
    private RectangleShape cursor;
    private string _value = "";
    public string Value
    {
        get => _value;
        set => Set(value);
    }

    public Style placeholderStyle = new();
    private int cursorPosition = 0;

    private string _placeholderText = "";
    public string PlaceholderText
    {
        get => _placeholderText;
        set
        {
            _placeholderText = value;
            if (value.Length == 0)
            {
                textElement.AddStyle(placeholderStyle);
                textElement.text.Value = PlaceholderText;
            }
            else
            {
                textElement.text.Value = value;
                textElement.RemoveStyle(placeholderStyle);
            }
        }
    }

    public TextInput(Element parent) : base(parent) {}

    public TextInput(Element parent, Style baseStyle) : base(parent, baseStyle) {}

    public TextInput(Style baseStyle) : base(baseStyle) {}

    public TextInput() : base() {}


    protected override void Init()
    {
        base.Init();

        measureText = new();
        textElement = new(this, "");
        textElement.Style.ignorePointerEvents = true;
        textElement.Style.alignSelfY = Alignment.Center;
        DefaultStyle.height = new Em(2f);
        DefaultStyle.paddingX = new Em(1f);
        cursor = new();

        placeholderStyle = new Style
        {
            fillColor = new Color(100, 100, 100),
        };

        events.OnMouseButtonReleased += (MouseButtonEventArgs button, Window window) =>
        {
            if (button.Button == Mouse.Button.Left)
            {
                events.OnFocus.Invoke();
            }
        };

        events.OnKeyPressed += (KeyEventArgs key, Window window) =>
        {
            if (IsFocused)
            {
                if (key.Code == Keyboard.Key.Backspace)
                {
                    if (_value.Length > 0 && cursorPosition > 0)
                    {
                        cursorPosition--;
                        RemoveAt(cursorPosition);
                    }
                }
                else if (key.Code == Keyboard.Key.Delete)
                {
                    if (_value.Length > 0 && cursorPosition < _value.Length)
                    {
                        RemoveAt(cursorPosition);
                    }
                }
                else if (key.Code == Keyboard.Key.Left)
                {
                    if (cursorPosition > 0)
                    {
                        cursorPosition--;
                    }
                }
                else if (key.Code == Keyboard.Key.Right)
                {
                    if (cursorPosition < _value.Length)
                    {
                        cursorPosition++;
                    }
                }
                else if (key.Code == Keyboard.Key.Enter || key.Code == Keyboard.Key.Escape)
                {
                    IsFocused = false;
                    inputEvents.Submit.Invoke(_value);
                }
                else if (!key.Control && !key.Alt && !key.System)
                {
                    if (key.Shift)
                    {
                        Insert(cursorPosition, KeyWithShiftToChar(key.Code));
                    }
                    else
                    {
                        Insert(cursorPosition, KeyToChar(key.Code));
                    }

                    cursorPosition++;
                }
            }
        };

        inputEvents.OnChange += (value) =>
        {
            textElement.text.Value = value;
            textElement.textShape.DisplayedString = value;
        };

        inputEvents.Submit += (value) => events.OnDefocus.Invoke();

        events.OnFocus += () =>
        {
            if (Value.Length == 0) textElement.text.Value = "";
        };

        events.OnDefocus += () =>
        {
            if (Value.Length == 0) textElement.text.Value = PlaceholderText;
        };
    }

    public override void BuildBox()
    {
        base.BuildBox();
        if (textElement != null && cursor != null)
        {
            var clampedCursorPosition = Math.Clamp(cursorPosition, 0, Value.Length);
            measureText.CharacterSize = textElement.textShape.CharacterSize;
            measureText.Font = textElement.textShape.Font;
            measureText.DisplayedString = Value[..clampedCursorPosition];
            var left = textElement.InnerLeft + measureText.GetLocalBounds().Width;

            cursor.Position = new(left, (InnerTop + InnerBottom) / 2f - textElement.textShape.CharacterSize / 2f);
            cursor.FillColor = textElement.ComputedStyle.fillColor;
            cursor.Size = new(2, textElement.textShape.CharacterSize);
        }
    }

    public override void Draw(RenderTarget target, RenderStates states)
    {
        base.Draw(target, states);

        if (IsFocused && DateTime.Now.Millisecond < 500)
        {
            target.Draw(cursor);
        }
    }

    public void Set(string value)
    {
        _value = value;
        inputEvents.OnChange?.Invoke(_value);
    }

    public void Append(string value)
    {
        Set(_value + value);
    }

    public void Insert(int index, string value)
    {
        var clampedIndex = Math.Clamp(index, 0, _value.Length);
        Set(_value.Insert(clampedIndex, value));
    }

    public void RemoveAt(int index)
    {
        var clampedIndex = Math.Clamp(index, 0, _value.Length - 1);
        Set(_value.Remove(clampedIndex, 1));
    }

    public static string KeyToChar(Keyboard.Key key)
    {
        return key switch
        {
            Keyboard.Key.A => "a",
            Keyboard.Key.B => "b",
            Keyboard.Key.C => "c",
            Keyboard.Key.D => "d",
            Keyboard.Key.E => "e",
            Keyboard.Key.F => "f",
            Keyboard.Key.G => "g",
            Keyboard.Key.H => "h",
            Keyboard.Key.I => "i",
            Keyboard.Key.J => "j",
            Keyboard.Key.K => "k",
            Keyboard.Key.L => "l",
            Keyboard.Key.M => "m",
            Keyboard.Key.N => "n",
            Keyboard.Key.O => "o",
            Keyboard.Key.P => "p",
            Keyboard.Key.Q => "q",
            Keyboard.Key.R => "r",
            Keyboard.Key.S => "s",
            Keyboard.Key.T => "t",
            Keyboard.Key.U => "u",
            Keyboard.Key.V => "v",
            Keyboard.Key.W => "w",
            Keyboard.Key.X => "x",
            Keyboard.Key.Y => "y",
            Keyboard.Key.Z => "z",
            Keyboard.Key.Num0 => "0",
            Keyboard.Key.Num1 => "1",
            Keyboard.Key.Num2 => "2",
            Keyboard.Key.Num3 => "3",
            Keyboard.Key.Num4 => "4",
            Keyboard.Key.Num5 => "5",
            Keyboard.Key.Num6 => "6",
            Keyboard.Key.Num7 => "7",
            Keyboard.Key.Num8 => "8",
            Keyboard.Key.Num9 => "9",
            Keyboard.Key.LBracket => "[",
            Keyboard.Key.RBracket => "]",
            Keyboard.Key.Semicolon => ";",
            Keyboard.Key.Comma => ",",
            Keyboard.Key.Period => ".",
            Keyboard.Key.Quote => "'",
            Keyboard.Key.Slash => "/",
            Keyboard.Key.Backslash => "\\",
            Keyboard.Key.Tilde => "`",
            Keyboard.Key.Equal => "=",
            Keyboard.Key.Hyphen => "-",
            Keyboard.Key.Space => " ",
            Keyboard.Key.Enter => "\n",
            Keyboard.Key.Tab => "\t",
            Keyboard.Key.Add => "+",
            Keyboard.Key.Subtract => "-",
            Keyboard.Key.Multiply => "*",
            Keyboard.Key.Divide => "/",
            Keyboard.Key.Numpad0 => "0",
            Keyboard.Key.Numpad1 => "1",
            Keyboard.Key.Numpad2 => "2",
            Keyboard.Key.Numpad3 => "3",
            Keyboard.Key.Numpad4 => "4",
            Keyboard.Key.Numpad5 => "5",
            Keyboard.Key.Numpad6 => "6",
            Keyboard.Key.Numpad7 => "7",
            Keyboard.Key.Numpad8 => "8",
            Keyboard.Key.Numpad9 => "9",
            _ => ""
        };
    }

    public static string KeyWithShiftToChar(Keyboard.Key key)
    {
        return key switch
        {
            Keyboard.Key.A => "A",
            Keyboard.Key.B => "B",
            Keyboard.Key.C => "C",
            Keyboard.Key.D => "D",
            Keyboard.Key.E => "E",
            Keyboard.Key.F => "F",
            Keyboard.Key.G => "G",
            Keyboard.Key.H => "H",
            Keyboard.Key.I => "I",
            Keyboard.Key.J => "J",
            Keyboard.Key.K => "K",
            Keyboard.Key.L => "L",
            Keyboard.Key.M => "M",
            Keyboard.Key.N => "N",
            Keyboard.Key.O => "O",
            Keyboard.Key.P => "P",
            Keyboard.Key.Q => "Q",
            Keyboard.Key.R => "R",
            Keyboard.Key.S => "S",
            Keyboard.Key.T => "T",
            Keyboard.Key.U => "U",
            Keyboard.Key.V => "V",
            Keyboard.Key.W => "W",
            Keyboard.Key.X => "X",
            Keyboard.Key.Y => "Y",
            Keyboard.Key.Z => "Z",
            Keyboard.Key.Num0 => ")",
            Keyboard.Key.Num1 => "!",
            Keyboard.Key.Num2 => "@",
            Keyboard.Key.Num3 => "#",
            Keyboard.Key.Num4 => "$",
            Keyboard.Key.Num5 => "%",
            Keyboard.Key.Num6 => "^",
            Keyboard.Key.Num7 => "&",
            Keyboard.Key.Num8 => "*",
            Keyboard.Key.Num9 => "(",
            Keyboard.Key.LBracket => "{",
            Keyboard.Key.RBracket => "}",
            Keyboard.Key.Semicolon => ":",
            Keyboard.Key.Comma => "<",
            Keyboard.Key.Period => ">",
            Keyboard.Key.Quote => "\"",
            Keyboard.Key.Slash => "?",
            Keyboard.Key.Backslash => "|",
            Keyboard.Key.Tilde => "~",
            Keyboard.Key.Equal => "+",
            Keyboard.Key.Hyphen => "_",
            Keyboard.Key.Space => " ",
            Keyboard.Key.Enter => "\r",
            Keyboard.Key.Add => "+",
            Keyboard.Key.Subtract => "-",
            Keyboard.Key.Multiply => "*",
            Keyboard.Key.Divide => "/",
            Keyboard.Key.Numpad0 => "0",
            Keyboard.Key.Numpad1 => "1",
            Keyboard.Key.Numpad2 => "2",
            Keyboard.Key.Numpad3 => "3",
            Keyboard.Key.Numpad4 => "4",
            Keyboard.Key.Numpad5 => "5",
            Keyboard.Key.Numpad6 => "6",
            Keyboard.Key.Numpad7 => "7",
            Keyboard.Key.Numpad8 => "8",
            Keyboard.Key.Numpad9 => "9",
            _ => ""
        };
    }

}