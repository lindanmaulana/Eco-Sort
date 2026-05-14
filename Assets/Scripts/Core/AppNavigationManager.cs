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
        SceneManager.LoadScene("MainMenu");
    }

    public void HandleBack()
    {
        SceneManager.LoadScene(backScene);
    }

    public void HandleChangeScene(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }

    IEnumerator LoadScene(string sceneName)
    {
        if (transition != null) {
            transition.SetTrigger("Start"); 
        }

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadSceneAsync(sceneName);
    }
}
