using System.Collections;

namespace Terminal.Commands
{
    public class RestartCommand: CommandBase
    {
        public RestartCommand(string originalInput, TerminalWindow terminal) : base(originalInput, terminal)
        {
        }

        protected override IEnumerator ExecuteCoroutine()
        {
            yield return null;
        }
    }
}