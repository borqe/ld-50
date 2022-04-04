using System.Collections;
using ConsoleInput;
using UnityEngine;

namespace Terminal.Commands
{
    public class HelpCommand: CommandBase
    {
        public HelpCommand(string originalInput, TerminalWindow terminal) : base(originalInput, terminal)
        {
            if (TerminalCommandData.Commands.Data.ContainsKey("help"))
            {
                _data = TerminalCommandData.Commands.Data["help"];
            }
            else
            {
                Debug.LogError("Terminal command data for 'help' not loaded in!");
            }
        }

        protected override IEnumerator ExecuteCoroutine()
        {
            terminal.PrintOutput(_originalString, _data.outputs["default"]);
            terminal.PrintOutput("", TerminalCommandData.InstructionMessage, false);
            terminal.ClearLastCommand();
            yield return null;
        }
    }
}