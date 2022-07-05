using MusicLoverHandbook.Models.Attributes;

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
        #region Public Properties

        public override string Message => base.Message;

        #endregion Public Properties

        #region Public Constructors + Destructors

        public MissingRequiredAttributeException(object source, Type missingAttirbute)
            : base(Decorate(source, missingAttirbute))
        {
            Source = source.ToString();
        }

        #endregion Public Constructors + Destructors

        #region Private Methods

        private static string Decorate(object source, Type missing)
        {
            return $"{source.GetType()} \"{source}\" missing attribute: {missing}";
        }

        #endregion Private Methods
    }
}
