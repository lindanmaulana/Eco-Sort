using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrashBin : MonoBehaviour
{
    // Ini untuk menentukan tong ini jenis apa (Organik/Anorganik/B3)
    // Nilainya akan diisi otomatis oleh AppGameManager saat game mulai
    public TrashBinData binData;
    public int currentLevel = 1;
    public EcoGarbageCategory binType; 

    [Header("UI Settings")]
    public Slider capacityBar;
    public TextMeshProUGUI capacityText;
    private float currentAmount = 0f;
    private float calculatedMaxCapacity;

    void Start()
    {
        if (binData)
        {
            calculatedMaxCapacity = binData.GetTotalCapacity(currentLevel);

            UpdateUI();
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
                Debug.Log("BENAR! Membuang: " + data.garbageName);

                AddProgress(10f);
            }
            else
            {
                Debug.Log("SALAH! " + data.garbageName + " bukan di sini!");
                // Tambah pinalti di sini nanti
            }

            Destroy(other.gameObject);
        }
    }

    void AddProgress(float amount)
    {
        currentAmount += amount;
        currentAmount = Mathf.Clamp(currentAmount, 0, binData.baseCapacity);

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
            capacityText.text = calculatedMaxCapacity.ToString() + " / " + calculatedMaxCapacity.ToString();
        }
    }

    public void InitializeBin()
    {
        if (binData)
        {
            calculatedMaxCapacity = binData.GetTotalCapacity(currentLevel);

            currentAmount = 0;

            UpdateUI();
            Debug.Log($"Tong {binType} siap! Level: {currentLevel}, Kapasitas: {calculatedMaxCapacity}");
        }
    }
}