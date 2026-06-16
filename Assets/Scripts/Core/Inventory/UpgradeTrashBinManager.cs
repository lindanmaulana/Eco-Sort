using UnityEngine;
using System.Collections.Generic;

public class UpgradeTrashBinManager : MonoBehaviour
{
    [Header("Prefab & Container Setup")]
    [SerializeField] private GameObject upgradeCardPrefab;
    [SerializeField] private Transform contentContainer;

    private void OnEnable()
    {
        RenderUpgradeList();
    }

    public void RenderUpgradeList()
    {
        if (contentContainer == null) return;
        
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        if (AppInventoryManager.instance == null)
        {
            Debug.LogError("[UpgradeManager] AppInventoryManager tidak ditemukan!");
            return;
        }

        List<OwnedBin> myInventory = AppInventoryManager.instance.playerInventory;

        foreach (OwnedBin ownedItem in myInventory)
        {
            TrashBinData masterData = AppInventoryManager.instance.GetDataFromMaster(ownedItem.binID);
            
            if (masterData == null) continue;

            int upgradeCost = masterData.GetUpgradeCost(ownedItem.currentLevel);

            GameObject cardGo = Instantiate(upgradeCardPrefab, contentContainer);
            
            TrashBinCard cardUi = cardGo.GetComponent<TrashBinCard>();

            if (cardUi != null)
            {
                cardUi.SetupCard(masterData, ownedItem.currentLevel, upgradeCost, (id) => {
                    TriggerUpgradeProcess(id, upgradeCost);
                });
            }
        }
    }

    private void TriggerUpgradeProcess(string binID, int cost)
    {
        if (AppInventoryManager.instance.totalCoins >= cost)
        {
            AppInventoryManager.instance.SpendCoins(cost);
            AppInventoryManager.instance.UpgradeOwnedBin(binID);

            RenderUpgradeList();
            Debug.Log($"[Upgrade] Berhasil upgrade {binID}. Koin berkurang {cost}");
        }
        else
        {
            Debug.LogWarning("[Upgrade] Koin kamu tidak cukup!");
        }
    }
}