using MusicLoverHandbook.Controls_and_Forms.UserControls.Notes;
using MusicLoverHandbook.Models.Attributes;

namespace MusicLoverHandbook.Models.Enums
{
    public enum NoteType
    {
        [EnumColor(255, 0x6881DD, 255, 0xADB9DF)]
        [InformationCarrier]
        [AssociatedType(typeof(NoteAuthor))]
        [AssociatedNoteCreationType(NoteCreationOrder.AuthorThenDisc)]
        Author,

        [EnumColor(255, 0x899FF0, 255, 0xAEDFDC)]
        [InformationCarrier]
        [AssociatedType(typeof(NoteDisc))]
        [AssociatedNoteCreationType(NoteCreationOrder.DiscThenAuthor)]
        Disc,

        [EnumColor(255, 0x9DB0F3, 255, 0xE2ECAF)]
        [InformationCarrier]
        [StringValue("Song name")]
        [AssociatedType(typeof(NoteSong))]
        Song,

        [EnumColor(255, 0xAEBDF3, 255, 0xC7B1D5)]
        [InformationCarrier]
        [StringValue("Song file")]
        [AssociatedType(typeof(NoteSongFile))]
        SongFile,

        [EnumColor(255, 0xC9D3F7)]
        AddButton
    }
}
