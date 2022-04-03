using System.Collections;
using UnityEngine;

namespace ConsoleInput.Commands
{
    public class UndefinedCommand: CommandBase
    {
        public UndefinedCommand(ConsoleWindow console) : base(console)
        {
        }

        protected override IEnumerator ExecuteCoroutine()
        {
            _console.PrintOutput(_data.name, _data.outputs["default"]);
            yield return null;
        }
    }
}