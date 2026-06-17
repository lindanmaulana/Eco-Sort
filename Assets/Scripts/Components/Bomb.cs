using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Bomb Settings")]
    public int damage = 1; // Jumlah nyawa yang dikurangi
    public GameObject explosionEffectPrefab; // [Opsional] Prefab efek ledakan/partikel

    // Fungsi bawaan Unity yang otomatis jalan saat objek ini DIKLIK oleh mouse/jari
    private void OnMouseDown()
    {
        Explode();
    }

    void Explode()
    {
        // 1. MEMBUAT EFEK MELEDAK
        if (explosionEffectPrefab != null)
        {
            // Munculkan efek ledakan di posisi bom saat ini
            GameObject effect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            
            // Hancurkan efek ledakan setelah 1 detik agar tidak memenuhi memori
            Destroy(effect, 1f);
        }

        // 2. MENGURANGI NYAWA PLAYER
        // Di sini kita bisa memanggil fungsi pengurang nyawa.
        // Sementara kita cetak dulu di Console untuk memastikan kodenya bekerja:
        Debug.Log("DUARRR! Bom meledak. Nyawa berkurang: " + damage);
        
        // TODO: Hubungkan dengan UI Nyawa kamu di sini nanti, contoh:
        // GameManager.instance.ReduceLives(damage);

        // 3. HANCURKAN OBJEK BOM
        // Hilangkan bom dari layar karena sudah meledak
        Destroy(gameObject);
    }
}