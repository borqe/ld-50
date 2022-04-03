using System.Collections.Generic;
using UnityEngine;

namespace ConsoleInput
{
    public static class TerminalCommandData
    {
        public static TerminalCommands Data = new TerminalCommands();

        public static void ReadFromFile()
        {
            var jsonTextFile = Resources.Load<TextAsset>("Commands/commandJson");
            TerminalCommands Data = JsonUtility.FromJson<TerminalCommands>(jsonTextFile.text);
        }
    }
    
    [System.Serializable]
    public class CommandData
    {
        public string name;
        
        /// <summary>
        /// parameter > output according to that parameter
        /// </summary>
        public Dictionary<string, string> outputs = new Dictionary<string, string>();
    }

    [System.Serializable]
    public class TerminalCommands
    {
        public Dictionary<string, CommandData> Data = new Dictionary<string, CommandData>();
    }
}