using UnityEngine;

public class GarbageSpawner: MonoBehaviour
{
    public GameObject[] garbagePrefabs; // Tempat menaruh prefab biru tadi
    public float spawnInterval = 1f;    // Muncul setiap 2 detik
    public Vector2 spawnRangeX = new Vector2(-7f, 7f); // Jarak kiri-kanan
    public Vector2 spawnRangeY = new Vector2(-4f, 4f); // Jarak atas-bawah

    void Start()
    {
        // Menjalankan fungsi Spawn setiap beberapa detik
        InvokeRepeating("SpawnGarbage", 1f, spawnInterval);
    }

    void SpawnGarbage()
    {
        if (garbagePrefabs.Length == 0) return;

        // 1. Pilih sampah secara acak dari list
        int randomIndex = Random.Range(0, garbagePrefabs.Length);
        
        // 2. Tentukan posisi acak di dalam layar
        Vector2 spawnPos = new Vector2(Random.Range(spawnRangeX.x, spawnRangeX.y), Random.Range(spawnRangeY.x, spawnRangeY.y));

        // 3. Munculkan sampah
        GameObject go = Instantiate(garbagePrefabs[randomIndex], spawnPos, Quaternion.identity);

        // Coba mulai dari 0.4f (40% dari ukuran asli). 
        // Kalau masih kegedean, ganti jadi 0.3f. Kalau kekecilan, ganti jadi 0.6f.
        go.transform.localScale = new Vector3(0.4f, 0.4f, 1f);

        // 4. Beri gerakan acak awal agar sampah tidak diam
        Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 randomForce = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            rb.AddForce(randomForce, ForceMode2D.Impulse);
        }
    }
}
