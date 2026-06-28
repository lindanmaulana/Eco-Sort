using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AppCardOffer : MonoBehaviour {
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI txtVersi;       
    [SerializeField] private Image imgTong;                 
    [SerializeField] private TextMeshProUGUI txtHargaAtauStatus; 
    [SerializeField] private Image imgCoinIcon;             
    [SerializeField] private Button btnAksi;

    [Header("Audio Settings")]
    [SerializeField] private AudioEvent soundClickSuccess;
    [SerializeField] private AudioEvent soundClickFailed;

    private TrashBinData tbData;
    private System.Action onCardClickedCallback;

    public void InitializeCard(TrashBinData data, bool isOwned, bool isEquipped, System.Action onClickAction)
    {
        tbData = data;
        onCardClickedCallback = onClickAction;

        if (txtVersi != null) 
        {
            if (data != null && !string.IsNullOrEmpty(data.binName))
            {
                txtVersi.text = data.binName; 
            }
            else
            {
                txtVersi.text = "No Name"; 
            }
        }
        if (imgTong != null) imgTong.sprite = data.binIcon;

        btnAksi.onClick.RemoveAllListeners();
        btnAksi.onClick.AddListener(() => 
        {
            if (AudioManager.instance != null)
            {
                if (isOwned)
                {
                    if (soundClickSuccess != null) AudioManager.instance.PlaySFX(soundClickSuccess);
                }
                else
                {
                    if (AppInventoryManager.instance != null)
                    {
                        if (AppInventoryManager.instance.totalCoins >= data.basePrice)
                        {
                            if (soundClickSuccess != null) AudioManager.instance.PlaySFX(soundClickSuccess);
                        }
                        else
                        {
                            if (soundClickFailed != null) AudioManager.instance.PlaySFX(soundClickFailed);
                        }
                    }
                }
            }

            onCardClickedCallback?.Invoke();
        });

        if (isEquipped)
        {
            txtHargaAtauStatus.text = "USED";
            if (imgCoinIcon != null) imgCoinIcon.gameObject.SetActive(false); 
            btnAksi.interactable = false; 
        }
        else if (isOwned)
        {
            txtHargaAtauStatus.text = "USE";
            if (imgCoinIcon != null) imgCoinIcon.gameObject.SetActive(false); 
            btnAksi.interactable = true;
        }
        else
        {
            txtHargaAtauStatus.text = data.basePrice.ToString();
            if (imgCoinIcon != null) imgCoinIcon.gameObject.SetActive(true); 
            btnAksi.interactable = true;
        }
    }
}