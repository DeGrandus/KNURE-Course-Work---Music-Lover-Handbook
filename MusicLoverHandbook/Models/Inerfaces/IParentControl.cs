using System.Collections.ObjectModel;

namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface IParentControl
    {
        #region Public Properties

        ObservableCollection<INoteControlChild> InnerNotes { get; }

        #endregion Public Properties
    }
}
