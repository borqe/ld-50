using System.Collections;
using UnityEngine;

namespace ConsoleInput.Commands
{
    public class ClearCommand: CommandBase
    {
        public ClearCommand(ConsoleWindow console) : base(console)
        {
        }

        protected override IEnumerator ExecuteCoroutine()
        {
            yield return null;
        }
    }
}