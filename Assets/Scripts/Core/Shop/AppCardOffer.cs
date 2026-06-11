using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AppCardOffer: MonoBehaviour {
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI txtVersi;       
    [SerializeField] private Image imgTong;                 
    [SerializeField] private TextMeshProUGUI txtHargaAtauStatus; 
    [SerializeField] private Image imgCoinIcon;             
    [SerializeField] private Button btnAksi;

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
        btnAksi.onClick.AddListener(() => onCardClickedCallback?.Invoke());

        if (isEquipped)
        {
            txtHargaAtauStatus.text = "EQUIPPED";
            if (imgCoinIcon != null) imgCoinIcon.gameObject.SetActive(false); 
            btnAksi.interactable = false; 
        }
        else if (isOwned)
        {
            txtHargaAtauStatus.text = "EQUIP";
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