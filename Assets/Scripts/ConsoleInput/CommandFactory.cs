using UnityEngine;

namespace ConsoleInput.Commands
{
    public static class CommandFactory
    {
        public static CommandBase GetCommand(string commandName, ConsoleWindow terminal)
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