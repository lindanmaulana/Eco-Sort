using UnityEngine;

public class SceneAudioInitializer : MonoBehaviour
{
    [Header("---- Scene Audio Setup ----")]
    [SerializeField] private AudioEvent backgroundMusicEvent;

    void Start()
    {
        // Pastikan AudioManager sehat dan ada di scene
        if (AudioManager.instance != null && backgroundMusicEvent != null)
        {
            // Ambil clip pertama dari AudioEvent untuk diputar di MusicSource
            if (backgroundMusicEvent.clips.Length > 0)
            {
                AudioClip musicClip = backgroundMusicEvent.clips[0];
                AudioManager.instance.PlayMusic(musicClip);
            }
        }
    }
}