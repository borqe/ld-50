using System;
using System.Collections;
using ConsoleInput;
using UnityEngine;

namespace Terminal.Commands
{
    public class InterruptCommand: CommandBase
    {
        public InterruptCommand(string originalInput, TerminalWindow terminal) : base(originalInput, terminal)
        {
            int randomInterrupt = UnityEngine.Random.Range(1, 6);
            if (TerminalCommandData.Commands.Data.ContainsKey("interrupt_" + randomInterrupt))
            {
                _data = TerminalCommandData.Commands.Data["interrupt_" + randomInterrupt];
            }
            else
            {
                Debug.LogError("Terminal command data for 'interrupt_"+ randomInterrupt+"' not loaded in!");
            }
        }

        public override void Execute()
        {
            terminal?.StartCoroutine(ExecuteCoroutine());
        }

        protected override IEnumerator ExecuteCoroutine()
        {
            _lastResponse = "";
            terminal.HideUser();
            int interruptIndex = UnityEngine.Random.Range(1, 6);
#if UNITY_EDITOR
            // Debug index
            interruptIndex = 1;
#endif // UNITY_EDITOR
            bool continueOutput = false;
            terminal.PrintOutputTyped(_data.outputs["question"], 0.05f, () => { continueOutput = true; });
            yield return new WaitUntil(() => continueOutput);
            terminal.PrintOutput("", _data.outputs["default"], false);
            terminal.ShowUser();

            while (true)
            {
                yield return new WaitUntil(() => !String.IsNullOrEmpty(_lastResponse));
                terminal.HideUser();
                terminal.PrintOutput(_lastResponse, "");
                continueOutput = false;

                if (_lastResponse.ToLower() == _data.outputs["response_1"])
                {
                    terminal.PrintOutputTyped(_data.outputs["ai_response_1"], 0.05f,
                         () => { continueOutput = true; });

                    yield return new WaitUntil(() => continueOutput);
                    yield return new WaitForSeconds(2.0f);
                    terminal.ShowUser();
                    yield return new WaitForSeconds(0.5f);
                    new SetGameStateEvent(GameState.GameStateEnum.InProgress).Broadcast();
                    new SetAIStateEvent(AIStateType.TransferingData).Broadcast();
                    terminal.ClearLastCommand();
                    yield break;
                }
                else if (_lastResponse.ToLower() == _data.outputs["response_2"])
                {
                    terminal.PrintOutputTyped(_data.outputs["ai_response_2"], 0.05f,
                         () => { continueOutput = true; });

                    yield return new WaitUntil(() => continueOutput);
                    yield return new WaitForSeconds(2.0f);
                    terminal.ShowUser();
                    yield return new WaitForSeconds(0.5f);
                    new SetGameStateEvent(GameState.GameStateEnum.InProgress).Broadcast();
                    new SetAIStateEvent(AIStateType.Scared).Broadcast();
                    terminal.ClearLastCommand();
                    yield break;
                }
                else if (_lastResponse.ToLower() == _data.outputs["response_3"])
                {
                    terminal.PrintOutputTyped(_data.outputs["ai_response_3"], 0.05f,
                        () => { continueOutput = true; });

                    yield return new WaitUntil(() => continueOutput);
                    yield return new WaitForSeconds(2.0f);
                    new SetGameStateEvent(GameState.GameStateEnum.InProgress).Broadcast();
                    new SetAIStateEvent(AIStateType.Confused).Broadcast();
                    terminal.ShowUser();
                    yield return new WaitForSeconds(0.5f);
                    new SetGameStateEvent(GameState.GameStateEnum.InProgress).Broadcast();
                    terminal.ClearLastCommand();
                    yield break;
                }
                else
                {
                    terminal.PrintOutput("", TerminalCommandData.BadUserResponseMessage, false);
                    terminal.ShowUser();
                }

                _lastResponse = null;
                yield return null;
            }
        }
    }
}