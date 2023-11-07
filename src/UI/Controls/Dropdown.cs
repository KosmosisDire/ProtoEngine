using SFML.Graphics;
using SFML.Window;

namespace ProtoEngine.UI;

public class Dropdown : UpdatableControl<int>
{
    readonly RoundedRectangle background = new();
    readonly RectangleShape clickBox = new();
    public bool IsOpen { get; protected set; } = false;

    public List<RectangleShape> optionShapes = new();
    public List<Text> optionTexts = new();
    public List<string> options = new();

    public string SelectedOption => options[Value];
    public int capturedOption = -1;

    public Dropdown(string label, IEnumerable<string> options, Panel panel, OnChanged? onChanged, int defaultIndex) : base(label, panel, onChanged, defaultIndex)
    {
        drawLabel = false;
        drawValue = false;
        this.options = options.ToList();
        optionTexts = options.Select(s => new Text(s)).ToList();
        optionShapes = optionTexts.Select(t => new RectangleShape()).ToList();
    }

    public static Dropdown FromEnum<T>(string label, Panel panel, OnChanged? onChanged, int defaultIndex) where T : Enum
    {
        return new Dropdown(label, Enum.GetNames(typeof(T)), panel, onChanged, defaultIndex);
    }

    public Enum AsEnum<T>() where T : Enum
    {
        return (T)Enum.Parse(typeof(T), SelectedOption);
    }

    protected override void Update()
    {
        zIndex = IsOpen ? 100 : 0;

        if (IsOpen)
        {
            for (int i = 0; i < optionShapes.Count; i++)
            {
                if (!isMouseCaptured && optionShapes[i].IsBeingDragged(Mouse.Button.Left, window))
                {
                    isMouseCaptured = true;
                    capturedOption = i;
                }
                else
                {
                    if (isMouseCaptured && capturedOption != -1)
                    {
                        Value = capturedOption;
                    }

                    isMouseCaptured = false;
                    IsOpen = false;
                }
            }
        }

        if (!isMouseCaptured && clickBox.IsBeingDragged(Mouse.Button.Left, window))
        {
            isMouseCaptured = true;
        }
        else
        {
            if (isMouseCaptured)
            {
                IsOpen = !IsOpen;
            }

            isMouseCaptured = false;
        }
    }

    public override void Draw(float y)
    {
        Update();

        base.Draw(y);

        clickBox.Size = OuterBounds.size;
        clickBox.Position = OuterBounds.position;

        background.Size = OuterBounds.size;
        background.Position = OuterBounds.position;
        background.FillColor = style.accentColor.Darken((isMouseCaptured || IsOpen) ? 0.2f : 0.0f);
        background.OutlineColor = Theme.strokeColor;
        background.OutlineThickness = style.OutlineThickness;
        background.Radius = style.CornerRadius;
        window.Draw(background);

        valueText.ApplyStyle(style).SetFontStyle(SFML.Graphics.Text.Styles.Bold).Center(OuterBounds, CenterAxis.Both).Draw(window);

        if (IsOpen)
        {
            for (int i = 0; i < optionShapes.Count; i++)
            {
                var shape = optionShapes[i];
                shape.Size = new Vector2(OuterBounds.Width, OuterBounds.Height);
                shape.Position = new Vector2(OuterBounds.Left, OuterBounds.Bottom + OuterBounds.Height * i);
                shape.FillColor = style.accentColor.Darken(isMouseCaptured ? 0.2f : 0.0f);
                shape.OutlineColor = Theme.strokeColor;
                shape.OutlineThickness = style.OutlineThickness;
                window.Draw(shape);

                optionTexts[i].SetFontStyle(SFML.Graphics.Text.Styles.Bold).ApplyStyle(style).Center(new(shape.GetGlobalBounds()), CenterAxis.Both).Draw(window);
            }
        }

    }
}
