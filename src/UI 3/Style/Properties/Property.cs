
namespace ProtoEngine.UI3;

public abstract class Property<T>
{
    protected Element appliedTo;
    public delegate T FetchValue();
    private FetchValue? oldGetValue;

    private FetchValue? _getValue;
    protected FetchValue? GetValue
    {
        get => _getValue;
        set
        {
            _getValue = value;
            oldGetValue = value;
        }
    }

    private FetchValue? _unsetValue;
    protected FetchValue? UnsetValue
    {
        get => _unsetValue;
        set 
        {
            _unsetValue = value;
            if (IsUnset)
            {
                _getValue = value;
            }
        }
    }

    private bool _isUnset;
    public bool IsUnset 
    {
        get => _isUnset;
        set
        {
            _isUnset = value;
            if (value)
            {
                _getValue = UnsetValue;
            }
            else
            {
                GetValue = oldGetValue;
            }
        }
    }

    protected bool createdFromValue = false;

    public T Value 
    {
        get => GetValue == null ? default! : GetValue.Invoke();
        set
        {
            createdFromValue = true;
            GetValue = () => value;
            IsUnset = false;
        }
    }

    public Property(bool unset)
    {
        GetValue = null;
        IsUnset = unset;
    }

    public Property<T> TryOverride(Property<T> overrideMeasure)
    {
        if (overrideMeasure.IsUnset) return this;
        return overrideMeasure;
    }
}