namespace MusicLoverHandbook.Models.Attributes
{
    public class ConnectedNoteTypeAttribute : Attribute
    {
        public Type ConnectedType { get; }

        public ConnectedNoteTypeAttribute(Type noteType)
        {
            ConnectedType = noteType;
        }
    }
}
