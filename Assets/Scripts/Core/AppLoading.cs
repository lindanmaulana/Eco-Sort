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
        yield return null; 

        while (AppInventoryManager.instance == null)
        {
            Debug.Log("Menunggu AppInventoryManager mendaftarkan diri...");
            yield return null;
        }

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
}
