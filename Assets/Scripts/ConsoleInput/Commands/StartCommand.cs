using System.Collections;
using ConsoleInput;
using UnityEngine;

namespace Terminal.Commands
{
    public class StartCommand: CommandBase
    {
        public StartCommand(string originalInput, TerminalWindow terminal) : base(originalInput, terminal)
        {
            if (TerminalCommandData.Commands.Data.ContainsKey("start"))
            {
                _data = TerminalCommandData.Commands.Data["start"];
            }
            else
            {
                Debug.LogError("Terminal command data for 'start' not loaded in!");
            }
        }

        protected override IEnumerator ExecuteCoroutine()
        {
            new SetGameStateEvent(GameState.GameStateEnum.InProgress).Broadcast();
            terminal.PrintOutput(_originalString, "");
            terminal.ClearLastCommand();
            yield return null;
        }
    }
}