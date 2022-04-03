using System.Collections;
using UnityEngine;

namespace Terminal.Commands
{
    public class ClearCommand: CommandBase
    {
        public ClearCommand(TerminalWindow terminal) : base(terminal)
        {
        }

        protected override IEnumerator ExecuteCoroutine()
        {
            yield return null;
        }
    }
}