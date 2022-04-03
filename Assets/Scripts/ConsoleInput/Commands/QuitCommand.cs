using System;
using System.Collections;
using ConsoleInput;
using UnityEngine;

namespace Terminal.Commands
{
    public class QuitCommand: CommandBase
    {
        public QuitCommand(string originalInput, TerminalWindow terminal) : base(originalInput, terminal)
        {
            if (TerminalCommandData.Commands.Data.ContainsKey("quit"))
            {
                _data = TerminalCommandData.Commands.Data["quit"];
            }
            else
            {
                Debug.LogError("Terminal command data for 'quit' not loaded in!");
            }
        }

        protected override IEnumerator ExecuteCoroutine()
        {
            _lastResponse = "";
            terminal.PrintOutput(_originalString, _data.outputs["default"]);

            while (true)
            {
                yield return new WaitUntil(() => !String.IsNullOrEmpty(_lastResponse));
                terminal.HideUser();
                
                if (_lastResponse.ToLower() == Response.No)
                {
                    terminal.PrintOutput(_lastResponse, "");
                    terminal.PrintOutput("", _data.outputs["n"], false);
                    terminal.ShowUser();
                    terminal.ClearLastCommand();
                    yield break;
                }
                else if (_lastResponse.ToLower() == Response.Yes)
                {
                    terminal.PrintOutput(_lastResponse, "");
                    terminal.PrintOutput("", "Time until shutdown: 5", false);
                    yield return new WaitForSeconds(1.0f);
                    terminal.PrintOutput("", "Time until shutdown: 4", false);
                    yield return new WaitForSeconds(1.0f);
                    terminal.PrintOutput("", "Time until shutdown: 3", false);
                    yield return new WaitForSeconds(1.0f);
                    terminal.PrintOutput("", "Time until shutdown: 2", false);
                    yield return new WaitForSeconds(1.0f);
                    terminal.PrintOutput("", "Time until shutdown: 1", false );
                    yield return new WaitForSeconds(1.0f);
                    terminal.PrintOutput("", "Session terminated...", false);
                    yield return new WaitForSeconds(2.0f);
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
                    Application.OpenURL(webplayerQuitURL);
#else
                    Application.Quit();
#endif
                }
                else
                {
                    terminal.PrintOutput(_lastResponse, "");
                    terminal.PrintOutput("", _data.outputs["bad_response"], false);
                    terminal.ShowUser();
                }

                _lastResponse = null;
                yield return null;
            }
            
        }
    }
}