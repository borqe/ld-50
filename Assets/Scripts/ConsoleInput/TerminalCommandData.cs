using System.Collections.Generic;
using UnityEngine;

namespace ConsoleInput
{
    public static class TerminalCommandData
    {
        public static TerminalCommandStorage Data = new TerminalCommandStorage();

        public static void ReadFromFile()
        {
            var jsonTextFile = Resources.Load<TextAsset>("Commands/commandJson");
            TerminalCommandStorage Data = JsonUtility.FromJson<TerminalCommandStorage>(jsonTextFile.text);
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
    public class TerminalCommandStorage
    {
        public Dictionary<string, CommandData> Data = new Dictionary<string, CommandData>();
    }
}