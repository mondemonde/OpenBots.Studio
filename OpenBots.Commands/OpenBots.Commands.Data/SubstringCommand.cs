﻿using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;

namespace OpenBots.Commands.Data
{
	[Serializable]
	[Category("Data Commands")]
	[Description("This command returns a substring from a specified string.")]
	public class SubstringCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Text Data")]
		[Description("Provide a variable or text value.")]
		[SampleUsage("Sample text to extract substring from || {vTextData}")]
		[Remarks("Providing data of a type other than a 'String' will result in an error.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_InputText { get; set; }

		[Required]
		[DisplayName("Starting Index")]
		[Description("Indicate the starting position within the text.")]
		[SampleUsage("0 || 1 || {vStartingIndex}")]
		[Remarks("0 for beginning, 1 for first character, n for nth character")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_StartIndex { get; set; }

		[DisplayName("Substring Length (Optional)")]
		[Description("Indicate number of characters to extract.")]
		[SampleUsage("1 || {vSubstringLength}")]
		[Remarks("1 for 1 position after start index, etc.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_StringLength { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Substring Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("{vUserVariable}")]
		[Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
		public string v_OutputUserVariableName { get; set; }

		public SubstringCommand()
		{
			CommandName = "SubstringCommand";
			SelectionName = "Substring";
			CommandEnabled = true;
		}

		public override void RunCommand(object sender)
		{
			var engine = (AutomationEngineInstance)sender;
			var inputText = v_InputText.ConvertUserVariableToString(engine);
			var startIndex = int.Parse(v_StartIndex.ConvertUserVariableToString(engine));
			var stringLength = v_StringLength.ConvertUserVariableToString(engine);

			//apply substring
			if (!string.IsNullOrEmpty(stringLength))
			{
				int length = int.Parse(stringLength);
				inputText = inputText.Substring(startIndex, length);
			}
			else
			{
				inputText = inputText.Substring(startIndex);
			}

			inputText.StoreInUserVariable(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			//create standard group controls
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InputText", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_StartIndex", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_StringLength", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Get Substring From '{v_InputText}' - " +
				$"Store Substring in '{v_OutputUserVariableName}']";
		}
	}
}