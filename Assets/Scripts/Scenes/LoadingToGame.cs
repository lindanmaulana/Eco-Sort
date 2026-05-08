using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingToGame : MonoBehaviour
{
    public float delayTime = 3.0f;
    public string targetSceneName = "MainMenu";

    void Start()
    {
        Invoke("LoadTargetScene", delayTime);
    }

    void LoadTargetScene()
    {
        SceneManager.LoadScene(targetSceneName);
    }
}
