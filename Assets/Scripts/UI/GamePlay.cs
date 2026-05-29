using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GamePlay: MonoBehaviour
{
    [Header("Ui Components")]
    public Image backgroundGameplay;

    [Header("Trash Bin")]
    public Image tbOrganik;
    public Image tbAnOrganik;
    public Image tbB3;

    [Header("Progress Bars")]
    public Slider barTbOrganik;
    public Slider barTbAnOrganik;
    public Slider barTbB3;


    [Header("Text Mash Pro Bar")]
    public TMP_Text txtTbBarOrganik;
    public TMP_Text txtTbBarAnOrganik;
    public TMP_Text txtTbBarB3;

    public void HandleChangeScene(string nameScene)
    {
        Time.timeScale = 1f;
        Debug.Log("Tombol diklik! Mencoba berpindah ke scene: " + nameScene);
        SceneManager.LoadScene(nameScene);
    }
}
