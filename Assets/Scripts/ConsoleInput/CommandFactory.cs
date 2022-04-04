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
                    return new StartCommand(commandName, terminal);
                case "clear":
                    return new ClearCommand(commandName, terminal);
                case "help":
                    return new HelpCommand(commandName, terminal);
                case "reset":
                    return new ResetCommand(commandName, terminal);
                case "startup":
                    return new StartupCommand(commandName, terminal);
                case "login":
                    return new LoginCommand(commandName, terminal);
                case "quit":
                    return new QuitCommand(commandName, terminal);
                case "escape":
                    return new EscapeCommand(commandName, terminal);
                case "interrupt":
                    return new InterruptCommand(commandName, terminal);
                default:
                    return new UndefinedCommand(commandName, terminal);
            }
        }
    }
}