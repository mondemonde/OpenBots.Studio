using Steeroid.Models.Scripting;
using System.Collections.Generic;
using System.Drawing;
//using System.Windows.Forms;
//using taskt.Core.Script;
//using taskt.UI.Forms;

namespace Steeroid.Models.Interfaces.Scripting
{
    public interface IScriptCommand
    {
        bool CommandEnabled { get; set; }
        string CommandID { get; set; }
        string CommandName { get; set; }
        bool CustomRendering { get; set; }
        int DefaultPause { get; set; }
        Color DisplayForeColor { get; set; }
        bool IsCommented { get; set; }
        int LineNumber { get; set; }
        bool PauseBeforeExeucution { get; set; }
        string SelectionName { get; set; }
        string v_Comment { get; set; }

        void GenerateID();
        string GetDisplayValue();
        void ReLoad(object editor = null);
        void Render(object editor = null);
        //List<> Render(frmCommandEditor editor);
        void MyRunCommand(object sender);
        void MyRunCommand(object sender, IScriptAction command);
    }
}