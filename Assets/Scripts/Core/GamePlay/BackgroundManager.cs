using UnityEngine;

[ExecuteInEditMode] // Biar langsung kelihatan hasilnya di Scene tanpa pencet Play
public class BackgroundManager : MonoBehaviour
{
    void Start()
    {
        ScaleBackgroundToFit();
    }

    // Kalau kamu mau di-update terus saat layar berubah ukuran,
    // ganti void Update() { ScaleBackgroundToFit(); }
    
    void ScaleBackgroundToFit()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        // Reset scale dulu
        transform.localScale = Vector3.one;

        // Ambil ukuran gambar asli
        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        // Hitung ukuran kamera dalam world space
        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        // Hitung rasio scale untuk masing-masing sumbu
        float scaleX = worldScreenWidth / width;
        float scaleY = worldScreenHeight / height;

        // Pilih tipe scaling:
        // 1. FILL / COVER: Mengisi semua layar, gambar mungkin kepotong dikit (Pakai Max)
        // 2. FIT: Seluruh gambar kelihatan, mungkin ada sisa hitam di pinggir (Pakai Min)
        
        float finalScale = Mathf.Max(scaleX, scaleY); // Pilih Fill/Cover agar layar penuh

        transform.localScale = new Vector3(finalScale, finalScale, 1.0f);
    }
}