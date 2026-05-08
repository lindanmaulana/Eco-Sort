using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager: MonoBehaviour
{
    public void OpenSetting()
    {
        SceneManager.LoadScene("Setting");
    }

    public void HandleToShop()
    {
        SceneManager.LoadScene("Shop");
    }
}
