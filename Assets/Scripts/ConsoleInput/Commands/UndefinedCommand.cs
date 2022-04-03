using System;
using System.Collections;
using ConsoleInput;
using UnityEngine;

namespace Terminal.Commands
{
    public class UndefinedCommand: CommandBase
    {
        public UndefinedCommand(string originalInput, TerminalWindow terminal) : base(originalInput, terminal)
        {
            if (TerminalCommandData.Commands.Data.ContainsKey("undefined"))
            {
                _data = TerminalCommandData.Commands.Data["undefined"];
            }
            else
            {
                Debug.LogError("Terminal command data not loaded in!");
            }
        }

        protected override IEnumerator ExecuteCoroutine()
        {
            terminal.PrintOutput(_originalString, String.Format(_data.outputs["default"], _originalString));
            yield return null;
        }
    }
}