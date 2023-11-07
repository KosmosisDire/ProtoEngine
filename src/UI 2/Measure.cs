namespace ProtoEngine.UI2;

// public class UnitVector2
// {
//     public Element? Element 
//     {
//         get => x?.element;
//         set
//         {
//             if (x != null) x.element = value;
//             if (y != null) y.element = value;
//         }
//     }

//     public UnitValue? x;
//     public UnitValue? y;

//     public UnitVector2(UnitValue x, UnitValue y) {
//         this.x = x;
//         this.y = y;
//     }

//     public UnitVector2(float x, float y, Units units, Element? context = null) {
//         this.x = new UnitValue(x, units, context);
//         this.y = new UnitValue(y, units, context);
//     }

//     public UnitVector2(float x, float y, Units units, UnitVector2? percentageOf, Element? context = null) 
//     {
//         var percentageOfX = percentageOf?.x ?? context?.Width ?? 0f;
//         var percentageOfY = percentageOf?.y ?? context?.Height ?? 0f;

//         this.x = new UnitValue(x, units, percentageOfX, context);
//         this.y = new UnitValue(y, units, percentageOfY, context);
//     }

//     public void Recalculate() 
//     {
//         x?.Recalculate();
//         y?.Recalculate();
//     }

//     public Vector2 ConvertTo(Units units)
//     {
//         return new Vector2(x?.ConvertTo(units) ?? Element?.Width ?? 0f, y?.ConvertTo(units) ?? Element?.Height ?? 0f);
//     }

//     public static implicit operator Vector2(UnitVector2 unitVector2) {
//         return new Vector2(unitVector2?.x ?? 0f, unitVector2?.y ?? 0f);
//     }
//     public static implicit operator UnitVector2(Vector2 vector) {
//         return new UnitVector2(vector.X, vector.Y, Units.Pixels, null);
//     }

//     public static UnitVector2 operator +(UnitVector2 unitVector2, Vector2 vector) {
//         if(unitVector2.x != null) unitVector2.x += vector.X;
//         if(unitVector2.y != null) unitVector2.y += vector.Y;
//         return unitVector2;
//     }
//     public static UnitVector2 operator -(UnitVector2 unitVector2, Vector2 vector) {
//         if(unitVector2.x != null) unitVector2.x -= vector.X;
//         if(unitVector2.y != null) unitVector2.y -= vector.Y;
//         return unitVector2;
//     }
//     public static UnitVector2 operator *(UnitVector2 unitVector2, float value) {
//         if(unitVector2.x != null) unitVector2.x *= value;
//         if(unitVector2.y != null) unitVector2.y *= value;
//         return unitVector2;
//     }
//     public static UnitVector2 operator /(UnitVector2 unitVector2, float value) {
//         if(unitVector2.x != null) unitVector2.x /= value;
//         if(unitVector2.y != null) unitVector2.y /= value;
//         return unitVector2;
//     }
//     public static UnitVector2 operator -(UnitVector2 unitVector2) {
//         if(unitVector2?.x != null) unitVector2.x = -(unitVector2?.x ?? 0f);
//         if(unitVector2?.y != null) unitVector2.y = -(unitVector2?.y ?? 0f);
//         return unitVector2 ?? new UnitVector2(0f, 0f, Units.Pixels, null);
//     }
// }

// public class Measure: IDisposable, IEquatable<Measure>
// {
//     public Element? element;
//     private Measure? percentageOf;
//     public float Value { get; private set; }
//     public Units Units { get; private set; }
//     public float Pixels { get; private set; }
//     public List<Measure> dependents = new();

//     public Measure(int value, Units units, Element? context = null) {
//         Update(value, units, context);
//     }

//     public Measure(float value, Units units, Element? context = null) {
//         Update(value, units, context);
//     }

//     public void Update(float value, Units units, Element? context = null) {
//         Value = value;
//         Units = units;
//         element = context;
//         if (units == Units.Percent) {
//             throw new Exception("Cannot use percent units without specifying a percentage reference");
//         }
//         Recalculate();
//     }

