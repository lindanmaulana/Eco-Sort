using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class AppUIManager : MonoBehaviour
{
    public static AppUIManager instance;

    [Header("UI Panels")]
    [SerializeField] private GameObject panelPause;
    [SerializeField] private GameObject panelGameWin;
    [SerializeField] private GameObject panelGameOver;

    [Header("TextMeshPro References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI coinText;

    [Header("Juice Animation Settings")]
    [SerializeField] private float punchScaleAmount = 1.25f;
    [SerializeField] private float animationSpeed = 12f;

    private Vector3 originalScoreScale;
    private Vector3 originalCoinScale;
    
    private Vector3 targetScoreScale;
    private Vector3 targetCoinScale;

    public bool IsPaused { get; private set; } = false;

    [Header("TextMeshPro Rekapitulasi (Panel Game Over)")]
    [SerializeField] private TextMeshProUGUI textRecapOrganik;
    [SerializeField] private TextMeshProUGUI textRecapAnorganik;
    [SerializeField] private TextMeshProUGUI textRecapB3;
    [SerializeField] private TextMeshProUGUI textRecapTotalTrash;
    [SerializeField] private TextMeshProUGUI textRecapScore;
    [SerializeField] private TextMeshProUGUI textRecapCoinsReward;

    private void Awake()
    {
        // Singleton pattern agar mudah dipanggil dari AppGameManager
        if (instance == null) instance = this;
        else Destroy(gameObject);

        // FIX: Amankan dan isi ukuran asli teks di Awake agar matematika perkalian tidak bernilai 0
        if (scoreText != null) originalScoreScale = scoreText.transform.localScale;
        if (coinText != null) originalCoinScale = coinText.transform.localScale;

        // Set target awal sesuai ukuran aslinya
        targetScoreScale = originalScoreScale;
        targetCoinScale = originalCoinScale;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelGameOver != null && panelGameOver.activeSelf) return;
            if (panelGameWin != null && panelGameWin.activeSelf) return;
            
            TogglePause();
        }

        // FIX LOGIKA BARU: Efek animasi halus mengembalikan ukuran teks ke normal setelah dibusungkan (Punch)
        // Menggunakan Unscaled Delta Time agar animasi teks tetap bergerak halus meskipun game sedang di-pause/freeze
        if (scoreText != null)
        {
            scoreText.transform.localScale = Vector3.Lerp(scoreText.transform.localScale, targetScoreScale, Time.unscaledDeltaTime * animationSpeed);
            if (Vector3.Distance(scoreText.transform.localScale, targetScoreScale) < 0.005f) targetScoreScale = originalScoreScale;
        }

        if (coinText != null)
        {
            coinText.transform.localScale = Vector3.Lerp(coinText.transform.localScale, targetCoinScale, Time.unscaledDeltaTime * animationSpeed);
            if (Vector3.Distance(coinText.transform.localScale, targetCoinScale) < 0.005f) targetCoinScale = originalCoinScale;
        }
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;
        panelPause.SetActive(IsPaused);
        
        // Menggunakan transisi waktu yang aman untuk New Input System
        Time.timeScale = IsPaused ? 0f : 1f; 
    }

    public void ShowGameOver(int organic, int inorganic, int b3, int scoreLevel, int coinsEarned)
    {
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);

            if (EventSystem.current != null)
            {
                Debug.Log("EventSystem berhasil di matikan");
                EventSystem.current.SetSelectedGameObject(null); 
            } else
            {
                Debug.Log("EventSystem gagal di matikan");
            }

            if (textRecapOrganik != null) textRecapOrganik.text = organic + " Item";
            if (textRecapAnorganik != null) textRecapAnorganik.text = inorganic + " Item";
            if (textRecapB3 != null) textRecapB3.text = b3 + " Item";
            
            // HITUNG GRAND TOTAL (Memperbaiki bug matematika 10+10+10 = 30)
            int grandTotal = organic + inorganic + b3;
            if (textRecapTotalTrash != null) textRecapTotalTrash.text = grandTotal + " Item";

            // MASUKKAN DATA SKOR DAN COIN REWARD BARU
            if (textRecapScore != null) textRecapScore.text = scoreLevel + " PTS";
            if (textRecapCoinsReward != null) textRecapCoinsReward.text = "+" + coinsEarned;

            DisableAllGarbageDraggables();

            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Debug.Log("[AppUIManager] Game Over Screen Sukses Aktif & Waktu DIHENTIKAN (0f)!");
            // Opsional: aktifkan Time.timeScale = 0f jika ingin permainan berhenti total saat kalah
        }
    }

    public void ShowGameWin()
    {
        if (panelGameWin != null)
        {
            panelGameWin.SetActive(true);
        }
    }
    // public void ShowGameWin(int organic, int inorganic, int b3, int scoreLevel, int coinsEarned)
    // {
    //     if (panelGameWin != null)
    //     {
    //         panelGameWin.SetActive(true);

    //         if (EventSystem.current != null)
    //         {
    //             EventSystem.current.SetSelectedGameObject(null); 
    //         }

    //         if (winRecapOrganik != null) winRecapOrganik.text = organic + " Item";
    //         if (winRecapAnorganik != null) winRecapAnorganik.text = inorganic + " Item";
    //         if (winRecapB3 != null) winRecapB3.text = b3 + " Item";
            
    //         int grandTotal = organic + inorganic + b3;
    //         if (winRecapTotalTrash != null) winRecapTotalTrash.text = grandTotal + " Item";

    //         if (winRecapScore != null) winRecapScore.text = scoreLevel + " PTS";
    //         if (winRecapCoinsReward != null) winRecapCoinsReward.text = "+" + coinsEarned;

    //         DisableAllGarbageDraggables();

    //         Time.timeScale = 0f;
    //         Cursor.lockState = CursorLockMode.None;
    //         Cursor.visible = true;
            
    //         Debug.Log("[AppUIManager] Game Win Screen Sukses Aktif dengan data rekap dinamis!");
    //     }
    // }

    // --- Fungsionalitas Tombol Panel ---
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    private void DisableAllGarbageDraggables()
    {
        // Cari semua komponen skrip drag sampah yang sedang aktif di scene
        WasteDraggable[] remainingGarbages = GameObject.FindObjectsByType<WasteDraggable>(FindObjectsInactive.Exclude);
        foreach (WasteDraggable garbage in remainingGarbages)
        {
            garbage.enabled = false; // Matikan skripnya secara total
        }
    }

    private void OnEnable()
    {
        AppGameManager.OnGameplayDataChanged += UpdateGameplayUI;
        
        // Reset tampilan angka di awal level
        if (scoreText != null) scoreText.text = "0";
        if (coinText != null) coinText.text = "0";
    }

    private void OnDisable()
    {
        // Lepas langganan agar tidak terjadi kebocoran memori (memory leak)
        AppGameManager.OnGameplayDataChanged -= UpdateGameplayUI;
    }

    // Fungsi pembaca siaran data dari AppGameManager
    private void UpdateGameplayUI(int currentScore, int currentCoins)
    {
        // 1. Cek dan update angka teks Skor jika berubah
        if (scoreText != null && scoreText.text != currentScore.ToString())
        {
            scoreText.text = currentScore.ToString();
            scoreText.transform.localScale = originalScoreScale * punchScaleAmount; // Efek denyut
        }

        // 2. Cek dan update angka teks Koin jika berubah
        if (coinText != null && coinText.text != currentCoins.ToString())
        {
            coinText.text = currentCoins.ToString();
            coinText.transform.localScale = originalCoinScale * punchScaleAmount; // Efek denyut
        }
    }
}