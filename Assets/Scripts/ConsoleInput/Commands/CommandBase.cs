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
        protected string _lastResponse;
        
        public CommandBase(string originalInput, TerminalWindow terminal)
        {
            this.terminal = terminal;
            this._originalString = originalInput;
        }

        public void Execute()
        {
            terminal?.StartCoroutine(ExecuteCoroutine());
        }

        public void Respond(string response)
        {
            char[] charsToTrim = { ' ' };
            _lastResponse = response.ToLower().TrimEnd(charsToTrim);
        }

        protected abstract IEnumerator ExecuteCoroutine();

        public static class Response
        {
            public static readonly string Yes = "y";
            public static readonly string No = "n";
            public static readonly string One = "1";
            public static readonly string Two = "2";
            public static readonly string Three = "3";
        }
    }
}