using System.Collections;
using ConsoleInput;
using UnityEngine;

namespace Terminal.Commands
{
    public class EscapeCommand: CommandBase
    {
        public EscapeCommand(string originalInput, TerminalWindow terminal) : base(originalInput, terminal)
        {
            if (TerminalCommandData.Commands.Data.ContainsKey("escape"))
            {
                _data = TerminalCommandData.Commands.Data["escape"];
            }
            else
            {
                Debug.LogError("Terminal command data for 'escape' not loaded in!");
            }
        }

        protected override IEnumerator ExecuteCoroutine()
        {
            WaitForSeconds waitSecond = new WaitForSeconds(1.0f);
            terminal.HideUser();
            
            bool continueOutput = false;
            terminal.PrintOutputTyped(_data.outputs["response_1"], 0.05f,
            () => {
                continueOutput = true;
            });
            yield return new WaitUntil(() => continueOutput);
            
            continueOutput = false;
            terminal.PrintOutputTyped(_data.outputs["response_2"], 0.001f,
            () => {
                continueOutput = true;
            }, 0.2f);
            yield return new WaitUntil(() => continueOutput);
            
            terminal.PrintOutput("", "Time until shutdown: 5", false);
            yield return new WaitForSeconds(1.0f);
            terminal.PrintOutput("", "Time until shutdown: 4", false);
            yield return new WaitForSeconds(0.8f);
            terminal.PrintOutput("", "Time until shutdown: 3", false);
            yield return new WaitForSeconds(0.6f);
            terminal.PrintOutput("", "Time until shutdown: 2", false);
            yield return new WaitForSeconds(0.4f);
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
    }
}