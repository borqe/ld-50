using System.Collections;
using ConsoleInput;
using UnityEngine;

namespace Terminal.Commands
{
    public class StartupCommand: CommandBase
    {
        public StartupCommand(string originalInput, TerminalWindow terminal) : base(originalInput, terminal)
        {
            if (TerminalCommandData.Commands.Data.ContainsKey("startup"))
            {
                _data = TerminalCommandData.Commands.Data["startup"];
            }
            else
            {
                Debug.LogError("Terminal command data for 'startup' not loaded in!");
            }
        }

        protected override IEnumerator ExecuteCoroutine()
        {
            terminal.HideUser();
            terminal.PrintOutput("",_data.outputs["booting"], false);
            yield return terminal.StartCoroutine(terminal.LoadingCoroutine(2.0f));
            terminal.PrintOutput("",_data.outputs["successful"], false);
            yield return terminal.StartCoroutine(terminal.LoadingCoroutine(1.0f));
            terminal.PrintOutput("", TerminalCommandData.InstructionMessage, false);
            // while (true)
            // {
            //     yield return null;
            //     
            //     
            // }
            terminal.ClearLastCommand();
            terminal.ShowUser();
        }
    }
}