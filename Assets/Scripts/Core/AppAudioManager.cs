using UnityEngine;

public class AppAudioManager: MonoBehaviour
{
    public static AppAudioManager instance;

    [Header("---- Audio Source ----")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;



    [Header("---- Audio Clips (Daftar Musik) ----")]
    public AudioClip mainMenuMusic;
    public AudioClip gameplayMusic;

    
    [Header("---- Audio Clips (Daftar SFX/Not) ----")]
    public AudioClip coinSFX;
    public AudioClip buttonClickSFX;
    public AudioClip gameOverSFX;
    public AudioClip gameWinSfx;


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

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        sfxSource.PlayOneShot(clip); 
    }
}