using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrashBin : MonoBehaviour
{
    // Ini untuk menentukan tong ini jenis apa (Organik/Anorganik/B3)
    // Nilainya akan diisi otomatis oleh AppGameManager saat game mulai
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

                AddProgress(1f);

                if (gameManager != null)
                {
                    gameManager.RecordGarbageEntry(data);
                    gameManager.CheckWinCondition();
                }

                Destroy(other.gameObject);
            }
            else
            {
                Debug.Log("SALAH! " + data.garbageName + " bukan di sini!");

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