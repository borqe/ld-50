using System;
using System.Collections;
using ConsoleInput;
using UnityEngine;

namespace Terminal.Commands
{
    public class ResetCommand: CommandBase
    {
        public ResetCommand(string originalInput, TerminalWindow terminal) : base(originalInput, terminal)
        {
            if (TerminalCommandData.Commands.Data.ContainsKey("reset"))
            {
                _data = TerminalCommandData.Commands.Data["reset"];
            }
            else
            {
                Debug.LogError("Terminal command data for 'reset' not loaded in!");
            }
        }

        protected override IEnumerator ExecuteCoroutine()
        {
            _lastResponse = "";
            terminal.PrintOutput(_originalString, _data.outputs["default"]);
            terminal.PrintOutput("", TerminalCommandData.ConfirmationMessage, false);

            while (true)
            {
                yield return new WaitUntil(() => !String.IsNullOrEmpty(_lastResponse));
                terminal.HideUser();

                if (_lastResponse == Response.No)
                {
                    terminal.PrintOutput(_lastResponse, "");
                    terminal.ShowUser();
                    terminal.ClearLastCommand();
                    yield break;
                }
                else if (_lastResponse == Response.Yes)
                {
                    terminal.PrintOutput(_lastResponse, "");
                    bool continueOutput = false;
                    int randomResponse = UnityEngine.Random.Range(1, 3);
                    string responseKey = "response_" + randomResponse;
                    terminal.PrintOutputTyped(_data.outputs[responseKey], 0.05f,
                    () => {
                        continueOutput = true;
                    });
                    yield return new WaitUntil(() => continueOutput);
                    terminal.PrintOutput("", _data.outputs["reboot"], false);
                    yield return new WaitForSeconds(2.0f);
                    
                    _lastResponse = null;
                    yield return null;
                    
                    new SetGameStateEvent(GameState.GameStateEnum.GameNotStarted).Broadcast();
                    terminal.ClearLastCommand();
                    yield break;
                }
                else
                {
                    terminal.PrintOutput(_lastResponse, "");
                    terminal.PrintOutput("", TerminalCommandData.BadConfirmationResponseMessage, false);
                    terminal.ShowUser();
                    _lastResponse = null;
                }
            }
        }
    }
}