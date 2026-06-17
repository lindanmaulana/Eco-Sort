using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class NavigationManager: MonoBehaviour
{
    [Tooltip("Ketik nama scene tujuan di sini")]
    public string backScene;

    public Animator transition;
    public float transitionTime = 1f;
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            HandleBack();
        }
    }

    public void HandleToMainMenu()
    {
        HandleChangeScene("MainMenu");
    }

    public void HandleBack()
    {
        if (!string.IsNullOrEmpty(backScene))
        {
            HandleChangeScene(backScene);
        }
        else
        {
            Debug.LogWarning("[NavigationManager] Nama backScene belum diisi di Inspector!");
        }
    }

    public void HandleChangeScene(string sceneName)
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    IEnumerator LoadSceneRoutine(string sceneName)
    {
        if (transition != null) {
            transition.SetTrigger("Start"); 
        }

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadSceneAsync(sceneName);
    }
}
