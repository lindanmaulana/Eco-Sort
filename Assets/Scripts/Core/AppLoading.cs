using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Loading : MonoBehaviour
{
    [Header("UI References")]
    public Slider loadingSlider;

    [Header("Settings")]
    public string sceneToLoad;
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
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            loadingSlider.value = progress;

            yield return null;
        }
    }
}
