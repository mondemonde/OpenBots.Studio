
using System;
using System.Drawing;
using System.Windows.Forms;
using taskt.Core.Automation.Commands;
using Tasktskie.Common.UI.Forms;

namespace Tasktskie.Common.Contracts
{
    public interface IPlugger
    {
        /// <summary>
        /// Name of plugger
        /// </summary>
        string PluggerName { get; set; }

        /// <summary>
        /// It will return UserControl which will display on Main application
        /// </summary>
        /// <returns></returns>
        UserControl GetPlugger();

        Image Icon { get; }

        Type CommandType { get; }

        bool IsWebRecorder { get; }
        UIWebRecorderForm GetWebRecorder();



    }
}
