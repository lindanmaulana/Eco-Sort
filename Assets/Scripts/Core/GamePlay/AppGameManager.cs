using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AppGameManager: MonoBehaviour
{
    [Header("Titik Lokasi Tong")]
    public Transform posOrganic;
    public Transform posInorganic;
    public Transform posB3;

    [Header("Master Prefab")]
    public GameObject baseBinPrefab; 

    [Header("Data Yang Dipakai (Equipped)")]
    // Ini nanti diisi dengan ScriptableObject tong yang dipilih user
    private TrashBinData currentOrganicData;
    private TrashBinData currentInorganicData;
    private TrashBinData currentB3Data;


    [Header("Statistik Sementara Level Ini")]
    public int coins;
    public List<GarbageData> garbageHistory = new List<GarbageData>();


    [Header("Sistem Nyawa")]
    public AppHeartsUI HeartsUI;
    public int maxHearts = 300;      
    public int currentHearts;      
    public bool isGameOver = false;

    void Start()
    {
        currentHearts = maxHearts;
        if (HeartsUI != null)
        {
            HeartsUI.UpdateHeartsVisuals(currentHearts, maxHearts);
        }

        LoadAndSpawn();
    }

    void LoadAndSpawn()
    {        
        string organicName = PlayerPrefs.GetString("User_Equipped_Organic", "BinStarterOrganik");
        string inorganicName = PlayerPrefs.GetString("User_Equipped_Anorganic", "BinStarterAnorganik");
        string b3Name = PlayerPrefs.GetString("User_Equipped_B3", "BinStarterB3");

        currentOrganicData = Resources.Load<TrashBinData>("TrashBins/" + organicName);
        currentInorganicData = Resources.Load<TrashBinData>("TrashBins/" + inorganicName);
        currentB3Data = Resources.Load<TrashBinData>("TrashBins/" + b3Name);
        
        // Cek darurat kalau filenya gak ketemu
        if (currentOrganicData == null || currentInorganicData == null || currentB3Data == null) Debug.LogError("Data Tong tidak ditemukan di Resources/TrashBins!");

        SetupBin(posOrganic, currentOrganicData, EcoGarbageCategory.Organic);
        SetupBin(posInorganic, currentInorganicData, EcoGarbageCategory.Inorganic);
        SetupBin(posB3, currentB3Data, EcoGarbageCategory.B3);
    }

    void SetupBin(Transform spawnPos, TrashBinData data, EcoGarbageCategory type)
    {
        if(data == null)
        {
            Debug.LogError($"[AppGameManager] Gagal load data untuk {type}. Cek nama file di Resources/TrashBins!");
            return;
        }

        GameObject bin = Instantiate(baseBinPrefab, spawnPos.position, Quaternion.identity);
        bin.name = "Tong_" + type;
        
        // 1. Ganti gambar tong sesuai data
        SpriteRenderer sr = bin.GetComponent<SpriteRenderer>();
        if (sr != null) sr.sprite = data.binIcon;
        
        // 2. Set tipe tong di script logic (TrashBin)
        if (bin.TryGetComponent<TrashBin>(out TrashBin binScript))
        {
            binScript.binType = type;
            binScript.binData = data;

            binScript.currentLevel = AppInventoryManager.instance.GetBinLevel(data.binName);
            binScript.InitializeBin();
        }
    }

    public void RecordGarbageEntry(GarbageData enteredGarbage)
    {
        garbageHistory.Add(enteredGarbage);

        coins += enteredGarbage.scorePoint;

        Debug.Log("Successfully recorded: " + enteredGarbage.garbageName);

        Debug.Log($"--- RIWAYAT SAMPAH TERBARU (Total: {garbageHistory.Count}) ---");
        
        for (int i = 0; i < garbageHistory.Count; i++)
        {
            // Kita print nomor urut, nama sampah, dan tipenya
            Debug.Log($"[{i + 1}] {garbageHistory[i].garbageName} ({garbageHistory[i].type})");
        }
        
        Debug.Log("------------------------------------------------");
    }

    public void RecordWrongEntry(GarbageData wrongGarbage)
    {
        if (isGameOver) return;

        currentHearts -= wrongGarbage.penaltyPoint;

        Debug.LogWarning($"[PENALTI] {wrongGarbage.garbageName} salah masuk! " + $"-{wrongGarbage.penaltyPoint} Nyawa. Sisa: {currentHearts}/{maxHearts}");

        if (HeartsUI != null)
        {
            HeartsUI.UpdateHeartsVisuals(currentHearts, maxHearts);    
        }

        if (currentHearts <= 0)
        {
            currentHearts = 0;
            isGameOver = true;
            Debug.LogError("GAME OVER! Nyawa kamu sudah habis!");
            // TriggerGameOver();
        }
    }
}