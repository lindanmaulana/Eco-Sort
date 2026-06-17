using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class MainMenuManager: MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI coinText;

    void Start() {
        Invoke("UpdateCoinDisplay", 0.1f);
    }

    public void UpdateCoinDisplay()
    {
        if (AppInventoryManager.instance == null)
        {
            Debug.LogError("BUG: AppInventoryManager.instance KOSONG/NULL! Apakah managernya sudah di-spawn di scene sebelumnya?");
        }
        
        if (coinText == null)
        {
            Debug.LogError("BUG: UI TextMeshPro (coinText) belum kamu tarik ke dalam slot Inspector di MainMenuManager!");
        }

        if (AppInventoryManager.instance != null && coinText != null)
        {
            int currentCoin = AppInventoryManager.instance.totalCoins;
            
            coinText.text = currentCoin.ToString();
        }
    }

    public void OpenSetting()
    {
        SceneManager.LoadScene("Setting");
    }

    public void HandleToShop()
    {
        SceneManager.LoadScene("ShopV2");
    }
}
