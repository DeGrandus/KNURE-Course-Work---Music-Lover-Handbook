using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLoverHandbook.Controls_and_Forms.Custom_Controls
{
    public class RenderPanel : Panel
    {
        public RenderPanel()
        {
            AdjustFormScrollbars(true);
        }
        protected override void AdjustFormScrollbars(bool displayScrollbars)
        {
            VerticalScroll.Visible = true;
        }
    }
}
