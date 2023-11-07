
namespace ProtoEngine.UI;

public abstract class Property<T>
{
    protected Element appliedTo;
    public delegate T FetchValue();

    private FetchValue? _getValue;
    protected FetchValue? GetValue
    {
        get => _getValue;
        set => _getValue = value;
    }

    private FetchValue? _unsetValue;
    protected FetchValue? UnsetValue
    {
        get => _unsetValue;
        set => _unsetValue = value;
    }

    private bool _isUnset;
    public bool IsUnset 
    {
        get => _isUnset;
        set
        {
            _isUnset = value;
            if (value) GetValue = null;
        }
    }

    protected bool createdFromValue = false;

    public T Value 
    {
        get => GetValue is not null ? GetValue.Invoke() : (UnsetValue is not null ? UnsetValue.Invoke() : default!);
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

    public virtual Property<T> TryOverride(Property<T> prop)
    {
        if (prop.IsUnset) return this;
        return prop;
    }

    public void MakeUnset()
    {
        UnsetValue = GetValue;
        IsUnset = true;
    }
}