namespace MusicLoverHandbook.Models.Attributes
{
    public class AssociatedTypeAttribute : Attribute
    {
        public Type Type { get; }

        public AssociatedTypeAttribute(Type noteType)
        {
            Type = noteType;
        }
    }
}
