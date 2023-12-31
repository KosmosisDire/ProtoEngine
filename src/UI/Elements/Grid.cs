

namespace ProtoEngine.UI;

public class Grid<T> : Element where T : Element
{
    public delegate T CellBuilder(int x, int y);

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
    public List<T> cells = new();

    public void ForEachCell(Action<T, int, int> action)
    {
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numColumns; j++)
            {
                action(cells[i * numColumns + j], i, j);
            }
        }
    }

    public void ForEachRow(Action<Element> action)
    {
        for (int i = 0; i < numRows; i++)
        {
            action(rows[i]);
        }
    }

    public void AddCellStyle(Style style)
    {
        cells.ForEach(cell => cell.AddStyle(style));
    }

    public void RemoveCellStyle(Style style)
    {
        cells.ForEach(cell => cell.RemoveStyle(style));
    }

    public Grid(Element parent, int rows, int columns, CellBuilder cellBuilder) : base(parent)
    {
        Resize(rows, columns, cellBuilder);
    }

    public Grid(Element parent) : base(parent)
    {
    }


    public T GetCell(int x, int y)
    {
        return cells[y * numColumns + x];
    }

    public T[] GetNeighbors(int x, int y)
    {
        var neighbors = new T[8];

        if (x > 0)
        {
            neighbors[0] = GetCell(x - 1, y);
            if (y > 0) neighbors[1] = GetCell(x - 1, y - 1);
            if (y < numRows - 1) neighbors[2] = GetCell(x - 1, y + 1);
        }

        if (x < numColumns - 1)
        {
            neighbors[3] = GetCell(x + 1, y);
            if (y > 0) neighbors[4] = GetCell(x + 1, y - 1);
            if (y < numRows - 1) neighbors[5] = GetCell(x + 1, y + 1);
        }

        if (y > 0) neighbors[6] = GetCell(x, y - 1);
        if (y < numRows - 1) neighbors[7] = GetCell(x, y + 1);

        // remove nulls
        neighbors = neighbors.Where(obj => obj != null).ToArray();

        return neighbors;
    }

    public void Resize(int rows, int columns, CellBuilder cellBuilder)
    {
        var lastRows = this.numRows;
        var lastColumns = this.numColumns;
        this.numRows = rows;
        this.numColumns = columns;

        if (rows < lastRows)
        {
            for (int i = rows; i < lastRows; i++)
            {
                this.rows[i].Remove();
            }

            for (int i = rows * columns; i < lastRows * lastColumns; i++)
            {
                this.cells[i].Remove();
            }
        }

        if (columns < lastColumns)
        {
            for (int i = columns; i < lastColumns; i++)
            {
                for (int j = 0; j < lastRows; j++)
                {
                    this.cells[j * lastColumns + i].Remove();
                }
            }

            for (int i = rows * columns; i < lastRows * lastColumns; i++)
            {
                this.cells[i].Remove();
            }
        }

        if (rows > lastRows)
        {
            for (int i = lastRows; i < rows; i++)
            {
                var row = new Element(this);
                if(_rowStyle != null) row.SetBaseStyle(_rowStyle.Value);
                row.Style.flowDirection = new DirectionProperty(() => this.ComputedStyle.flowDirection == Direction.Vertical ? Direction.Horizontal : Direction.Vertical);
                row.Style.gap = new AbsPx(() => this.ComputedStyle.gap.Value);
                this.rows.Add(row);

                for (int j = 0; j < lastColumns; j++)
                {
                    var cell = cellBuilder(j, i);
                    cell.Parent = row;
                    if(_cellStyle != null) cell.SetBaseStyle(_cellStyle.Value);
                    this.cells.Add(cell);
                }
            }
        }

        if (columns > lastColumns)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = lastColumns; j < columns; j++)
                {
                    var cell = cellBuilder(j, i);
                    cell.Parent = this.rows[i];
                    if(_cellStyle != null) cell.SetBaseStyle(_cellStyle.Value);
                    this.cells.Add(cell);
                }
            }
        }
    }
}