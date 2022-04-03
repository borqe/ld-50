using System.Collections;
using UnityEngine;

namespace ConsoleInput.Commands
{
    public abstract class CommandBase
    {
        // MonoBehaviour is for coroutine execution
        protected ConsoleWindow _console;
        protected CommandData _data;
        
        public CommandBase(ConsoleWindow console)
        {
            _console = console;
        }

        public void Execute()
        {
            _console?.StartCoroutine(ExecuteCoroutine());
        }

        protected abstract IEnumerator ExecuteCoroutine();
    }
}