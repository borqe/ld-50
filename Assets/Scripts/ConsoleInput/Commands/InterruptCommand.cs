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
            terminal.PrintOutput(_originalString, _data.outputs["default"]);
            terminal.ClearLastCommand();
            yield return null;
        }
    }
}