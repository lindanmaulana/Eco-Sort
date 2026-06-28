using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePause: MonoBehaviour
{
    public void HanldeRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void HandleBack()
    {
        if (AppGameManager.instance != null)
        {
            if (!AppGameManager.instance.isGameOver)
            {
                AppGameManager.instance.TriggerGameOver();
            }
        } else
        {
            Debug.LogError("AppGameManager instance tidak ditemukan di scene ini!");
        }
    }
}