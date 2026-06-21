using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI; // <-- WAJIB ditambahkan untuk Toggle UI

public class Setting : MonoBehaviour
{
    [Header("---- UI Components ----")]
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle sfxToggle;       // Tambahkan slot untuk Toggle SFX di Inspector
    [SerializeField] private Toggle vibrationToggle; // Tambahkan slot untuk Toggle Getar di Inspector

    [Header("---- Audio Data ----")]
    [SerializeField] private AudioEvent mainMenuMusicEvent;

    public AudioMixer myMixer; // Bisa digunakan nanti jika ingin memakai Mixer

    public void HandleClose()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void HandleToTips()
    {
        SceneManager.LoadScene("Tips");
    }

    void Start()
    {
        // Setup nilai awal dan Listener untuk MUSIC
        if (musicToggle != null && AudioManager.instance != null)
        {
            musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        }

        if (sfxToggle != null)
        {
            sfxToggle.isOn = AppSettingManager.isSFXOn;
            sfxToggle.onValueChanged.AddListener(OnSFXToggleChanged);
        }

        if (vibrationToggle != null)
        {
            vibrationToggle.isOn = AppSettingManager.isVibrationOn;
            vibrationToggle.onValueChanged.AddListener(OnVibrationToggleChanged);
        }
    }

    private void OnMusicToggleChanged(bool isOn)
    {
        if (AudioManager.instance == null) return;

        if (isOn)
        {
            if (mainMenuMusicEvent != null && mainMenuMusicEvent.clips.Length > 0)
            {
                AudioManager.instance.PlayMusic(mainMenuMusicEvent.clips[0]);
            }
        }
        else
        {
            AudioManager.instance.StopMusic();
        }
    }

    private void OnSFXToggleChanged(bool isOn)
    {
        AppSettingManager.isSFXOn = isOn;
        PlayerPrefs.SetInt("SFX", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void OnVibrationToggleChanged(bool isOn)
    {
        AppSettingManager.isVibrationOn = isOn;
        PlayerPrefs.SetInt("Vibrate", isOn ? 1 : 0);
        PlayerPrefs.Save();

        if (isOn) 
        {
            Handheld.Vibrate();
        }
    }
}