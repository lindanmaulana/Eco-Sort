using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class AppUIManager : MonoBehaviour
{
    public static AppUIManager instance;

    [Header("UI Panels")]
    [SerializeField] private GameObject panelPause;
    [SerializeField] private GameObject panelGameWin;
    [SerializeField] private GameObject panelGameOver;

    public bool IsPaused { get; private set; } = false;

    private void Awake()
    {
        // Singleton pattern agar mudah dipanggil dari AppGameManager
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelGameOver != null && panelGameOver.activeSelf) return;
            if (panelGameWin != null && panelGameWin.activeSelf) return;
            
            TogglePause();
        }
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;
        panelPause.SetActive(IsPaused);
        
        // Menggunakan transisi waktu yang aman untuk New Input System
        Time.timeScale = IsPaused ? 0f : 1f; 
    }

    public void ShowGameOver()
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
}