using MusicLoverHandbook.Controls_and_Forms.Custom_Controls;
using MusicLoverHandbook.Controls_and_Forms.UserControls;
using MusicLoverHandbook.Models.Enums;
using MusicLoverHandbook.Models.Inerfaces;
using MusicLoverHandbook.Models.NoteAlter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLoverHandbook.Models.Delegates
{
    public delegate void BasicFilterResultsChangedHandler(List<LiteNote> prefilteredResults);
    public delegate void PathAnalyzerResultHandler(
        PathAnalyzerResult result,
        string obeservedString
    );
    public delegate void AdvancedFilteringModeChangedHandler(BasicSwitchLabel self, bool isSpecial);
    public delegate void StateChangeHandler(object? sender, bool IsSpecialState);
    public delegate void StateChangedEvent(SmartComboBox sender, InputStatus state);
    public delegate void QuickSearchResultHandler(IEnumerable<INoteControlChild> QSResult);
}
