using System.Collections;
using UnityEngine;

namespace Terminal.Commands
{
    public class UndefinedCommand: CommandBase
    {
        public UndefinedCommand(TerminalWindow terminal) : base(terminal)
        {
        }

        protected override IEnumerator ExecuteCoroutine()
        {
            terminal.PrintOutput(_data.name, _data.outputs["default"]);
            yield return null;
        }
    }
}