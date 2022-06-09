using MusicLoverHandbook.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLoverHandbook.Models.Inerfaces
{
    internal interface INote
    {
        string NoteText { get; set; }
        string NoteDescription { get; set; }
    }
    internal interface INote<InnerContentType> where InnerContentType : INote
    { 
        public IReadOnlyCollection<InnerContentType> InnerContent { get; }

    }
}
