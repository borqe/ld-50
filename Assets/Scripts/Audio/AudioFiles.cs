using System;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(fileName = "AudioFiles", menuName = "AudioFiles", order = 0)]
    public class AudioFiles : ScriptableObject
    {
        public AudioClip Music;
        public List<VoiceLine> voiceLines = new List<VoiceLine>();
        public Dictionary<string, AudioClip> VoiceLines = new Dictionary<string, AudioClip>();

        private void OnEnable()
        {
            foreach (var line in voiceLines)
            {
                VoiceLines.Add(line.Key, line.Clip);
            }
        }
    }

    [Serializable]
    public class VoiceLine
    {
        public string Key;
        public AudioClip Clip;
    }
}