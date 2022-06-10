using MusicLoverHandbook.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLoverHandbook.Models.Inerfaces
{
    public interface ISmartComboBox
    {
        InputState State { get; set; }
        InputType InputType { get; }
    }
}
