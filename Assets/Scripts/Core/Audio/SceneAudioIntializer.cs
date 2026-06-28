using UnityEngine;

public class SceneAudioInitializer : MonoBehaviour
{
    [Header("---- Scene Audio Setup ----")]
    [SerializeField] private AudioEvent backgroundMusicEvent;

    void Start()
    {
        if (AudioManager.instance != null)
        {
            bool isMusicOn = PlayerPrefs.GetInt(DataKeyPlayerPrefs.SETTING_MUSIC_BACKGROUND, 1) == 1;

            if (isMusicOn)
            {
                if (backgroundMusicEvent != null && backgroundMusicEvent.clips.Length > 0)
                {
                    AudioClip musicClip = backgroundMusicEvent.clips[0];
                    AudioManager.instance.PlayMusic(musicClip);
                }
            }
            else
            {
                AudioManager.instance.StopMusic();
            }
        }
    }
}