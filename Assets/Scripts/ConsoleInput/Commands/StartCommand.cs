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
            terminal.PrintOutput(_originalString, "");
            terminal.HideUser();
            terminal.PrintOutput("", _data.outputs["accessing"], false);
            yield return terminal.StartCoroutine(terminal.LoadingCoroutine(2.0f));
            yield return new WaitForSeconds(1.0f);
            bool continueOutput = false;
            terminal.PrintOutputTyped(_data.outputs["ai_overtaking"], 0.05f,
            () => {
                continueOutput = true;
            });
            yield return new WaitUntil(() => continueOutput);
            yield return terminal.StartCoroutine(terminal.LoadingCoroutine(2.0f));
            
            new SetGameStateEvent(GameState.GameStateEnum.InProgress).Broadcast();
            terminal.ShowUser();
            terminal.ClearLastCommand();
            yield return null;
        }
    }
}