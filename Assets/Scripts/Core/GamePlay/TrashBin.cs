using UnityEngine;

public class TrashBin : MonoBehaviour
{
    // Ini untuk menentukan tong ini jenis apa (Organik/Anorganik/B3)
    // Nilainya akan diisi otomatis oleh AppGameManager saat game mulai
    public EcoGarbageCategory binType; 

    private void OnTriggerEnter2D(Collider2D other)
    {
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
            }
            else
            {
                Debug.Log("SALAH! " + data.garbageName + " bukan di sini!");
                // Tambah pinalti di sini nanti
            }

            Destroy(other.gameObject);
        }
    }
}