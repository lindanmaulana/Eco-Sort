using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI; 

public class Setting : MonoBehaviour
{
    [Header("---- UI Components ----")]
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle sfxToggle;       
    [SerializeField] private Toggle vibrationToggle; 

    [Header("---- Audio Data ----")]
    [SerializeField] private AudioEvent mainMenuMusicEvent;

    public AudioMixer myMixer; 

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
        if (musicToggle != null)
        {
            bool isMusicActiveInGame = PlayerPrefs.GetInt(DataKeyPlayerPrefs.SETTING_MUSIC_BACKGROUND, 1) == 1;
            
            musicToggle.isOn = !isMusicActiveInGame; 
            musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        }

        if (sfxToggle != null)
        {
            bool isSoundActiveInGame = PlayerPrefs.GetInt(DataKeyPlayerPrefs.SETTING_SOUND_BACKGROUND, 1) == 1;
            
            AppSettingManager.isSFXOn = isSoundActiveInGame;

            sfxToggle.isOn = !isSoundActiveInGame;
            sfxToggle.onValueChanged.AddListener(OnSFXToggleChanged);
        }

        if (vibrationToggle != null)
        {
            bool isVibrateActiveInGame = PlayerPrefs.GetInt(DataKeyPlayerPrefs.SETTING_VIBRATE_BACKGROUND, 1) == 1;
            
            AppSettingManager.isVibrationOn = isVibrateActiveInGame;

            vibrationToggle.isOn = !isVibrateActiveInGame;
            vibrationToggle.onValueChanged.AddListener(OnVibrationToggleChanged);
        }

        if (AudioManager.instance != null)
        {
            bool isMusicActive = PlayerPrefs.GetInt(DataKeyPlayerPrefs.SETTING_MUSIC_BACKGROUND, 1) == 1;
            bool isSoundActive = PlayerPrefs.GetInt(DataKeyPlayerPrefs.SETTING_SOUND_BACKGROUND, 1) == 1;

            AudioManager.instance.SetMusicMute(!isMusicActive);
            AudioManager.instance.SetSFXMute(!isSoundActive);
        }
    }

    // private void OnMusicToggleChanged(bool isOn)
    // {
    //     bool isMusicOn = !isOn;

    //     PlayerPrefs.SetInt(DataKeyPlayerPrefs.SETTING_MUSIC_BACKGROUND, isMusicOn ? 1 : 0);
    //     PlayerPrefs.Save();

    //     if (AudioManager.instance == null) return;

    //     if (isMusicOn)
    //     {
    //         if (mainMenuMusicEvent != null && mainMenuMusicEvent.clips.Length > 0)
    //         {
    //             AudioManager.instance.PlayMusic(mainMenuMusicEvent.clips[0]);
    //         }
    //     }
    //     else
    //     {
    //         AudioManager.instance.StopMusic();
    //     }
    // }

    private void OnMusicToggleChanged(bool isOn)
    {
        bool isMusicOn = !isOn;

        PlayerPrefs.SetInt(DataKeyPlayerPrefs.SETTING_MUSIC_BACKGROUND, isMusicOn ? 1 : 0);
        PlayerPrefs.Save();

        if (AudioManager.instance == null) return;

        AudioManager.instance.SetMusicMute(!isMusicOn);

        if (isMusicOn && mainMenuMusicEvent != null && mainMenuMusicEvent.clips.Length > 0)
        {
            AudioManager.instance.PlayMusic(mainMenuMusicEvent.clips[0]);
        }
    }

    private void OnSFXToggleChanged(bool isOn)
    {
        bool isSFXOn = !isOn;

        AppSettingManager.isSFXOn = isSFXOn;
        PlayerPrefs.SetInt(DataKeyPlayerPrefs.SETTING_SOUND_BACKGROUND, isSFXOn ? 1 : 0);
        PlayerPrefs.Save();

        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetSFXMute(!isSFXOn); 
        }
    }

    private void OnVibrationToggleChanged(bool isON)
    {
        bool isVibrationOn = !isON;

        AppSettingManager.isVibrationOn = isVibrationOn;
        PlayerPrefs.SetInt(DataKeyPlayerPrefs.SETTING_VIBRATE_BACKGROUND, isVibrationOn ? 1 : 0);
        PlayerPrefs.Save();

        if (isVibrationOn) 
        {
            Handheld.Vibrate();
        }
    }
}