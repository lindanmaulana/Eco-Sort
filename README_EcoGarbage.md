# Eco Garbage Collector - Progress Report

## 1. Sistem Arsitektur Data
Proyek ini menggunakan pendekatan **Data-Driven** dengan memisahkan antara Master Data, Save Data, dan Game Logic.

### A. Master Data (ScriptableObject)
- **File:** `TrashBinData.cs`
- **Fungsi:** Menyimpan template statistik tong (Base Capacity, Bonus per Level, Icon, Tipe).
- **Logika Kapasitas:** Menggunakan fungsi `GetTotalCapacity(level)` untuk menghitung kapasitas dinamis.
  - *Rumus:* `baseCapacity + (capacityBonusPerLevel * (currentLevel - 1))`

### B. Save Data & Inventory (JSON + PlayerPrefs)
- **File:** `AppInventoryManager.cs`
- **Fungsi:** - Mengelola koin (`totalCoins`).
  - Menyimpan daftar tong yang dimiliki (`playerInventory`) dalam bentuk List of `OwnedBin`.
  - Level setiap tong disimpan di dalam objek `OwnedBin` dan diserialisasi ke JSON.
  - Menyimpan status tong yang sedang dipakai (`Equipped`).

### C. Game Manager & Spawning
- **File:** `AppGameManager.cs`
- **Fungsi:** - Membaca data `Equipped` dari PlayerPrefs.
  - Melakukan `Resources.Load` untuk mengambil ScriptableObject yang sesuai.
  - Spawn prefab tong dan melakukan "Dependency Injection" (mengirimkan data SO dan Level ke script `TrashBin`).

### D. Game Logic (Object Behavior)
- **File:** `TrashBin.cs`
- **Fungsi:** - Menerima data dari GameManager.
  - Menghitung `calculatedMaxCapacity` saat inisialisasi.
  - Mengatur Slider UI dan Text Kapasitas secara dinamis.
  - Validasi kecocokan sampah (`EcoGarbageCategory`).

---

## 2. Alur Data (Data Flow)
1. **Load:** `AppInventoryManager` memuat JSON dari PlayerPrefs -> List `OwnedBin` (Nama & Level).
2. **Setup:** `AppGameManager` spawn tong -> Ambil Level dari `AppInventoryManager` -> Kirim ke `TrashBin`.
3. **Init:** `TrashBin` memanggil `InitializeBin()` -> Menghitung Kapasitas Maksimal dari Rumus SO.
4. **Action:** Sampah masuk -> `AddProgress()` -> Slider update berdasarkan `calculatedMaxCapacity`.

---

## 3. Status Terakhir (Latest Fixes)
- [x] **NullReference Fix:** Memastikan `binData` diisi oleh GameManager sebelum `TrashBin` melakukan perhitungan.
- [x] **Type Casting Fix:** Memperbaiki pengecekan null pada objek di C# (menggunakan `if (obj != null)` bukan `if (obj)`).
- [x] **Dynamic Key Fix:** Menggunakan `binData.binName` sebagai kunci unik di Inventory untuk mendukung berbagai versi tong (V1, V2, V3).
- [x] **Initialization:** Menggunakan fungsi `InitializeBin()` manual untuk menghindari *race condition* pada `Start()`.

## 4. Rencana Selanjutnya (Next Steps)
- Implementasi sistem **Upgrade UI** yang memotong koin dan menaikkan level di `AppInventoryManager`.
- Penambahan pinalti jika salah memasukkan sampah ke tong.
- Sistem JSON untuk menyimpan progres inventori yang lebih kompleks.

---
*Dokumen ini dibuat untuk memudahkan sinkronisasi progress pengembangan sistem Eco Garbage Collector.*
