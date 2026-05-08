using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class NavigationManager: MonoBehaviour
{
    [Tooltip("Ketik nama scene tujuan di sini")]
    public string targetScene;
    [Tooltip("Ketik nama scene tujuan di sini")]
    public string backScene;
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            HandleBack();
        }
    }

    public void HandleToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void HandleBack()
    {
        SceneManager.LoadScene(backScene);
    }
}
