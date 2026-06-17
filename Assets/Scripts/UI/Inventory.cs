using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory: MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI txtTotalCoins;

    void OnEnable()
    {
        RenderInventoryItems();
    }

    public void RenderInventoryItems()
    {
        if (txtTotalCoins != null && AppInventoryManager.instance != null)
        {
            txtTotalCoins.text = AppInventoryManager.instance.totalCoins.ToString();
        }
    }
}