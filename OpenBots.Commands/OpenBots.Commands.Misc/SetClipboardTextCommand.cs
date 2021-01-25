﻿using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.User32;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;

namespace OpenBots.Commands.Misc
{
	[Serializable]
	[Category("Misc Commands")]
	[Description("This command sets text to the user's clipboard.")]
	public class SetClipboardTextCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Text")]
		[Description("Select or provide the text to set on the clipboard.")]
		[SampleUsage("Hello || {vTextToSet}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_TextToSet { get; set; }

		public SetClipboardTextCommand()
		{
			CommandName = "SetClipboardTextCommand";
			SelectionName = "Set Clipboard Text";
			CommandEnabled = true;            
		}

		public override void RunCommand(object sender)
		{
			var engine = (AutomationEngineInstance)sender;
			var input = v_TextToSet.ConvertUserVariableToString(engine);

			User32Functions.SetClipboardText(input);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TextToSet", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Text '{v_TextToSet}']";
		}
	}
}
