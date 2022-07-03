using MusicLoverHandbook.Controls_and_Forms.Forms;

namespace MusicLoverHandbook
{
    internal static class Program
    {
        #region Internal Properties

        internal static MainForm MainForm => new MainForm();

        #endregion Internal Properties

        #region Private Methods

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            ApplicationConfiguration.Initialize();
            Application.Run(MainForm);
        }

        #endregion Private Methods
    }
}
