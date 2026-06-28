using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrashBin : MonoBehaviour
{
    [Header("Manager References")]
    public AppGameManager gameManager;
    public TrashBinData binData;
    public int currentLevel = 1;
    public EcoGarbageCategory binType; 

    [Header("UI Settings")]
    public Slider capacityBar;
    public TextMeshProUGUI capacityText;
    private float currentAmount = 0f;
    private float calculatedMaxCapacity;

    public void Start()
    {
        if (gameManager == null)
        {
            gameManager = FindAnyObjectByType<AppGameManager>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GarbageItem item = other.GetComponent<GarbageItem>();
        WasteDraggable dragScript = other.GetComponent<WasteDraggable>();

        if (item != null && item.data != null && dragScript != null)
        {
            if (!dragScript.wasDraggedByPlayer)
            {
                Debug.Log("Cuma numpang lewat, jangan ditangkep.");
                return;
            }

            GarbageData data = item.data;

            if (data.type == binType)
            {
                if (currentAmount >= calculatedMaxCapacity)
                {
                    Debug.LogWarning($"Tong {binType} sudah PENUH! {data.garbageName} tidak bisa masuk.");
                    Destroy(other.gameObject); 
                    return;
                }

                dragScript.PlayFeedbackSFX(true);
                AddProgress(1f);

                if (gameManager != null)
                {
                    gameManager.RecordGarbageEntry(data);
                    // PERUBAHAN: Baris CheckWinCondition() di sini telah dihapus
                }

                Destroy(other.gameObject);
            }
            else
            {
                Debug.Log("SALAH! " + data.garbageName + " bukan di sini!");
                dragScript.PlayFeedbackSFX(false);

                if (other.TryGetComponent<WasteDraggable>(out WasteDraggable drag)) 
                {
                    drag.enabled = false;
                }

                if (other.TryGetComponent<Collider2D>(out Collider2D col)) col.enabled = false;
                if (other.TryGetComponent<SpriteRenderer>(out SpriteRenderer sprite)) sprite.enabled = false;

                if(gameManager != null)
                {
                    gameManager.RecordWrongEntry(data);
                }

                Destroy(other.gameObject);
            }
        }
    }

    void AddProgress(float amount)
    {
        currentAmount += amount;
        currentAmount = Mathf.Clamp(currentAmount, 0, calculatedMaxCapacity);

        UpdateUI();
    }

    void UpdateUI()
    {
        if (capacityBar)
        {
            capacityBar.maxValue = calculatedMaxCapacity;
            capacityBar.value = currentAmount;
        }

        if (capacityText)
        {
            capacityText.text = Mathf.RoundToInt(currentAmount).ToString() + " / " + calculatedMaxCapacity.ToString();
        }
    }

    public void InitializeBin()
    {
        if (gameManager == null)
        {
            gameManager = FindAnyObjectByType<AppGameManager>();
        }

        if (AppInventoryManager.instance != null)
        {
            string activeBinID = "";

            switch(binType)
            {
                case EcoGarbageCategory.Organic:
                    activeBinID = AppInventoryManager.instance.userEquippedOrganic;
                    break;
                case EcoGarbageCategory.Inorganic:
                    activeBinID = AppInventoryManager.instance.userEquippedAnorganic;
                    break;
                case EcoGarbageCategory.B3:
                    activeBinID = AppInventoryManager.instance.userEquippedB3;
                    break;
            }

            binData = AppInventoryManager.instance.GetDataFromMaster(activeBinID);
            var ownedData = AppInventoryManager.instance.playerInventory.Find(b => b.binID == activeBinID);
            if (ownedData != null)
            {
                currentLevel = ownedData.currentLevel;
            }
            else
            {
                currentLevel = 1;
            }
        } 
        else
        {
            Debug.LogWarning("AppInventoryManager tidak ditemukan! Menggunakan data fallback di Inspector.");
        }

        if (binData)
        {
            calculatedMaxCapacity = binData.GetTotalCapacity(currentLevel);
            currentAmount = 0;

            UpdateUI();
            Debug.Log($"Tong {binType} siap! Level: {currentLevel}, Kapasitas: {calculatedMaxCapacity}");
        }
    }

    public bool IsBinFull()
    {
        return currentAmount >= calculatedMaxCapacity;
    }
}