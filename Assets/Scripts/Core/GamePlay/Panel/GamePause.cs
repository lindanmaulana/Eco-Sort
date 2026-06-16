using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePause: MonoBehaviour
{
    public void HanldeRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}