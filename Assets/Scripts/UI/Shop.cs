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
            // Ambil status kepemilikan barang lewat playerInventory bawaan managermu
            bool isOwned = AppInventoryManager.instance.playerInventory.Exists(b => b.binName == itemData.binName);
            
            // Cek status kecocokan equip berdasarkan kategori tipenya masing-masing
            bool isEquipped = false;
            switch (itemData.type)
            {
                case EcoGarbageCategory.Organic:
                    isEquipped = AppInventoryManager.instance.userEquippedOrganic == itemData.binName;
                    break;
                case EcoGarbageCategory.Inorganic:
                    isEquipped = AppInventoryManager.instance.userEquippedAnorganic == itemData.binName;
                    break;
                case EcoGarbageCategory.B3:
                    isEquipped = AppInventoryManager.instance.userEequippedB3 == itemData.binName;
                    break;
            }

            // Buat objek kartu baru di dalam Grid Layout Group
            GameObject cardGo = Instantiate(cardPrefab, gridAreaParent);
            AppCardOffer controller = cardGo.GetComponent<AppCardOffer>();

            if (controller != null)
            {
                // Daftarkan data ke komponen kartu beserta logika tombol kliknya
                controller.InitializeCard(itemData, isOwned, isEquipped, () =>
                {
                    OnCardButtonClicked(itemData, isOwned, isEquipped);
                });
            }
        }
    }

    private void OnCardButtonClicked(TrashBinData data, bool isOwned, bool isEquipped)
    {
        if (isEquipped) return; // Pengaman ganda

        if (isOwned)
        {
            // Jika sudah punya, langsung panggil fungsi pasang bawaan skrip managermu
            AppInventoryManager.instance.EquipBin(data.binName);
            Debug.Log($"Memasang tong: {data.binName}");
        }
        else
        {
            // Jika belum punya, panggil transaksi potong koin dari fungsi SpendCoins milikmu
            bool canBuy = AppInventoryManager.instance.SpendCoins(data.basePrice);

            if (canBuy)
            {
                // Jika koin cukup dan berhasil dipotong, masukkan barang ke inventory permanen
                AppInventoryManager.instance.AddBinToPlayerInventory(data.binName);
                Debug.Log($"Berhasil membeli tong: {data.binName}!");
            }
            else
            {
                Debug.Log("Gagal membeli! Koin kamu tidak cukup.");
            }
        }

        // 3. Gambar ulang seluruh item toko agar teks status tombol langsung berganti seketika
        RenderShopItems();
    }
}
