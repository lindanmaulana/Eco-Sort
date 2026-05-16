using UnityEngine;
using UnityEngine.UI;

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

    void Start()
    {
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
}