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

    // Fungsi untuk memutar musik background (Looping)
    public void PlayMusic(AudioClip musicClip)
    {
        if (musicClip == null) return;
        
        if (musicSource.clip == musicClip && musicSource.isPlaying) return;

        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    // Fungsi untuk mematikan musik
    public void StopMusic()
    {
        musicSource.Stop();
    }

    // Fungsi pusat untuk menyalakan SFX menggunakan data dari AudioEvent
    public void PlaySFX(AudioEvent audioEvent)
    {
        if (audioEvent == null) return;
        
        // Kita perintahkan si AudioEvent untuk bunyi menggunakan SFX Source kita
        audioEvent.Play(sfxSource);
    }
}