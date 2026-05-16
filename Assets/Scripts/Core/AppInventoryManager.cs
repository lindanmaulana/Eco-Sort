using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AppInventoryManager: MonoBehaviour
{
    public static AppInventoryManager instance;

   [Header("Systems - Master Data")]
    public List<TrashBinData> trashBinMasterData;
    public List<GarbageData> garbageMasterData;


    [Header("User Data")]
    public int totalCoins;
    public List<OwnedBin> playerInventory = new List<OwnedBin>();


    [Header("Equipped Bins")]
    public string userEquippedOrganic;
    public string userEquippedAnorganic;
    public string userEequippedB3;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadInventory();
        } 
        else
        {
            Destroy(gameObject);
        }
    }


    public void LoadInventory()
    {
        LoadDataCoins();

        userEquippedOrganic = PlayerPrefs.GetString(DataKeyPlayerPrefs.EQUIP_ORGANIC, "BinStarterOrganik");
        userEquippedAnorganic = PlayerPrefs.GetString(DataKeyPlayerPrefs.EQUIP_INORGANIC, "BinStarterAnorganik");
        userEequippedB3 = PlayerPrefs.GetString(DataKeyPlayerPrefs.EQUIP_B3, "BinStarterB3");

        if (!PlayerPrefs.HasKey(DataKeyPlayerPrefs.INVENTORY_SAVED)) {
            playerInventory.Clear();

            AddBinToPlayerInventory("BinStarterOrganik");
            AddBinToPlayerInventory("BinStarterAnorganik");
            AddBinToPlayerInventory("BinStarterB3");

            PlayerPrefs.SetString(DataKeyPlayerPrefs.EQUIP_ORGANIC, "BinStarterOrganik");
            PlayerPrefs.SetString(DataKeyPlayerPrefs.EQUIP_INORGANIC, "BinStarterAnorganik");
            PlayerPrefs.SetString(DataKeyPlayerPrefs.EQUIP_B3, "BinStarterB3");
            
            PlayerPrefs.SetInt(DataKeyPlayerPrefs.INVENTORY_SAVED, 1);
            SaveInventory();
        } else
        {
            // JIKA SUDAH ADA DATA (Pemain Lama):
            // Di sini nanti kita pakai JSON untuk muat list playerInventory
            // (Tapi untuk tes sekarang, biarkan list diisi manual atau lewat fungsi Buy)

            if (PlayerPrefs.HasKey(DataKeyPlayerPrefs.INVENTORY_DATA))
            {
                string json = PlayerPrefs.GetString(DataKeyPlayerPrefs.INVENTORY_DATA);
                InventoryWrapper wrapper = JsonUtility.FromJson<InventoryWrapper>(json);

                if (wrapper != null && wrapper.items != null)
                {
                    playerInventory = wrapper.items;
                }
            }

        }
    }

    public void SaveInventory()
    {
        InventoryWrapper wrapper = new InventoryWrapper();

        wrapper.items = playerInventory;
        string json = JsonUtility.ToJson(wrapper);

        PlayerPrefs.SetString(DataKeyPlayerPrefs.INVENTORY_DATA, json);
        PlayerPrefs.SetInt(DataKeyPlayerPrefs.USER_COINS, totalCoins);
        PlayerPrefs.Save();
    }

    public void EquipBin(string binNameToEquip) {
        TrashBinData data = GetDataFromMaster(binNameToEquip);


        if (playerInventory.Exists(b => b.binName == binNameToEquip))
        {
            switch (data.type)
            {
                case EcoGarbageCategory.Organic:
                    userEquippedOrganic = binNameToEquip;
                    break;
                case EcoGarbageCategory.Inorganic:
                    userEquippedAnorganic = binNameToEquip;
                    break;
                case EcoGarbageCategory.B3:
                    userEequippedB3 = binNameToEquip;
                    break;
            }


            // Jangan lupa simpan status equip ke PlayerPrefs
            PlayerPrefs.SetString(DataKeyPlayerPrefs.EQUIP_ORGANIC, userEquippedOrganic);
            PlayerPrefs.SetString(DataKeyPlayerPrefs.EQUIP_INORGANIC, userEquippedAnorganic);
            PlayerPrefs.SetString(DataKeyPlayerPrefs.EQUIP_B3, userEequippedB3);
            PlayerPrefs.Save();
            
            Debug.Log($"Berhasil memasang {binNameToEquip} ke slot {data.type}");
        }
    }


    public void UpgradeOwnedBin(string binName)
    {
        OwnedBin binToUpgrade = playerInventory.Find(b => b.binName == binName);

        if (binToUpgrade != null)
        {
            TrashBinData masterData = GetDataFromMaster(binName);

            if (binToUpgrade.currentLevel < masterData.maxLevel)
            {
                binToUpgrade.currentLevel++;

                SaveInventory();
                Debug.Log($"{binName} sekarang level {binToUpgrade.currentLevel}");
            } else
            {
                Debug.Log("Sudah mencapai level maksimal!");
            }
        }
    }

    // Fungsi untuk menambah barang baru ke daftar milik user
    public void AddBinToPlayerInventory(string name)
    {
        OwnedBin checkBin = playerInventory.Find(b => b.binName == name);

        if (checkBin == null)
        {
            OwnedBin newBin = new()
            {
                binName = name,
                currentLevel = 1,
            };

            playerInventory.Add(newBin);
            SaveInventory(); 
        }
        else
        {
            Debug.Log("User sudah punya barang ini!");
        }
    }

    public TrashBinData GetDataFromMaster(string name)
    {
        return trashBinMasterData.Find(bin => bin.binName == name);
    }

    public int GetBinLevel(string name)
    {
        OwnedBin bin = playerInventory.Find(b => b.binName == name);

        return (bin != null) ? bin.currentLevel : 1;
    }

    public void LoadDataCoins()
    {
        totalCoins = PlayerPrefs.GetInt(DataKeyPlayerPrefs.USER_COINS, DataKeyPlayerPrefs.USER_COINS_DEFAULT);
    }
}


[System.Serializable]
public class OwnedBin
{
    public string binName;   
    public int currentLevel;
}

[System.Serializable]
public class InventoryWrapper {
    public List<OwnedBin> items;
}