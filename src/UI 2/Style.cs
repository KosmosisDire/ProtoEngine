// using SFML.Graphics;

// namespace ProtoEngine.UI2;

// public class StyleSet
// {
//     private readonly List<Style> styles = new();
//     public Style FinalStyle {get; private set;} = new();
//     public Style BaseStyle {get; set;}
//     public Action OnStyleChange;

//     public StyleSet()
//     {
//     }

//     public void AddStyle(Style style)
//     {
//         BaseStyle ??= style;

//         styles.Add(style);
//         BuildStyle();
//     }

//     public void RemoveStyle(Style style)
//     {
//         styles.Remove(style);
//         BuildStyle();
//     }

//     public void BuildStyle()
//     {
//         FinalStyle.Clear();
//         foreach (var s in styles)
//         {
//             FinalStyle.Merge(s);
//         }

//         OnStyleChange?.Invoke();
//     }
// }

// public class Style
// {
//     public Rule<Direction?> layoutDirection;
//     public Rule<Alignment?> alignSelf;
//     public Rule<Alignment?> alignContent;
//     public Rule<PositionReference?> positionReference;
    
//     public MeasureRule width;
//     public MeasureRule height;
//     public MeasureRule top;
//     public MeasureRule left;

//     public MeasureRule marginHorizontal;
//     public MeasureRule marginVertical;
//     public MeasureRule paddingHorizontal;
//     public MeasureRule paddingVertical;

//     public MeasureRule fontSize;
//     public MeasureRule cornerRadius;
//     public MeasureRule borderWidth;

//     public Rule<Color?> color;
//     public Rule<Color?> borderColor;

//     public List<RuleBase> rules;


//     public Style() 
//     {
//         Clear();
//     }

//     public void Clear()
//     {
//         rules = new()
//         {
//             (layoutDirection = new Rule<Direction?>(null)),
//             (alignSelf = new Rule<Alignment?>(null)),
//             (alignContent = new Rule<Alignment?>(null)),
//             (positionReference = new Rule<PositionReference?>(null)),
//             (width = new MeasureRule(null)),
//             (height = new MeasureRule(null)),
//             (top = new MeasureRule(null)),
//             (left = new MeasureRule(null)),
//             (marginHorizontal = new MeasureRule(null)),
//             (marginVertical = new MeasureRule(null)),
//             (paddingHorizontal = new MeasureRule(null)),
//             (paddingVertical = new MeasureRule(null)),
//             (fontSize = new MeasureRule(null)),
//             (cornerRadius = new MeasureRule(null)),
//             (borderWidth = new MeasureRule(null)),
//             (color = new Rule<Color?>(null)),
//             (borderColor = new Rule<Color?>(null))
//         };
//     }

//     public void Merge(Style other)
//     {
//         this.layoutDirection = other.layoutDirection.IsSet ? other.layoutDirection : this.layoutDirection;
//         this.alignSelf = other.alignSelf.IsSet ? other.alignSelf : this.alignSelf;
//         this.alignContent = other.alignContent.IsSet ? other.alignContent : this.alignContent;
//         this.positionReference = other.positionReference.IsSet ? other.positionReference : this.positionReference;
//         this.width = other.width.IsSet ? other.width : this.width;
//         this.height = other.height.IsSet ? other.height : this.height;
//         this.top = other.top.IsSet ? other.top : this.top;
//         this.left = other.left.IsSet ? other.left : this.left;
//         this.marginHorizontal = other.marginHorizontal.IsSet ? other.marginHorizontal : this.marginHorizontal;
//         this.marginVertical = other.marginVertical.IsSet ? other.marginVertical : this.marginVertical;
//         this.paddingHorizontal = other.paddingHorizontal.IsSet ? other.paddingHorizontal : this.paddingHorizontal;
//         this.paddingVertical = other.paddingVertical.IsSet ? other.paddingVertical : this.paddingVertical;
//         this.fontSize = other.fontSize.IsSet ? other.fontSize : this.fontSize;
//         this.cornerRadius = other.cornerRadius.IsSet ? other.cornerRadius : this.cornerRadius;
//         this.borderWidth = other.borderWidth.IsSet ? other.borderWidth : this.borderWidth;
//         this.color = other.color.IsSet ? other.color : this.color;
//         this.borderColor = other.borderColor.IsSet ? other.borderColor : this.borderColor;
//     }
// }