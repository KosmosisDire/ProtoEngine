using SFML.Graphics;
using static ProtoEngine.UI.UpdatableControl<float>;

namespace ProtoEngine.UI;

public struct Series
{
    public delegate Vector2 PointRemapper(Vector2 point);
    public List<Vector2> points;
    public List<float> values;
    public Color color;
    public SetValue setValue;

    public float Min {get; private set;} = float.MaxValue;
    public float Max {get; private set;} = float.MinValue;

    public Series(Color color, SetValue setValue)
    {
        this.color = color;
        this.setValue = setValue;
        points = new();
        values = new();
    }

    public void Step(int currentStep)
    {
        var value = setValue.Invoke();
        var fraction = (Max - Min) * 0.005f;
        if (float.IsInfinity(fraction) || float.IsNaN(fraction)) fraction = 0;
        if (MathF.Abs(value - values.LastOrDefault()) > fraction)
        {
            AddValue(value, currentStep);
        }
    }

    public void AddValue(float value, int currentStep)
    {
        values.Add(value);
        points.Add(new Vector2(currentStep, value));
        if (value > Max) Max = value;
        if (value < Min) Min = value;
    }

    public readonly void Reset()
    {
        points.Clear();
        values.Clear();
    }

    public readonly VertexArray CreateVertexArray(int currentStep, PointRemapper pointRemapper)
    {
        var plot = new VertexArray(PrimitiveType.LineStrip);
        for (var i = 0; i < points.Count; i++)
        {
            plot.Append(new Vertex(pointRemapper(points[i]), color));
        }

        plot.Append(new Vertex(pointRemapper(new Vector2(currentStep, values.LastOrDefault())), color));
        return plot;
    }

    public readonly float GetCurrentValue()
    {
        return setValue.Invoke();
    }
}


public class Plot : UpdatableControl<float>
{

    public List<Series> series = new();
    protected float min;
    protected float max;
    protected readonly RectangleShape background = new();
    protected readonly RectangleShape xAxis = new();
    protected readonly RectangleShape yAxis = new();
    protected int currentStep = 0;
    protected bool started = false;

    public Plot(Panel panel, IEnumerable<Series> series) : base("label", panel, series.FirstOrDefault().setValue ?? (() => 0))
    {
        this.series = series.ToList();
        drawLabel = false;
        drawValue = false;
        Reset();
    }

    protected override void Update()
    {
        base.Update();

        if (!started || series.Count == 0) return;

        foreach (var set in series)
        {
            set.Step(currentStep);
            if (set.Max * 1.1f > max) max = set.Max * 1.1f;
            if (set.Min * 0.9f < min) min = set.Min * 0.9f;
        }

        currentStep++;
    }

    public void Reset()
    {
        series.ForEach(s => s.Reset());
        currentStep = 0;
        started = false;
        max = float.MinValue;
        min = float.MaxValue;
    }

    public void Start()
    {
        started = true;
    }

    public void Stop()
    {
        started = false;
    }

    protected Vector2 RemapToScreen(Vector2 point)
    {
        return new Vector2(OuterBounds.Left + point.X / currentStep * OuterBounds.Width, OuterBounds.Bottom - (point.Y - min) / (max - min) * OuterBounds.Height);
    }

    public override void Draw(float y)
    {
        base.Draw(y);

        innerBounds = innerBounds.ChangeHeight(innerBounds.Width / 2f);

        // background.Size = innerBounds.size;
        // background.Position = innerBounds.position;
        // background.FillColor = Theme.barColor;
        // background.OutlineColor = Theme.accentColor;
        // background.OutlineThickness = Theme.OutlineThickness;
        // window.Draw(background);

        if (min < 0 && max > 0)
        {
            xAxis.Size = new Vector2(OuterBounds.Width, Theme.LineThickness);
            xAxis.Position = new Vector2(RemapToScreen(new(0, 0)).Y, OuterBounds.Top + OuterBounds.Height / 2 - xAxis.Size.Y / 2);
            xAxis.FillColor = Theme.xAxisColor;
            window.Draw(xAxis);
        }

        foreach (var set in series)
        {
            var plot = set.CreateVertexArray(currentStep, RemapToScreen);
            window.Draw(plot);
            plot.Dispose();
        }
    }
}

