using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI coinAmount;
    
    [Header("Layout Settings")]
    [SerializeField] private Transform gridAreaParent; 
    [SerializeField] private GameObject cardPrefab;  

    [Header("UI Coin Header (Opsional)")]
    [SerializeField] private TextMeshProUGUI txtTotalCoins; 

    void Start() {
        RenderShopItems();
    }

    public void RenderShopItems()
    {
        if (txtTotalCoins != null) 
            txtTotalCoins.text = AppInventoryManager.instance.totalCoins.ToString();

        foreach (Transform child in gridAreaParent)
        {
            Destroy(child.gameObject);
        }

        List<TrashBinData> masterDatabase = AppInventoryManager.instance.trashBinMasterData;
        foreach (TrashBinData itemData in masterDatabase)
        {
            if (itemData == null || string.IsNullOrEmpty(itemData.binID)) continue;

            var ownedItem = AppInventoryManager.instance.playerInventory.Find(b => b.binID == itemData.binID);
            bool isOwned = (ownedItem != null);
            bool isEquipped = AppInventoryManager.instance.CheckIfEquipped(itemData);
            
            GameObject cardGo = Instantiate(cardPrefab, gridAreaParent);
            AppCardOffer controller = cardGo.GetComponent<AppCardOffer>();

            if (controller != null)
            {
                controller.InitializeCard(itemData, isOwned, isEquipped, () =>
                {
                    OnCardButtonClicked(itemData);
                });
            }
        }
    }

    private void OnCardButtonClicked(TrashBinData data)
    {
        if (data == null) return;

        var ownedItem = AppInventoryManager.instance.playerInventory.Find(b => b.binID == data.binID);
        bool currentOwned = (ownedItem != null); 
        bool currentEquipped = AppInventoryManager.instance.CheckIfEquipped(data);

        if (currentEquipped) return;
        
        if (currentOwned)
        {
            AppInventoryManager.instance.EquipBin(data.binID);
            Debug.Log($"Memasang tong: {data.binID}");
        }
        else
        {
            Debug.Log($"[Shop Debug] Mencoba membeli. Harga: {data.basePrice} | Koin Asli di Script: {AppInventoryManager.instance.totalCoins}");
            bool canBuy = AppInventoryManager.instance.SpendCoins(data.basePrice);

            if (canBuy)
            {
                AppInventoryManager.instance.AddBinToPlayerInventory(data.binID);
                AppInventoryManager.instance.EquipBin(data.binID);
                Debug.Log($"Berhasil membeli tong: {data.binID}!");
            }
            else
            {
                Debug.Log("Gagal membeli! Koin kamu tidak cukup.");
            }
        }

        RenderShopItems();
    }
}