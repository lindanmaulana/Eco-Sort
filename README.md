# Eco Garbage Collector - Progress Report

## 1. Sistem Arsitektur Data
Proyek ini menggunakan pendekatan **Data-Driven** dengan memisahkan antara Master Data, Save Data, Gameplay Session Data, dan Game Logic.

### A. Master Data (ScriptableObject)
- **File:** `TrashBinData.cs` & `GarbageData.cs`
- **Fungsi:** - `TrashBinData`: Menyimpan template statistik tong (Base Capacity, Bonus per Level, Icon, Tipe, Max Level).
  - `GarbageData`: Menyimpan identitas sampah (Nama, Icon, Tipe Kategori) beserta Nilai Ekonominya (`scorePoint` & `penaltyPoint`).
- **Logika Kapasitas:** Menggunakan fungsi `GetTotalCapacity(level)` untuk menghitung kapasitas dinamis.
  - *Rumus:* `baseCapacity + (capacityBonusPerLevel * (currentLevel - 1))`

### B. Kunci Penyimpanan Terpusat (Static Class)
- **File:** `DataKeyPlayerPrefs.cs`
- **Fungsi:** Menyimpan seluruh string key `PlayerPrefs` secara global (`public const string`) untuk mencegah kesalahan typo lintas script dan mempermudah manajemen *database* lokal game.

### C. Save Data & Inventory (JSON + PlayerPrefs - Global)
- **File:** `AppInventoryManager.cs` (Persistent Singleton)
- **Fungsi:** - Mengelola koin permanen (`totalCoins`).
  - Menyimpan daftar tong yang dimiliki (`playerInventory`) dalam bentuk List of `OwnedBin` yang diserialisasikan ke JSON via `InventoryWrapper`.
  - Mengatur fungsi transaksional permanen seperti `TryUpgradeBin()` (validasi koin & kenaikan level) dan `EquipBin()`.

### D. Game Session Manager (Gameplay Scene - Temporary)
- **File:** `AppGameManager.cs` (Local Scene Singleton)
- **Fungsi:** - Membaca data `Equipped` dari PlayerPrefs saat scene dimulai dan melakukan *dependency injection* ke objek tong yang di-spawn.
  - **Pencatatan Sementara:** Menyimpan `koinYangDidapatLevelIni` dan struktur data `Dictionary<string, int> riwayatSampahLevelIni` secara lokal di RAM agar otomatis ter-reset saat *restart* scene.
  - Menyediakan fungsi `KlaimHadiahKeInventoryPermanen()` untuk mengirim akumulasi hadiah ke `AppInventoryManager` saat level selesai.

### E. Game Logic (Object Behavior)
- **File:** `TrashBin.cs`
- **Fungsi:** - Menerima cetak biru data dari `AppGameManager` via fungsi `InitializeBin()`.
  - Melakukan enkapsulasi `Start()` murni sebagai *fallback safety net* pencarian referensi manager.
  - Membatasi masuknya sampah secara fisik jika isi tong sudah menyentuh `calculatedMaxCapacity`.
  - Memvalidasi kecocokan kategori sampah (`EcoGarbageCategory`) dan melaporkan hasil sukses ke `AppGameManager`.

---

## 2. Alur Data (Data Flow)
1. **Load:** `AppInventoryManager` memuat database JSON dan total koin utama dari `PlayerPrefs` menggunakan referensi `DataKeyPlayerPrefs`.
2. **Setup:** `AppGameManager` (Scene Gameplay) membaca data tong terpasang $\rightarrow$ menyuntikkan data SO, level, dan referensi dirinya ke komponen `TrashBin`.
3. **Gameplay Action:** - Sampah masuk $\rightarrow$ `TrashBin` mencocokkan tipe kategori sampah.
   - Jika benar $\rightarrow$ `AddProgress()` pada UI slider bertambah, lalu memanggil `gameManager.RecordGarbageEntry(data)`.
   - `AppGameManager` mencatat nama sampah ke dalam riwayat `Dictionary` dan mengakumulasikan `scorePoint` sementara di RAM.
4. **End Game Session:** Pemain menyelesaikan level $\rightarrow$ Menekan tombol Klaim $\rightarrow$ `AppGameManager` mentransfer koin sementara ke `AppInventoryManager.instance.AddCoins()` $\rightarrow$ Data tersimpan permanen di HP.

---

## 3. Status Terakhir (Latest Fixes)
- [x] **Centralized Key System:** Migrasi seluruh string hardcode PlayerPrefs ke file arsitektur terisolasi `DataKeyPlayerPrefs.cs`.
- [x] **TrashBin Visual & Logic Fix:** - Memperbaiki teks UI `UpdateUI()` yang sempat terkunci bernilai penuh dari awal game (diubah menjadi `currentAmount / calculatedMaxCapacity`).
  - Memperbaiki pembatasan `Mathf.Clamp()` pada pengisian progress agar dinamis mengikuti kapasitas upgrade, bukan kapasitas dasar level 1.
- [x] **Gameplay History Isolation:** Memindahkan sistem penghitungan skor dan log riwayat pemilahan sampah dari script global ke script lokal scene gameplay (`AppGameManager`) demi performa memori dan kemudahan *reset data* saat restart level.
- [x] **Capacity Overflow Protection:** Menambahkan baris penahan masuknya sampah baru (`currentAmount >= calculatedMaxCapacity`) agar sampah hancur tanpa diproses jika tong sudah penuh.

## 4. Rencana Selanjutnya (Next Steps)
- [ ] **Sistem Nyawa & Penalti (Gameplay):** - Menggunakan nilai `penaltyPoint` dari `GarbageData` saat pemain salah memasukkan kategori sampah ke dalam tong.
  - Mengurangi total nyawa (*Hearts/Health*) pemain di scene gameplay melalui `AppGameManager`.
  - Trigger kondisi *Game Over* jika nyawa pemain menyentuh angka 0.
- [ ] **Sistem Pause Menu (Settings):**
  - Membuat fungsi Pause yang memanipulasi `Time.timeScale = 0f` agar gameplay berhenti sementara saat tombol Setting diklik.
  - Menyediakan UI Panel Pause dengan tombol *Resume*, *Restart*, dan *Main Menu*.
- [ ] **Sistem Upgrade UI Window:** Pembuatan panel UI toko memanfaatkan fungsi `TryUpgradeBin()` dari `AppInventoryManager`.
- [ ] **Score Screen UI:** Rekapitulasi hasil akhir level berdasarkan isi *Dictionary* riwayat sampah sebelum scene dihancurkan.

---
*Dokumen ini dibuat untuk memudahkan sinkronisasi progress pengembangan sistem Eco Garbage Collector.*