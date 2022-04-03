using System.Collections;

namespace Terminal.Commands
{
    public class InterruptCommand: CommandBase
    {
        public InterruptCommand(string originalInput, TerminalWindow terminal) : base(originalInput, terminal)
        {
        }

        protected override IEnumerator ExecuteCoroutine()
        {
            yield return null;
        }
    }
}