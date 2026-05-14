using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class Setting: MonoBehaviour
{
    public AudioMixer myMixer;
    public void HandleClose()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void HandleToTips()
    {
        SceneManager.LoadScene("Tips");
    }

    public void ToggleVibration()
    {
        AppSettingManager.isVibrationOn = !AppSettingManager.isVibrationOn;
        PlayerPrefs.SetInt("Vibrate", AppSettingManager.isVibrationOn ? 1 : 0);
        PlayerPrefs.Save();

        if (AppSettingManager.isVibrationOn) Handheld.Vibrate();
    }

    public void ToggleMusic()
    {
        AppSettingManager.isMusicOn = !AppSettingManager.isMusicOn;
        PlayerPrefs.SetInt("Music", AppSettingManager.isMusicOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ToggleSFX()
    {
        AppSettingManager.isSFXOn = !AppSettingManager.isSFXOn;
        PlayerPrefs.SetInt("SFX", AppSettingManager.isSFXOn ? 1 : 0);
        PlayerPrefs.Save();
    }
}