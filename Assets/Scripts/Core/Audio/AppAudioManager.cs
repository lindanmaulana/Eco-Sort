using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("---- Audio Sources ----")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip musicClip)
    {
        if (musicClip == null) return;
        
        if (musicSource.clip == musicClip && musicSource.isPlaying) return;

        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(AudioEvent audioEvent)
    {
        if (audioEvent == null) return;
        
        audioEvent.Play(sfxSource);
    }

    public void SetMusicMute(bool isMute)
    {
        musicSource.mute = isMute;
    }

    public void SetSFXMute(bool isMute)
    {
        sfxSource.mute = isMute;
    }
}