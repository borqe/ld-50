using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace ConsoleInput
{
    public static class TerminalCommandData
    {
        private static TerminalCommandStorage commands = new TerminalCommandStorage();
        public static TerminalCommandStorage Commands
        {
            get => commands;
        }

        public static void ReadFromFile()
        {
            var jsonTextFile = Resources.Load<TextAsset>("Commands/commandData");
            commands = JsonConvert.DeserializeObject<TerminalCommandStorage>(jsonTextFile.text);
        }

        public static void WriteToFile()
        {
            TerminalCommandStorage storage = new TerminalCommandStorage();
            CommandData data = new CommandData();
            data.name = "clear";
            data.outputs = new Dictionary<string, string>() {{"key", "value"}};
            
            storage.Data.Add("clear", data);
            File.WriteAllText("Assets/Resources/Commands/commandData.json", JsonConvert.SerializeObject(storage));
        }

        public static string ConfirmationMessage => Commands.Data["confirmation"].outputs["default"];
        public static string BadConfirmationResponseMessage => Commands.Data["confirmation"].outputs["bad_response"];
        public static string BadUserResponseMessage => Commands.Data["user_response"].outputs["bad_response"];
        public static string UnauthorizedMessage => Commands.Data["unauthorized"].outputs["default"];
        public static string InstructionMessage => Commands.Data["instructions"].outputs["default"];
    }
    
    [System.Serializable]
    public class CommandData
    {
        [JsonProperty("name")]
        public string name;
        
        /// <summary>
        /// parameter > output according to that parameter
        /// </summary>
        [JsonProperty("outputs")]
        public Dictionary<string, string> outputs = new Dictionary<string, string>();
    }

    [System.Serializable]
    public class TerminalCommandStorage
    {
        [JsonProperty("data")]
        public Dictionary<string, CommandData> Data = new Dictionary<string, CommandData>();
    }
}