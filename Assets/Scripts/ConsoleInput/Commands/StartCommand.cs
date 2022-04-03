using System.Collections;

namespace ConsoleInput.Commands
{
    public class StartCommand: CommandBase
    {
        public StartCommand(ConsoleWindow console) : base(console)
        {
        }

        protected override IEnumerator ExecuteCoroutine()
        {
            new SetGameStateEvent(GameState.GameStateEnum.InProgress).Broadcast();
            yield return null;
        }
    }
}