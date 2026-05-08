using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AppInventoryManager: MonoBehaviour
{
    public static AppInventoryManager instance;

    public int totalCoins;
    public List<string> ownedItems = new List<string>();

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


    public void AddCoins(int amount)
    {
        totalCoins += amount;
        SaveInventory();
    }

    public bool IsItemOwned(string itemName)
    {
        return ownedItems.Contains(itemName);
    }

    public void SaveInventory()
    {
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        PlayerPrefs.Save();
    }

    void LoadInventory()
    {
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
    }
}
