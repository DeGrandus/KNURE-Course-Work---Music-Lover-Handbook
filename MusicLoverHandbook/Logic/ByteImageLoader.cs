using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
