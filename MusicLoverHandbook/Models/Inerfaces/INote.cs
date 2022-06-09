using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLoverHandbook.Models.Inerfaces
{
    internal interface INote
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public INote InnerContent { get; set; }
    }
}
