using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AppInventoryManager: MonoBehaviour
{
    public static AppInventoryManager instance;

   [Header("Systems - Master Data")]
    public List<TrashBinData> trashBinMasterData;


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

    // CUSTOM METHOD Inventory
    public void LoadInventory()
    {
        totalCoins = PlayerPrefs.GetInt("UserCoins", 50);

        userEquippedOrganic = PlayerPrefs.GetString("User_Equipped_Organic", "Organik_Starter");
        userEquippedAnorganic = PlayerPrefs.GetString("User_Equipped_Anorganic", "Anorganik_Starter");
        userEequippedB3 = PlayerPrefs.GetString("User_Equipped_B3", "B3_Starter");

        if (!PlayerPrefs.HasKey("InventorySaved")) {
            playerInventory.Clear();

            AddBinToPlayerInventory("Organik_Starter");
            AddBinToPlayerInventory("Anorganik_Starter");
            AddBinToPlayerInventory("B3_Starter");

            PlayerPrefs.SetString("User_Equipped_Organic", "Organik_Starter");
            PlayerPrefs.SetString("User_Equipped_Anorganic", "Anorganik_Starter");
            PlayerPrefs.SetString("User_Equipped_B3", "B3_Starter");
            
            PlayerPrefs.SetInt("InventorySaved", 1);
            SaveInventory();
        } else
        {
            // JIKA SUDAH ADA DATA (Pemain Lama):
            // Di sini nanti kita pakai JSON untuk muat list playerInventory
            // (Tapi untuk tes sekarang, biarkan list diisi manual atau lewat fungsi Buy)

            string json = PlayerPrefs.GetString("InventoryData");
            InventoryWrapper wrapper = JsonUtility.FromJson<InventoryWrapper>(json);
            playerInventory = wrapper.items;
        }
    }

    public void EquipBin(string binNameToEquip) {
        TrashBinData data = GetDataFromMaster(binNameToEquip);


        if (playerInventory.Exists(b => b.binName == binNameToEquip))
        {
            switch (data.type)
            {
                case TrashBinType.Organic:
                    userEquippedOrganic = binNameToEquip;
                    break;
                case TrashBinType.Inorganic:
                    userEquippedAnorganic = binNameToEquip;
                    break;
                case TrashBinType.B3:
                    userEequippedB3 = binNameToEquip;
                    break;
            }


            // Jangan lupa simpan status equip ke PlayerPrefs
            PlayerPrefs.SetString("User_Equipped_Organic", userEquippedOrganic);
            PlayerPrefs.SetString("User_Equipped_Anorganic", userEquippedAnorganic);
            PlayerPrefs.SetString("User_Equipped_B3", userEequippedB3);
            PlayerPrefs.Save();
            
            Debug.Log($"Berhasil memasang {binNameToEquip} ke slot {data.type}");
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

    public void SaveInventory()
    {
        InventoryWrapper wrapper = new InventoryWrapper();

        wrapper.items = playerInventory;

        string json = JsonUtility.ToJson(wrapper);

        PlayerPrefs.SetString("InventoryData", json);
        PlayerPrefs.SetInt("UserCoins", totalCoins);
        PlayerPrefs.Save();
    }

    public TrashBinData GetDataFromMaster(string name)
    {
        return trashBinMasterData.Find(bin => bin.binName == name);
    }

    public void LoadDataCoins()
    {
        totalCoins = PlayerPrefs.GetInt("UserCoins", 0);
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