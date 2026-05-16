using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrashBin : MonoBehaviour
{
    // Ini untuk menentukan tong ini jenis apa (Organik/Anorganik/B3)
    // Nilainya akan diisi otomatis oleh AppGameManager saat game mulai
    public EcoGarbageCategory binType; 

    [Header("UI Settings")]
    public Slider capacityBar;
    public TextMeshProUGUI capacityText;
    public float currentAmount = 0f;
    public float maxAmount = 100f;

    void Start()
    {
        UpdateUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("Sesuatu masuk ke area Tong: " + other.name);
        // Cek apakah benda yang masuk punya script GarbageItem
        GarbageItem item = other.GetComponent<GarbageItem>();
        WasteDraggable dragScript = other.GetComponent<WasteDraggable>();

        // 2. Cek apakah yang masuk benar-benar objek sampah
        if (item != null && item.data != null && dragScript != null)
        {
            if (!dragScript.wasDraggedByPlayer)
            {
                Debug.Log("Cuma numpang lewat, jangan ditangkep.");
                return;
            }

            GarbageData data = item.data;

            // 3. Bandingkan: Apakah tipe sampah sama dengan tipe tong ini?
            if (data.type == binType)
            {
                Debug.Log("BENAR! Membuang: " + data.garbageName);
                // Tambah skor di sini nanti

                if (capacityBar)
                {
                    capacityBar.value = currentAmount;
                }
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
        currentAmount = Mathf.Clamp(currentAmount, 0, maxAmount);
        UpdateUI();
    }
    void UpdateUI()
    {
        if (capacityBar)
        {
            capacityBar.maxValue = maxAmount;
            capacityBar.value = currentAmount;
        }

        if (capacityText)
        {
            capacityText.text = currentAmount.ToString() + " / " + maxAmount.ToString();
        }
    }
}