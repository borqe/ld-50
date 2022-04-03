using System.Collections;

namespace Terminal.Commands
{
    public class StartCommand: CommandBase
    {
        public StartCommand(TerminalWindow terminal) : base(terminal)
        {
        }

        protected override IEnumerator ExecuteCoroutine()
        {
            new SetGameStateEvent(GameState.GameStateEnum.InProgress).Broadcast();
            yield return null;
        }
    }
}