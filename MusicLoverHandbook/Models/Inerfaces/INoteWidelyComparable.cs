namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface INoteWidelyComparable
    {
        #region Public Methods

        bool Equals(object? obj);

        int GetHashCode();

        bool RoughEquals(object? obj);

        #endregion Public Methods
    }
}
