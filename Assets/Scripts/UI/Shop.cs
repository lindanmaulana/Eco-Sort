using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class Shop: MonoBehaviour
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
        // Update jumlah koin di header toko agar terus sinkron saat ada transaksi beli
        if (txtTotalCoins != null) 
            txtTotalCoins.text = AppInventoryManager.instance.totalCoins.ToString();

        // 1. Bersihkan kloningan objek kartu lama agar grid tidak menumpuk
        foreach (Transform child in gridAreaParent)
        {
            Destroy(child.gameObject);
        }

        // 2. Ambil List database master langsung dari AppInventoryManager milikmu
        List<TrashBinData> masterDatabase = AppInventoryManager.instance.trashBinMasterData;
        foreach (TrashBinData itemData in masterDatabase)
        {
            if (itemData == null || string.IsNullOrEmpty(itemData.binID)) continue;

            // Ambil status kepemilikan barang lewat playerInventory bawaan managermu
            bool isOwned = AppInventoryManager.instance.playerInventory.Exists(b => b.binID == itemData.binID);

            // Cek status kecocokan equip berdasarkan kategori tipenya masing-masing    
            bool isEquipped = AppInventoryManager.instance.CheckIfEquipped(itemData);
            GameObject cardGo = Instantiate(cardPrefab, gridAreaParent);
            AppCardOffer controller = cardGo.GetComponent<AppCardOffer>();

            if (controller != null)
            {
                // Daftarkan data ke komponen kartu beserta logika tombol kliknya
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

        bool currentOwned = AppInventoryManager.instance.playerInventory.Exists(b => b.binID == data.binID);
        bool currentEquipped = AppInventoryManager.instance.CheckIfEquipped(data);

        if (currentEquipped) return;
        
        if (currentOwned)
        {
            AppInventoryManager.instance.EquipBin(data.binID);
            Debug.Log($"Memasang tong: {data.binID}");
        }
        else
        {
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
