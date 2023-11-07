

namespace ProtoEngine.UI;

public class Grid : Element
{
    public int numRows = 0;
    public int numColumns = 0;

    private Style? _rowStyle;
    public Style RowStyle
    {
        set
        {
            rows.ForEach(row => row.SetBaseStyle(value));
            _rowStyle = value;
        }
    }

    private Style? _cellStyle;
    public Style CellStyle
    {
        set
        {
            cells.ForEach(cell => cell.SetBaseStyle(value));
            _cellStyle = value;
        }
    }

    public List<Element> rows = new();
    public List<Element> cells = new();

    public void ForEachCell(Action<Element> action)
    {
        cells.ForEach(action);
    }

    public void ForEachRow(Action<Element> action)
    {
        rows.ForEach(action);
    }

    public void AddCellStyle(Style style)
    {
        cells.ForEach(cell => cell.AddStyle(style));
    }

    public void RemoveCellStyle(Style style)
    {
        cells.ForEach(cell => cell.RemoveStyle(style));
    }

    public Grid(Element parent, int rows, int columns, ElementBuilder cellBuilder) : base(parent)
    {
        Regenerate(rows, columns, cellBuilder);
    }

    public void Regenerate(int rows, int columns, ElementBuilder cellBuilder)
    {
        if (rows < this.numRows)
        {
            for (int i = rows; i < this.numRows; i++)
            {
                this.rows[i].Remove();
                this.rows.RemoveAt(i);
                i--;
            }
        }

        if (columns < this.numColumns)
        {
            for (int i = columns; i < this.numColumns; i++)
            {
                this.cells[i].Remove();
                this.cells.RemoveAt(i);
                i--;
            }
        }

        if (rows > this.numRows)
        {
            for (int i = this.numRows; i < rows; i++)
            {
                var row = new Element(this);
                if(_rowStyle != null) row.SetBaseStyle(_rowStyle.Value);
                row.Style.flowDirection = new DirectionProperty(() => this.ComputedStyle.flowDirection == Direction.Vertical ? Direction.Horizontal : Direction.Vertical);
                row.Style.gap = new AbsPx(() => this.ComputedStyle.gap.Value);
                this.rows.Add(row);

                for (int j = 0; j < this.numColumns; j++)
                {
                    var cell = cellBuilder();
                    cell.Parent = row;
                    if(_cellStyle != null) cell.SetBaseStyle(_cellStyle.Value);
                    this.cells.Add(cell);
                }
            }
        }

        if (columns > this.numColumns)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = this.numColumns; j < columns; j++)
                {
                    var cell = cellBuilder();
                    cell.Parent = this.rows[i];
                    if(_cellStyle != null) cell.SetBaseStyle(_cellStyle.Value);
                    this.cells.Add(cell);
                }
            }
        }

        this.numRows = rows;
        this.numColumns = columns;
    }
}