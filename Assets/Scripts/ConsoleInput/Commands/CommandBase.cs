using System.Collections;
using ConsoleInput;
using UnityEngine;

namespace Terminal.Commands
{
    public abstract class CommandBase
    {
        // MonoBehaviour is for coroutine execution
        protected TerminalWindow terminal;
        protected CommandData _data;
        
        public CommandBase(TerminalWindow terminal)
        {
            this.terminal = terminal;
        }

        public void Execute()
        {
            terminal?.StartCoroutine(ExecuteCoroutine());
        }

        protected abstract IEnumerator ExecuteCoroutine();
    }
}