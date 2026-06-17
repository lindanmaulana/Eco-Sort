using UnityEngine;
using UnityEngine.UI;
using TMPro;    

public class TrashBinCard : MonoBehaviour
{
    [Header("UI References")]
    public Image imgItem;
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtCategory;
    public TextMeshProUGUI txtTotal; 
    public Slider sliderLevel;       
    public TextMeshProUGUI txtSliderTotal;
    public Button btnUpgrade;
    public TextMeshProUGUI txtCost;


    [Header("Equip Indicator Setup")]
    public Image imgIsEquip;

    private string currentBinID;
    private System.Action<string> onUpgradeClicked;

    public void SetupCard(TrashBinData data, int currentLevel, int cost, System.Action<string> onUpgradeClickAction)
    {
        currentBinID = data.binID;
        onUpgradeClicked = onUpgradeClickAction;

        imgItem.sprite = data.binIcon;
        txtName.text = data.binName;
        txtCategory.text = data.type.ToString();
        txtCost.text = cost.ToString();


        if (imgIsEquip != null)
        {
            if (AppInventoryManager.instance != null)
            {
                bool isCurrentlyEquipped = AppInventoryManager.instance.CheckIfEquipped(data);
                
                imgIsEquip.enabled = isCurrentlyEquipped; 
            }
            else
            {
                imgIsEquip.enabled = false; 
            }
        }


        float currentCap = data.GetTotalCapacity(currentLevel);
        
        if (currentLevel >= data.maxLevel)
        {
            txtTotal.text = $"{currentCap} Item (MAX)";
            txtCost.text = "MAX";
            btnUpgrade.interactable = false; 
        }
        else
        {
            float nextCap = data.GetTotalCapacity(currentLevel + 1);
            txtTotal.text = $"{currentCap} Item > {nextCap} Item";
            btnUpgrade.interactable = true;
        }

        if (sliderLevel != null)
        {
            sliderLevel.maxValue = data.maxLevel;
            sliderLevel.value = currentLevel;
        }

        if (txtSliderTotal != null)
        {
            txtSliderTotal.text = $"Lv {currentLevel} / {data.maxLevel}";
        }

        btnUpgrade.onClick.RemoveAllListeners();
        btnUpgrade.onClick.AddListener(() => onUpgradeClicked?.Invoke(currentBinID));
    }
}