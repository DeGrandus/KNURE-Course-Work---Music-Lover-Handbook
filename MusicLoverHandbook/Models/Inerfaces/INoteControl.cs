﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLoverHandbook.Models.Inerfaces
{
    internal interface INoteControl : INote
    {
        new INoteControl InnerContent { get; set; }
    }
}