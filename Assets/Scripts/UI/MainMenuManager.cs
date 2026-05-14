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
        if (AppInventoryManager.instance != null && coinText != null)
        {
            int currentCoin = AppInventoryManager.instance.totalCoins;
            
            coinText.text = currentCoin.ToString();
        }
        else
        {
            Debug.LogWarning("Manager atau CoinText belum terpasang!");
        }
    }

    public void OpenSetting()
    {
        SceneManager.LoadScene("Setting");
    }

    public void HandleToShop()
    {
        SceneManager.LoadScene("Shop");
    }
}
