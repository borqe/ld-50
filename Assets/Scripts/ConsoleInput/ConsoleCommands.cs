using System.Collections.Generic;
using UnityEngine;

namespace ConsoleInput
{
    [CreateAssetMenu(fileName = "ConsoleCommands", menuName = "ConsoleCommands", order = 0)]
    public class ConsoleCommands : ScriptableObject
    {
        public List<ConsoleCommand> commands = new List<ConsoleCommand>();
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
    public class ConsoleCommand
    {
        public string commandName;
        [TextArea(3, 15)]
        public string commandResponse;
    }
}