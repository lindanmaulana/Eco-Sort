using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingToGame : MonoBehaviour
{
    [Header("UI References")]
    public Slider loadingSlider;

    [Header("Settings")]
    public string sceneToLoad;

    [Header("Audio Settings (Inisialisasi Awal)")]
    [SerializeField] private AudioEvent defaultBackgroundMusic;

       void Start()
    {
        if (loadingSlider != null)
        {
            loadingSlider.value = 0;
        }

        StartCoroutine(LoadSceneAsync());
    }


    IEnumerator LoadSceneAsync()
    {
        yield return null; 

        while (AppInventoryManager.instance == null)
        {
            Debug.Log("Menunggu AppInventoryManager mendaftarkan diri...");
            yield return null;
        }

        InitGameAudio();

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            if (loadingSlider != null)
            {
                loadingSlider.value = progress;
            }

            yield return null;
        }
    }

    private void InitGameAudio()
    {
        if (AudioManager.instance == null) return;

        bool isMusicOn = PlayerPrefs.GetInt(DataKeyPlayerPrefs.SETTING_MUSIC_BACKGROUND, 1) == 1;
        bool isSoundOn = PlayerPrefs.GetInt(DataKeyPlayerPrefs.SETTING_SOUND_BACKGROUND, 1) == 1;

        if (defaultBackgroundMusic != null && defaultBackgroundMusic.clips.Length > 0)
        {
            AudioManager.instance.PlayMusic(defaultBackgroundMusic.clips[0]);
        }

        AudioManager.instance.SetMusicMute(!isMusicOn);
        AudioManager.instance.SetSFXMute(!isSoundOn);

        Debug.Log($"[Loading Audio] Sinkronisasi Selesai. Musik Mute: {!isMusicOn}, SFX Mute: {!isSoundOn}");
    }
}
