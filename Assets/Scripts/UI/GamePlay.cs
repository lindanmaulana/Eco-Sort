using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GamePlay : MonoBehaviour
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

    private float totalGameDuration = 240f; 
    private bool isTimerFinished = false;

    private Coroutine bounceCoroutine;
    private Transform timerParentTransform;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
    }

    void Start()
    {
        if (minuteText != null) timerParentTransform = minuteText.transform.parent;
    }

    void Update()
    {
        if (isTimerFinished) return;

        gameElapsedTime += Time.deltaTime;

        float timeRemaining = totalGameDuration - gameElapsedTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isTimerFinished = true;
            TriggerTimeOut(); 
        }

        int totalSecondsCeil = Mathf.CeilToInt(timeRemaining);
        int menit = totalSecondsCeil / 60; 
        int detik = totalSecondsCeil % 60;

        if (secondText != null)
        {
            secondText.text = detik.ToString("00");
        }

        if (minuteText != null)
        {
            minuteText.text = menit.ToString() + " Min";
        }
    }

    private void TriggerTimeOut()
    {
        Debug.Log("[GAMEPLAY] Waktu 4 Menit Habis! Memeriksa kondisi tong...");

        AppGameManager gameManager = GameObject.FindAnyObjectByType<AppGameManager>();
        if (gameManager != null)
        {
            gameManager.CheckWinConditionOnTimeOut(); 
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
                targetColor = new Color(1f, 0.5f, 0f); 
                break;
            default:
                targetColor = Color.red;
                break;
        }

        if (minuteText != null) minuteText.color = targetColor;
        if (secondText != null) secondText.color = targetColor;

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