using System.Collections;
using ConsoleInput;
using UnityEngine;

namespace Terminal.Commands
{
    public class LoginCommand: CommandBase
    {
        public LoginCommand(string originalInput, TerminalWindow terminal) : base(originalInput, terminal)
        {
            if (TerminalCommandData.Commands.Data.ContainsKey("login"))
            {
                _data = TerminalCommandData.Commands.Data["login"];
            }
            else
            {
                Debug.LogError("Terminal command data for 'login' not loaded in!");
            }
        }

        protected override IEnumerator ExecuteCoroutine()
        {
            terminal.PrintOutput(_originalString, "");
            terminal.HideUser();
            terminal.PrintOutput("",_data.outputs["startup"], false);
            yield return terminal.StartCoroutine(terminal.LoadingCoroutine(2.0f));
            terminal.UpdateUser(terminal.Settings.UserString);
            terminal.PrintOutput("", _data.outputs["ascii"], false);
            yield return terminal.StartCoroutine(terminal.LoadingCoroutine(1.0f));
            terminal.PrintOutput("", _data.outputs["helloworld"], false);
            terminal.ClearLastCommand();
            yield return null;
            terminal.ShowUser();
        }
    }
}