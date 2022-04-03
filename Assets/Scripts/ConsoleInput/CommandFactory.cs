using UnityEngine;

namespace Terminal.Commands
{
    public static class CommandFactory
    {
        public static CommandBase GetCommand(string commandName, TerminalWindow terminal)
        {
            switch (commandName)
            {
                case "start":
                    return new StartCommand(terminal);
                default:
                    return new UndefinedCommand(terminal);
            }
        }
    }
}