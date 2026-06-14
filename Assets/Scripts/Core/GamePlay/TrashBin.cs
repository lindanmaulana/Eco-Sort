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
                // Debug.Log("SALAH! " + data.garbageName + " bukan di sini!");

                // if (other.TryGetComponent<WasteDraggable>(out WasteDraggable drag)) drag.enabled = false;
                // if (other.TryGetComponent<Collider2D>(out Collider2D col)) col.enabled = false;
                
                // Destroy(other.gameObject);

                // if(gameManager != null)
                // {
                //     gameManager.RecordWrongEntry(data);
                // }

                Debug.Log("SALAH! " + data.garbageName + " bukan di sini!");

                // 1. Matikan fungsi drag agar player dipaksa "melepas" sampah secara sistem
                if (other.TryGetComponent<WasteDraggable>(out WasteDraggable drag)) 
                {
                    drag.enabled = false;
                }

                // 2. Matikan Collider dan Sprite Renderer-nya agar tidak terlihat & tidak bisa diinteraksi
                if (other.TryGetComponent<Collider2D>(out Collider2D col)) col.enabled = false;
                if (other.TryGetComponent<SpriteRenderer>(out SpriteRenderer sprite)) sprite.enabled = false;

                // 3. Jalankan logika pengurangan darah & trigger panel Game Over
                if(gameManager != null)
                {
                    gameManager.RecordWrongEntry(data);
                }

                // 4. FIX MUTLAK: Hancurkan objek secara aman TANPA DELAY waktu, 
                // tapi biarkan Unity menyelesaikannya di akhir siklus frame ini.
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
                    activeBinID = AppInventoryManager.instance.userEequippedB3;
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
        } else
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