//     public Measure(float value, Units units, Measure? percentageOf, Element? context = null) {
//         Value = value;
//         Units = units;
//         element = context;
//         this.percentageOf = percentageOf;
//         percentageOf?.dependents.Add(this);
//         Recalculate();
//     }

//     public void UpdateFrom(Measure other) 
//     {
//         Value = other.Value;
//         Units = other.Units;
//         element = other.element;
//         percentageOf = other.percentageOf;
//         percentageOf?.dependents.Add(this);
//         Recalculate();
//     }

//     public void Recalculate() 
//     {
//         Pixels = CalculatePixels();
//         foreach (var dependent in dependents)
//         {
//             if (dependent == this) continue;
//             dependent?.Recalculate();
//         }
//     }

//     private float CalculatePixels() 
//     {
//         switch (Units)
//         {
//             case Units.Pixels:
//                 return Value;
//             case Units.Percent:
//                 return Value * (percentageOf ?? 0f);
//             case Units.Characters:
//                 return Value * (element?.Parent?.FontSize ?? 12f);
//             default:
//                 return Value;
//         }
//     }

//     public float ConvertTo(Units units)
//     {
//         switch (units)
//         {
//             case Units.Pixels:
//                 return Pixels;
//             case Units.Percent:
//                 return Pixels / (percentageOf ?? 0f);
//             case Units.Characters:
//                 return Pixels / (element?.Parent?.FontSize ?? 0f);
//             default:
//                 return Pixels;
//         }
//     }

//     public static implicit operator float(Measure unitValue) {
//         return unitValue.Pixels;
//     }
//     public static implicit operator Measure(float value) {
//         return new Measure(value, Units.Pixels, null);
//     }

//     public static Measure operator +(Measure unitValue, float value) {
//         unitValue.Value += value;
//         unitValue.Recalculate();
//         return unitValue;
//     }
//     public static Measure operator -(Measure unitValue, float value) {
//         unitValue.Value -= value;
//         unitValue.Recalculate();
//         return unitValue;
//     }
//     public static Measure operator *(Measure unitValue, float value) {
//         unitValue.Value *= value;
//         unitValue.Recalculate();
//         return unitValue;
//     }
//     public static Measure operator /(Measure unitValue, float value) {
//         unitValue.Value /= value;
//         unitValue.Recalculate();
//         return unitValue;
//     }
//     public static Measure operator -(Measure unitValue) {
//         unitValue.Value = -unitValue.Value;
//         unitValue.Recalculate();
//         return unitValue;
//     }

//     public static Measure Parse(string value, Element context) {
//         try
//         {
//             if (value.EndsWith("%"))
//             {
//                 return new Measure(float.Parse(value.Substring(0, value.Length - 1)), Units.Percent, context);
//             }
//             else if (value.EndsWith("px"))
//             {
//                 return new Measure(float.Parse(value.Substring(0, value.Length - 2)), Units.Pixels, context);
//             }
//             else if (value.EndsWith("ch"))
//             {
//                 return new Measure(float.Parse(value.Substring(0, value.Length - 2)), Units.Characters, context);
//             }
//             else
//             {
//                 return new Measure(float.Parse(value), Units.Pixels, context);
//             }
//         }
//         catch
//         {
//             throw new Exception("Invalid unit value: " + value);
//         }
//     }

//     public override string ToString() {
//         switch (Units)
//         {
//             case Units.Pixels:
//                 return Value + "px";
//             case Units.Percent:
//                 return Value + "%";
//             case Units.Characters:
//                 return Value + "ch";
//             default:
//                 return Value.ToString();
//         }
//     }

//     public void Dispose()
//     {
//         percentageOf?.dependents.Remove(this);
//     }

//     public bool Equals(Measure? other)
//     {
//         if (other == null)
//         {
//             return false;
//         }
        
//         return other == this;
//     }

//     ~Measure()
//     {
//         Dispose();
//     }
// }