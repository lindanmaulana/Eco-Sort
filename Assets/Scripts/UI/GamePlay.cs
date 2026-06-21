using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

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

    // Tracker Coroutine untuk animasi bounce agar tidak bertabrakan
    private Coroutine bounceCoroutine;
    private Transform timerParentTransform;

    void Awake()
    {
        // Inisialisasi Singleton
        if (Instance == null) { Instance = this; }
    }

    void Start()
    {
        if (minuteText != null) timerParentTransform = minuteText.transform.parent;
    }

    void Update()
    {
        gameElapsedTime += Time.deltaTime;

        int menit = Mathf.FloorToInt(gameElapsedTime / 60f); 
        int detik = Mathf.FloorToInt(gameElapsedTime % 60f);


        if (secondText != null)
        {
            secondText.text = detik.ToString("00");
        }

        if (minuteText != null)
        {
            minuteText.text = menit.ToString() + " Min";
        }
    }

    public void SetDifficultyVisuals(int difficultyLevel)
    {
        Color targetColor = Color.white;

        switch (difficultyLevel)
        {
            case 0:
                targetColor = Color.white;
                break;
            case 1:
                targetColor = Color.yellow;
                break;
            case 2:
                targetColor = new Color(1f, 0.5f, 0f); // Orange
                break;
            default:
                targetColor = Color.red;
                break;
        }

        if (minuteText != null) minuteText.color = targetColor;
        if (secondText != null) secondText.color = targetColor;

        // Picu animasi bounce pada UI Timer
        if (bounceCoroutine != null) StopCoroutine(bounceCoroutine);
        bounceCoroutine = StartCoroutine(BounceTimerRoutine());
    }

    IEnumerator BounceTimerRoutine()
    {
        Transform targetTransform = (timerParentTransform != null) ? timerParentTransform : minuteText.transform;
        
        if (targetTransform == null) yield break;

        Vector3 originalScale = Vector3.one;
        Vector3 targetScale = new Vector3(1.3f, 1.3f, 1.3f);
        
        float duration = 0.25f;
        float elapsed = 0f;

        while (elapsed < duration / 2f)
        {
            elapsed += Time.deltaTime;
            targetTransform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / (duration / 2f));
            yield return null;
        }

        elapsed = 0f;

        while (elapsed < duration / 2f)
        {
            elapsed += Time.deltaTime;
            targetTransform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / (duration / 2f));
            yield return null;
        }

        targetTransform.localScale = originalScale;
    }

    public void HandleChangeScene(string nameScene)
    {
        Debug.Log("Tombol diklik! Mencoba berpindah ke scene: " + nameScene);
        
        Time.timeScale = 1f;
        SceneManager.LoadScene(nameScene);
    }

}
