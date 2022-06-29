namespace MusicLoverHandbook.Controls_and_Forms.UserControls
{
    [Flags]
    public enum PathAnalyzerResult
    {
        File_NotMp3 = 1,
        File_HasEquivalence = 2,
        File_DoesNotExist = 4,
        File_InMusicFolder = 8,
        File_NotInMusicFolder = 16,
        IsNotAFile = 32,
    }
}