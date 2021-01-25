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

namespace OpenBots.Commands.Input
{
	[Serializable]
	[Category("Input Commands")]
	[Description("This command simulates a mouse movement to a specified position.")]
	public class SendMouseMoveCommand : ScriptCommand, ISendMouseMoveCommand
	{

		[Required]
		[DisplayName("X Position")]
		[Description("Input the new horizontal coordinate of the mouse. Starts from 0 on the left and increases going right.")]
		[SampleUsage("0 || {vXPosition}")]
		[Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range would be 0-1920.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowMouseCaptureHelper", typeof(UIAdditionalHelperType))]
		public string v_XMousePosition { get; set; }

		[Required]
		[DisplayName("Y Position")]
		[Description("Input the new vertical coordinate of the mouse. Starts from 0 at the top and increases going down.")]
		[SampleUsage("0 || {vYPosition}")]
		[Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range would be 0-1080.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowMouseCaptureHelper", typeof(UIAdditionalHelperType))]
		public string v_YMousePosition { get; set; }

		[DisplayName("Click Type (Optional)")]
		[PropertyUISelectionOption("None")]
		[PropertyUISelectionOption("Left Click")]
		[PropertyUISelectionOption("Middle Click")]
		[PropertyUISelectionOption("Right Click")]
		[PropertyUISelectionOption("Double Left Click")]
		[PropertyUISelectionOption("Left Down")]
		[PropertyUISelectionOption("Middle Down")]
		[PropertyUISelectionOption("Right Down")]
		[PropertyUISelectionOption("Left Up")]
		[PropertyUISelectionOption("Middle Up")]
		[PropertyUISelectionOption("Right Up")]
		[Description("Indicate the type of click required.")]
		[SampleUsage("")]
		[Remarks("You can simulate a custom click by using multiple mouse click commands in succession, adding **Pause Command** in between where required.")]
		public string v_MouseClick { get; set; }

		public SendMouseMoveCommand()
		{
			CommandName = "SendMouseMoveCommand";
			SelectionName = "Send Mouse Move";
			CommandEnabled = true;
			
			v_XMousePosition = "0";
			v_YMousePosition = "0";
			v_MouseClick = "None";
		}

		public override void RunCommand(object sender)
		{
			var engine = (AutomationEngineInstance)sender;
			var mouseX = v_XMousePosition.ConvertUserVariableToString(engine);
			var mouseY = v_YMousePosition.ConvertUserVariableToString(engine);

			User32Functions.SendMouseMove(mouseX, mouseY, v_MouseClick);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_XMousePosition", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_YMousePosition", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_MouseClick", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Target Coordinates '({v_XMousePosition},{v_YMousePosition})' - Click Type '{v_MouseClick}']";
		}    
	}
}