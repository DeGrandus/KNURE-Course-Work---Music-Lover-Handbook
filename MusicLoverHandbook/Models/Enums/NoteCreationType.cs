using MusicLoverHandbook.Models.Attributes;
using System.Reflection;

namespace MusicLoverHandbook.Models.Enums
{
    public enum NoteCreationOrder
    {
        [Order(NoteType.Author, NoteType.Disc, NoteType.Song, NoteType.SongFile)]
        AuthorThenDisc,

        [Order(NoteType.Disc, NoteType.Author, NoteType.Song, NoteType.SongFile)]
        DiscThenAuthor
    }

    public class MissingRequiredAttributeException : Exception
    {
        public override string Message => base.Message;

        public MissingRequiredAttributeException(object source, Type missingAttirbute)
            : base(Decorate(source, missingAttirbute))
        {
            Source = source.ToString();
        }

        private static string Decorate(object source, Type missing)
        {
            return $"{source.GetType()} \"{source}\" missing attribute: {missing}";
        }
    }
}
