namespace MusicLoverHandbook.Logic
{
    public static class ByteImageLoader
    {
        public static Image CreateFromBytes(byte[] bytes)
        {
            using (var memStream = new MemoryStream(bytes))
                return Image.FromStream(memStream);
        }
    }
}