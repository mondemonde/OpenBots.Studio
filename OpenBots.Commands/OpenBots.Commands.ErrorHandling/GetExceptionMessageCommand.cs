﻿using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Forms;

namespace OpenBots.Commands.ErrorHandling
{
	[Serializable]
	[Category("Error Handling Commands")]
	[Description("This command retrieves the most recent error in the engine and stores it in the defined variable.")]
	public class GetExceptionMessageCommand : ScriptCommand
	{

		[Required]
		[Editable(false)]
		[DisplayName("Output Exception Message Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("{vUserVariable}")]
		[Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
		public string v_OutputUserVariableName { get; set; }

		public GetExceptionMessageCommand()
		{
			CommandName = "GetExceptionMessageCommand";
			SelectionName = "Get Exception Message";
			CommandEnabled = true;            
		}

		public override void RunCommand(object sender)
		{
			var engine = (AutomationEngineInstance)sender;
			var error = engine.ErrorsOccured.OrderByDescending(x => x.LineNumber).FirstOrDefault();
			string errorMessage = string.Empty;
			if (error != null)
				errorMessage = $"Source: {error.SourceFile}, Line: {error.LineNumber}, " +
					$"Exception Type: {error.ErrorType}, Exception Message: {error.ErrorMessage}";
			errorMessage.StoreInUserVariable(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Store Exception Message in '{v_OutputUserVariableName}']";
		}
	}
}