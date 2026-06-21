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

            // ================= BARIS LOG TAMBAHAN KAMU =================
            if (garbageMasterData != null)
            {
                Debug.Log($"<color=lime>[Master Data Load]</color> Sukses memuat <color=yellow>{garbageMasterData.Count}</color> jenis data sampah dari Master Data.");
            }
            else
            {
                Debug.LogError("<color=red>[Master Data Error]</color> List garbageMasterData bernilai NULL di Inventory Manager!");
            }
        } 
        else
        {
            Destroy(gameObject);
        }
    }


    public void LoadInventory()
    {
        LoadDataCoins();

        userEquippedOrganic = PlayerPrefs.GetString(DataKeyPlayerPrefs.EQUIP_ORGANIC, "so");
        userEquippedAnorganic = PlayerPrefs.GetString(DataKeyPlayerPrefs.EQUIP_INORGANIC, "sao");
        userEequippedB3 = PlayerPrefs.GetString(DataKeyPlayerPrefs.EQUIP_B3, "sb3");

        if (!PlayerPrefs.HasKey(DataKeyPlayerPrefs.INVENTORY_SAVED)) {
            playerInventory.Clear();

            AddBinToPlayerInventory("so");
            AddBinToPlayerInventory("sao");
            AddBinToPlayerInventory("sb3");

            PlayerPrefs.SetString(DataKeyPlayerPrefs.EQUIP_ORGANIC, "so");
            PlayerPrefs.SetString(DataKeyPlayerPrefs.EQUIP_INORGANIC, "sao");
            PlayerPrefs.SetString(DataKeyPlayerPrefs.EQUIP_B3, "sb3");
            
            PlayerPrefs.SetInt(DataKeyPlayerPrefs.INVENTORY_SAVED, 1);
            SaveInventory();
        } else
        {
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



    public void EquipBin(string binID) {
        TrashBinData data = GetDataFromMaster(binID);
        
        if (data == null)
        {
            Debug.LogError($"Gagal pasang! Data Master untuk ID: {binID} tidak ditemukan.");
            return;
        }

        if (playerInventory.Exists(b => b.binID == binID))
        {
            switch (data.type)
            {
                case EcoGarbageCategory.Organic:
                    userEquippedOrganic = binID;
                    PlayerPrefs.SetString(DataKeyPlayerPrefs.EQUIP_ORGANIC, userEquippedOrganic);
                    break;
                case EcoGarbageCategory.Inorganic:
                    userEquippedAnorganic = binID;
                    PlayerPrefs.SetString(DataKeyPlayerPrefs.EQUIP_INORGANIC, userEquippedAnorganic);
                    break;
                case EcoGarbageCategory.B3:
                    userEequippedB3 = binID;
                    PlayerPrefs.SetString(DataKeyPlayerPrefs.EQUIP_B3, userEequippedB3);
                    break;
            }

            PlayerPrefs.Save();
            Debug.Log($"Berhasil memasang {binID} ke slot {data.type}");
        }
    }

    public bool CheckIfEquipped(TrashBinData binData)
    {
        if (binData == null) return false;

        return binData.type switch
        {
            EcoGarbageCategory.Organic => userEquippedOrganic == binData.binID,
            EcoGarbageCategory.Inorganic => userEquippedAnorganic == binData.binID,
            EcoGarbageCategory.B3 => userEequippedB3 == binData.binID,
            _ => false
        };
    }

    public void UpgradeOwnedBin(string binID)
    {
        OwnedBin binToUpgrade = playerInventory.Find(b => b.binID == binID);

        if (binToUpgrade != null)
        {
            TrashBinData masterData = GetDataFromMaster(binID);

            if (masterData != null && binToUpgrade.currentLevel < masterData.maxLevel)
            {
                binToUpgrade.currentLevel++;
                SaveInventory();
                Debug.Log($"{binID} sekarang level {binToUpgrade.currentLevel}");
            } else
            {
                Debug.Log("Sudah mencapai level maksimal!");
            }
        }
        else
        {
            Debug.LogError($"Gagal Upgrade! Tong dengan ID {binID} tidak ditemukan di inventory pemain.");
        }
    }
    public void AddBinToPlayerInventory(string binID)
    {
        OwnedBin checkBin = playerInventory.Find(b => b.binID == binID);

        if (checkBin == null)
        {
            OwnedBin newBin = new()
            {
                binID = binID,
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
    public TrashBinData GetDataFromMaster(string binID)
    {
        return trashBinMasterData.Find(bin => bin.binID == binID);
    }

    public int GetBinLevel(string binID)
    {
        OwnedBin bin = playerInventory.Find(b => b.binID == binID);
        return (bin != null) ? bin.currentLevel : 1;
    }



    public void LoadDataCoins()
    {
        totalCoins = PlayerPrefs.GetInt(DataKeyPlayerPrefs.USER_COINS, DataKeyPlayerPrefs.USER_COINS_DEFAULT);
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0) return;
        totalCoins += amount;

        SaveInventory();
    }

    public bool SpendCoins(int amount)
    {
        if (amount <= 0) return false;
        if (totalCoins < amount) return false;

        totalCoins -= amount;
        SaveInventory();
        return true;
    }
}


[System.Serializable]
public class OwnedBin
{
    public string binID;   
    public int currentLevel;
}

[System.Serializable]
public class InventoryWrapper {
    public List<OwnedBin> items;
}