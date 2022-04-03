using System.Collections;
using ConsoleInput;
using UnityEngine;

namespace Terminal.Commands
{
    public class ClearCommand: CommandBase
    {
        public ClearCommand(string originalInput, TerminalWindow terminal) : base(originalInput, terminal)
        {
            if (TerminalCommandData.Commands.Data.ContainsKey("clear"))
            {
                _data = TerminalCommandData.Commands.Data["clear"];
            }
            else
            {
                Debug.LogError("Terminal command data not loaded in!");
            }
        }

        protected override IEnumerator ExecuteCoroutine()
        {
            terminal.ClearOutput();
            terminal.ClearLastCommand();
            yield return null;
        }
    }
}