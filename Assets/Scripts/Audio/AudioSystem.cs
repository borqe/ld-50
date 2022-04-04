using UnityEngine;

namespace Audio
{
    public class AudioSystem: Singleton<AudioSystem>
    {
        [SerializeField] private AudioSource _aiSource;
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioFiles _files;

        private void OnEnable()
        {
            _musicSource.clip = _files.Music;
            _musicSource.loop = true;
            _musicSource.playOnAwake = true;
            _musicSource.Play();
        }

        public void PlayVoiceLine(string line)
        {
            _aiSource.Stop();
            if (_files.VoiceLines.ContainsKey(line))
            {
                _aiSource.clip = _files.VoiceLines[line];
            }
            _aiSource.Play();
        }
    }
}