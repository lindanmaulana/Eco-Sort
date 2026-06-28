using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class AppGameManager : MonoBehaviour
{
    public static AppGameManager instance;

    [Header("Titik Lokasi Tong")]
    public Transform posOrganic;
    public Transform posInorganic;
    public Transform posB3;

    [Header("Master Prefab")]
    public GameObject baseBinPrefab; 

    [Header("Data Yang Dipakai (Equipped)")]
    private TrashBinData currentOrganicData;
    private TrashBinData currentInorganicData;
    private TrashBinData currentB3Data;

    [Header("Statistik Sementara Level Ini")]
    public int coins;
    public int currentScoreLevel;
    public List<GarbageData> garbageHistory = new List<GarbageData>();

    [Header("Sistem Nyawa")]
    public AppHeartsUI HeartsUI;
    public int maxHearts = 300;      
    public int currentHearts;      
    public bool isGameOver = false;

    [Header("Game Economy Settings")]
    [Tooltip("Persentase skor yang diubah jadi koin. Contoh: 0.5f berarti koin adalah 50% dari skor.")]
    [SerializeField] private float coinConversionRate = 0.5f;
    public static event Action<int, int> OnGameplayDataChanged;

    [Header("Audio End Game Settings")]
    [SerializeField] private AudioEvent winSFX;
    [SerializeField] private AudioEvent gameOverSFX;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } 
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {        
        currentHearts = maxHearts;
        if (HeartsUI != null)
        {
            HeartsUI.UpdateHeartsVisuals(currentHearts, maxHearts);
        }

        coins = 0;
        currentScoreLevel = 0;

        LoadAndSpawn();
    }

    void LoadAndSpawn()
    {        
        string organicName = PlayerPrefs.GetString("User_Equipped_Organic", "so");
        string inorganicName = PlayerPrefs.GetString("User_Equipped_Anorganic", "sao");
        string b3Name = PlayerPrefs.GetString("User_Equipped_B3", "sb3");

        if (AppInventoryManager.instance == null)
        {
            Debug.LogError("[AppGameManager] AppInventoryManager.instance belum siap di scene!");
            return;
        }

        string organicID = AppInventoryManager.instance.userEquippedOrganic;
        string inorganicID = AppInventoryManager.instance.userEquippedAnorganic;
        string b3ID = AppInventoryManager.instance.userEquippedB3;

        currentOrganicData = AppInventoryManager.instance.GetDataFromMaster(organicID);
        currentInorganicData = AppInventoryManager.instance.GetDataFromMaster(inorganicID);
        currentB3Data = AppInventoryManager.instance.GetDataFromMaster(b3ID);
        
        if (currentOrganicData == null || currentInorganicData == null || currentB3Data == null) 
        {
            Debug.LogError("Data Tong tidak ditemukan di Resources/TrashBins!");
            return;
        }

        SetupBin(posOrganic, currentOrganicData, EcoGarbageCategory.Organic, organicID);
        SetupBin(posInorganic, currentInorganicData, EcoGarbageCategory.Inorganic, inorganicID);
        SetupBin(posB3, currentB3Data, EcoGarbageCategory.B3, b3ID);
    }

    void SetupBin(Transform spawnPos, TrashBinData data, EcoGarbageCategory type, string activeBinID)
    {
        if(data == null) return;
        if (baseBinPrefab == null) return;

        GameObject bin = Instantiate(baseBinPrefab, spawnPos.position, Quaternion.identity);
        bin.name = "Tong_" + type;
        
        SpriteRenderer sr = bin.GetComponent<SpriteRenderer>();
        if (sr != null) 
        {
            sr.sprite = data.binIcon;
        }
        
        if (bin.TryGetComponent<TrashBin>(out TrashBin binScript))
        {
            binScript.binType = type;
            binScript.binData = data;

            var ownedItem = AppInventoryManager.instance.playerInventory.Find(b => b.binID == activeBinID);
            binScript.currentLevel = (ownedItem != null) ? ownedItem.currentLevel : 1;

            binScript.InitializeBin();
        }
    }

    public void RecordGarbageEntry(GarbageData enteredGarbage)
    {
        garbageHistory.Add(enteredGarbage);

        currentScoreLevel += enteredGarbage.scorePoint;
        int coinGarbage = Mathf.CeilToInt(enteredGarbage.scorePoint * coinConversionRate);
        coins += coinGarbage;
        OnGameplayDataChanged?.Invoke(currentScoreLevel, coins);

        Debug.Log("Successfully recorded: " + enteredGarbage.garbageName);
    }

    public void RecordWrongEntry(GarbageData wrongGarbage)
    {
        if (isGameOver) return;

        currentHearts -= wrongGarbage.penaltyPoint;

        if (HeartsUI != null)
        {
            HeartsUI.UpdateHeartsVisuals(currentHearts, maxHearts);    
        }

        if (currentHearts <= 0)
        {
            currentHearts = 0;
            TriggerGameOver(); // Nyawa habis tetap langsung GameOver
        }
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        CleanUpSceneGarbage();
        CalculateLevelSummary(out int organic, out int inorganic, out int b3);

        PlayEndGameSound(gameOverSFX);

        if (AppUIManager.instance != null)
        {
            AppUIManager.instance.ShowGameOver(organic, inorganic, b3, currentScoreLevel, coins);
            ClaimReward();
        }
    }

    // DISINI PERUBAHAN UTAMANYA: Dipanggil khusus saat waktu 4 menit HABIS
    public void CheckWinConditionOnTimeOut()
    {
        if (isGameOver) return;
        isGameOver = true;

        CleanUpSceneGarbage();
        CalculateLevelSummary(out int organic, out int inorganic, out int b3);

        // Cari semua tong sampah yang ada di map
        TrashBin[] allBins = FindObjectsByType<TrashBin>(FindObjectsInactive.Exclude);
        bool isAllBinsFull = true;

        // Cek satu per satu apakah ada tong yang belum penuh
        foreach (TrashBin bin in allBins)
        {
            if (!bin.IsBinFull())
            {
                isAllBinsFull = false;
                break;
            }
        }

        // KEPUTUSAN AKHIR:
        if (isAllBinsFull && allBins.Length > 0)
        {
            // Jika waktu habis DAN semua tong penuh -> WIN!
            Debug.Log("[GameManager] Waktu habis dan semua tong PENUH! Player Menang.");

            PlayEndGameSound(gameOverSFX);
            if (AppUIManager.instance != null)
            {
                AppUIManager.instance.ShowGameWin(organic, inorganic, b3, currentScoreLevel, coins);
                ClaimReward();
            }
        }
        else
        {
            // Jika waktu habis TAPI ada tong yang belum penuh -> GAMEOVER!
            Debug.Log("[GameManager] Waktu habis tapi ada tong BELUM penuh! Player Kalah.");

            PlayEndGameSound(gameOverSFX);
            if (AppUIManager.instance != null)
            {
                AppUIManager.instance.ShowGameOver(organic, inorganic, b3, currentScoreLevel, coins);
                ClaimReward();
            }
        }
    }

    private void PlayEndGameSound(AudioEvent endAudioEvent)
    {
        bool isSFXOn = PlayerPrefs.GetInt(DataKeyPlayerPrefs.SETTING_SOUND_BACKGROUND, 1) == 1;
        if (!isSFXOn) return;

        if (AudioManager.instance != null)
        {
            AudioManager.instance.StopMusic();
            
            AudioManager.instance.PlaySFX(endAudioEvent);
        }
    }

    private void CleanUpSceneGarbage()
    {
        try 
        {
            GarbageSpawner spawner = GameObject.FindAnyObjectByType<GarbageSpawner>();
            if (spawner != null) spawner.enabled = false;

            WasteDraggable[] remainingGarbagess = GameObject.FindObjectsByType<WasteDraggable>(FindObjectsInactive.Exclude);
            foreach (WasteDraggable garbage in remainingGarbagess)
            {
                if (garbage.TryGetComponent<Collider2D>(out Collider2D col)) col.enabled = false;
                Destroy(garbage.gameObject);
            }
        }
        catch (System.Exception e) { Debug.LogError("Error pembersihan scene: " + e.Message); }
    }

    public void ClaimReward()
    {
        if (coins > 0)
        {
            if (AppInventoryManager.instance != null)
            {
                AppInventoryManager.instance.AddCoins(coins);
            }
            coins = 0; 
        }
    }

    private void CalculateLevelSummary(out int o, out int a, out int b3)
    {
        o = 0; a = 0; b3 = 0;
        foreach (GarbageData garbage in garbageHistory)
        {
            if (garbage.type == EcoGarbageCategory.Organic) o++;
            else if (garbage.type == EcoGarbageCategory.Inorganic) a++;
            else if (garbage.type == EcoGarbageCategory.B3) b3++;
        }
    }
}