using System.Collections;

namespace Terminal.Commands
{
    public class StartCommand: CommandBase
    {
        public StartCommand(string originalInput, TerminalWindow terminal) : base(originalInput, terminal)
        {
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