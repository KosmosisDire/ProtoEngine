
// namespace ProtoEngine.UI2;

// public class RuleBase
// {
//     private object value;
//     public object Value
//     {
//         get => value;
//         set
//         {
//             this.value = value;
//             IsSet = value != null;
//         }
//     }
//     public bool IsSet { get; protected set; }

//     public RuleBase(object value)
//     {
//         Value = value;
//     }
// }

// public class Rule<T> : RuleBase
// {
//     public new T Value
//     {
//         get => (T)base.Value;
//         set => base.Value = value;
//     }

//     public Rule(T value) : base(value)
//     {
//     }

//     public static implicit operator Rule<T>(T value) => new(value);
//     public static implicit operator T(Rule<T> value) => value.Value;
// }

// public class MeasureRule : Rule<Measure?>
// {
//     public MeasureRule(Measure? value) : base(value)
//     {
//     }

//     public static implicit operator MeasureRule(Measure? value) => new(value);
//     public static implicit operator MeasureRule(float value) => new(value);
//     public static implicit operator float?(MeasureRule value) => value.Value?.Value;
// }
