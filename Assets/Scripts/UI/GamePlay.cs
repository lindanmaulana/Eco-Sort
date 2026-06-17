using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GamePlay: MonoBehaviour
{
    public static GamePlay Instance;
    [Header("Ui Components")]
    public Image backgroundGameplay;
    public TextMeshProUGUI minuteText;
    public TextMeshProUGUI secondText;

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

    [HideInInspector] public float gameElapsedTime = 0f;

    void Awake()
    {
        // Inisialisasi Singleton
        if (Instance == null) { Instance = this; }
    }
    void Update()
    {
        gameElapsedTime += Time.deltaTime;

        int menit = Mathf.FloorToInt(gameElapsedTime / 60f); 
        int detik = Mathf.FloorToInt(gameElapsedTime % 60f);


        if (secondText != null)
        {
            secondText.text = detik.ToString();
        }

        if (minuteText != null)
        {
            minuteText.text = menit.ToString() + " Min";
        }
    }

    public void HandleChangeScene(string nameScene)
    {
        Debug.Log("Tombol diklik! Mencoba berpindah ke scene: " + nameScene);
        
        Time.timeScale = 1f;
        SceneManager.LoadScene(nameScene);
    }

}
