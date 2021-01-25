﻿using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Script;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace OpenBots.Commands.Loop
{
    [Serializable]
    [Category("Loop Commands")]
    [Description("This command repeats the execution of subsequent actions continuously.")]
    public class LoopContinuouslyCommand : ScriptCommand
    {
        public LoopContinuouslyCommand()
        {
            CommandName = "LoopContinuouslyCommand";
            SelectionName = "Loop Continuously";
            CommandEnabled = true;           
        }

        public override void RunCommand(object sender, ScriptAction parentCommand)
        {
            LoopContinuouslyCommand loopCommand = (LoopContinuouslyCommand)parentCommand.ScriptCommand;
            var engine = (AutomationEngineInstance)sender;
            engine.ReportProgress("Starting Continous Loop From Line " + loopCommand.LineNumber);

            while (true)
            {
                foreach (var cmd in parentCommand.AdditionalScriptCommands)
                {
                    if (engine.IsCancellationPending)
                        return;

                    engine.ExecuteCommand(cmd);

                    if (engine.CurrentLoopCancelled)
                    {
                        engine.ReportProgress("Exiting Loop From Line " + loopCommand.LineNumber);
                        engine.CurrentLoopCancelled = false;
                        return;
                    }

                    if (engine.CurrentLoopContinuing)
                    {
                        engine.ReportProgress("Continuing Next Loop From Line " + loopCommand.LineNumber);
                        engine.CurrentLoopContinuing = false;
                        break;
                    }
                }
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue();
        }
    }
}