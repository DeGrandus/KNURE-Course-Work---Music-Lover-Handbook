namespace MusicLoverHandbook.Logic
{
    public static partial class StringTagTools
    {
        #region Public Classes

        public abstract class Taggable : ITaggable
        {
            #region Public Properties

            public abstract TagType Type { get; }
            public string? Value { get; }
            public TagDataType ValueType { get; }

            #endregion Public Properties



            #region Public Constructors

            public Taggable(TagDataType type, string? value = null)
            {
                ValueType = type;
                if (ValueType == TagDataType.General)
                    return;

                Value = value;
            }

            #endregion Public Constructors



            #region Public Methods

            public override bool Equals(object? obj)
            {
                return obj is Taggable taggable
                    && Type == taggable.Type
                    && ValueType == taggable.ValueType
                    && Value == taggable.Value;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Type, ValueType, Value);
            }

            public override string ToString()
            {
                return $"Type: [{Type}] | ValueType: [{ValueType}]"
                    + (ValueType == TagDataType.General ? "" : $" | Value: [{Value}]");
            }

            #endregion Public Methods
        }

        #endregion Public Classes
    }
}
