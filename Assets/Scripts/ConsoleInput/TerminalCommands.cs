using System.Collections.Generic;
using UnityEngine;

namespace ConsoleInput
{
    [CreateAssetMenu(fileName = "ConsoleCommands", menuName = "ConsoleCommands", order = 0)]
    public class TerminalCommands : ScriptableObject
    {
        public List<TerminalCommand> commands = new List<TerminalCommand>();
        public Dictionary<string, string> commandDictionary = new Dictionary<string, string>();

        private void OnEnable()
        {
            foreach (var command in commands)
            {
                commandDictionary.Add(command.commandName, command.commandResponse);
            }
        }
    }

    [System.Serializable]
    public class TerminalCommand
    {
        public string commandName;
        [TextArea(3, 15)]
        public string commandResponse;
    }
}