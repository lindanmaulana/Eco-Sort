using UnityEngine;
using UnityEngine.SceneManagement;

public class AppSettingManager: MonoBehaviour
{
    public static AppSettingManager instance;
    public static bool isVibrationOn = true;
    public static bool isMusicOn = true;
    public static bool isSFXOn = true;

    void Awake()
    { 
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        CleanUpAudioListeners();
        LoadSettings();
    }

    void CleanUpAudioListeners()
    {
        AudioListener[] allListeners = Object.FindObjectsByType<AudioListener>();

        if (allListeners.Length > 1)
        {
            AudioListener myListener = GetComponent<AudioListener>();
            
            if (myListener != null)
            {
                myListener.enabled = false;
                Debug.Log("Audio Listener pada Manager dimatikan.");
            }
        }
    }

    void LoadSettings()
    {
        isVibrationOn = PlayerPrefs.GetInt("Vibrate", 1) == 1;
        isMusicOn     = PlayerPrefs.GetInt("Music", 1)   == 1;
        isSFXOn       = PlayerPrefs.GetInt("SFX", 1)     == 1;
    }
}

