using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AppCardTrash : MonoBehaviour 
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI txtGarbageName; 
    [SerializeField] private Image imgGarbageIcon;        

    private GarbageData garbageData;

    public void InitializeCard(GarbageData data)
    {
        garbageData = data;

        if (txtGarbageName != null) 
        {
            if (data != null && !string.IsNullOrEmpty(data.garbageName))
            {
                txtGarbageName.text = data.garbageName; 
            }
            else
            {
                txtGarbageName.text = "Tanpa Nama"; 
            }
        }

        if (imgGarbageIcon != null && data != null) 
        {
            imgGarbageIcon.sprite = data.garbageIcon;
        }
    }
}