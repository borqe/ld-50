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
        protected string _originalString;
        
        public CommandBase(string originalInput, TerminalWindow terminal)
        {
            this.terminal = terminal;
            this._originalString = originalInput;
        }

        public void Execute()
        {
            terminal?.StartCoroutine(ExecuteCoroutine());
        }

        protected abstract IEnumerator ExecuteCoroutine();
    }
}