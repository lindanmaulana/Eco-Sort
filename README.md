# Eco Garbage Collector - Progress Report

## 1. Sistem Arsitektur Data

Proyek ini menggunakan pendekatan **Data-Driven** dengan memisahkan antara Master Data, Save Data, Gameplay Session Data, dan Game Logic.

### A. Master Data (ScriptableObject)

- **File:** `TrashBinData.cs` & `GarbageData.cs`
- **Fungsi:** - `TrashBinData`: Menyimpan template statistik tong (Base Capacity, Bonus per Level, Icon, Tipe, Max Level).
  - `GarbageData`: Menyimpan identitas sampah (Nama, Icon, Tipe Kategori) beserta Nilai Ekonominya (`scorePoint` & `penaltyPoint`).
- **Logika Kapasitas:** Menggunakan fungsi `GetTotalCapacity(level)` untuk menghitung kapasitas dinamis.
  - _Rumus:_ `baseCapacity + (capacityBonusPerLevel * (currentLevel - 1))`

### B. Kunci Penyimpanan Terpusat (Static Class)

- **File:** `DataKeyPlayerPrefs.cs`
- **Fungsi:** Menyimpan seluruh string key `PlayerPrefs` secara global (`public const string`) untuk mencegah kesalahan typo lintas script dan mempermudah manajemen _database_ lokal game.

### C. Save Data & Inventory (JSON + PlayerPrefs - Global)

- **File:** `AppInventoryManager.cs` (Persistent Singleton)
- **Fungsi:** - Mengelola koin permanen (`totalCoins`).
  - Menyimpan daftar tong yang dimiliki (`playerInventory`) dalam bentuk List of `OwnedBin` yang diserialisasikan ke JSON via `InventoryWrapper`.
  - Mengatur fungsi transaksional permanen seperti `TryUpgradeBin()` (validasi koin & kenaikan level) dan `EquipBin()`.

### D. Game Session Manager (Gameplay Scene - Temporary)

- **File:** `AppGameManager.cs` (Local Scene Singleton)
- **Fungsi:** - Membaca data `Equipped` dari PlayerPrefs saat scene dimulai dan melakukan _dependency injection_ ke objek tong yang di-spawn.
  - **Pencatatan Sementara:** Menyimpan `koinYangDidapatLevelIni` dan struktur data `Dictionary<string, int> riwayatSampahLevelIni` secara lokal di RAM agar otomatis ter-reset saat _restart_ scene.
  - Menyediakan fungsi `KlaimHadiahKeInventoryPermanen()` untuk mengirim akumulasi hadiah ke `AppInventoryManager` saat level selesai.
  - Mengontrol kondisi akhir sesi permainan (_Win/Lose State_).

### E. Game Logic (Object Behavior)

- **File:** `TrashBin.cs`
- **Fungsi:** - Menerima cetak biru data dari `AppGameManager` via fungsi `InitializeBin()`.
  - Melakukan enkapsulasi `Start()` murni sebagai _fallback safety net_ pencarian referensi manager.
  - Membatasi masuknya sampah secara fisik jika isi tong sudah menyentuh `calculatedMaxCapacity`.
  - Memvalidasi kecocokan kategori sampah (`EcoGarbageCategory`) dan melaporkan hasil sukses/salah ke `AppGameManager`.
  - Menyediakan fungsi modular `IsBinFull()` untuk melaporkan status kepenuhan tong ke manager.

---

## 2. Alur Data (Data Flow)

1. **Load:** `AppInventoryManager` memuat database JSON dan total koin utama dari `PlayerPrefs` menggunakan referensi `DataKeyPlayerPrefs`.
2. **Setup:** `AppGameManager` (Scene Gameplay) membaca data tong terpasang $\rightarrow$ menyuntikkan data SO, level, dan referensi dirinya ke komponen `TrashBin`.
3. **Gameplay Action:** - Sampah masuk $\rightarrow$ `TrashBin` mencocokkan tipe kategori sampah.
   - **Jika Benar:** `AddProgress()` pada UI slider bertambah, lalu memanggil `gameManager.RecordGarbageEntry(data)`. `AppGameManager` mencatat riwayat sampah dan memanggil `CheckWinCondition()`.
   - **Jika Salah:** Memanggil `gameManager.RecordWrongEntry(data)` untuk mengurangi nyawa berdasarkan `penaltyPoint` dari sampah tersebut.
4. **End Game Session:** - **Kondisi Kalah:** Nyawa menyentuh `0` $\rightarrow$ `TriggerGameOver()` aktif $\rightarrow$ `Panel_GameOver` muncul.
   - **Kondisi Menang:** Seluruh tong di dalam scene bernilai penuh (`IsBinFull() == true`) $\rightarrow$ `TriggerGameWin()` aktif $\rightarrow$ `Panel_GameWin` muncul.

---

## 3. Status Terakhir (Latest Fixes & Features)

- [x] **Centralized Key System:** Migrasi seluruh string hardcode PlayerPrefs ke file arsitektur terisolasi `DataKeyPlayerPrefs.cs`.
- [x] **TrashBin Visual & Logic Fix:** Memperbaiki teks UI `UpdateUI()` dan pembatasan `Mathf.Clamp()` agar dinamis mengikuti level upgrade.
- [x] **Gameplay History Isolation:** Memindahkan sistem skor dan log riwayat ke `AppGameManager` demi kemudahan _reset data_ saat restart level.
- [x] **Capacity Overflow Protection:** Menambahkan penahan masuknya sampah baru (`currentAmount >= calculatedMaxCapacity`) agar sampah hancur tanpa diproses jika tong penuh.
- [x] **Sistem Nyawa Dinamis & Penalti:** Implementasi sistem 300 Max HP dengan visualisasi modular 5 hati (`AppHeartsUI`) yang terikat dengan nilai `penaltyPoint` (B3 memotong lebih besar).
- [x] **Teks Indikator HP Terperinci:** Sinkronisasi teks angka (Contoh: `298 / 300`) menggunakan TextMeshPro untuk melacak pengurangan HP kecil secara presisi.
- [x] **Sistem Win & Lose State Terpusat:**
  - Logika Game Over instan saat darah habis (`<= 0`).
  - Logika Level Win menggunakan metode modern Unity `FindObjectsByType<T>(FindObjectsInactive.Exclude)` untuk memastikan **semua** tong di layar wajib penuh sebelum panel kemenangan dipicu.
  - Integrasi aktivasi `Panel_GameOver` dan `Panel_GameWin` di dalam scene yang sama tanpa perlu berpindah scene.
- [x] **Fix - Pemicu Pengunci Ganda Game Over:** Menghapus redundansi variabel `isGameOver = true` di dalam fungsi `RecordWrongEntry` yang sebelumnya menyumbat gerbang masuk fungsi `TriggerGameOver()` dan menyebabkan panel UI macet.
- [x] **Fix - Pemutus Sinkronisasi Input Fisik UI:** Implementasi pemutusan status seret sampah (`isDragging = false`) pada baris teratas fungsi `Update()` di skrip seret, memastikan _EventSystem_ terbebas dari penahanan input _gameplay_ saat panel UI diaktifkan.
- [x] **Fix - Pembebasan Input Freeze Game Over:** Menemukan dan mengatasi masalah tombol beku akibat fitur _Error Pause_ pada Unity Editor Console saat mendeteksi `Debug.LogError`.
- [x] **Fix - Pemulihan Waktu Transisi Scene:** Menambahkan `Time.timeScale = 1f` pada fungsi `HandleChangeScene` di `GamePlay.cs` untuk mencegah scene MainMenu tersangkut di layar hijau akibat waktu global yang membeku.
- [x] **Fix - Dynamic Score & Recap System (Game Over):** Mengintegrasikan fungsi pengekstrakan data riwayat `CalculateLevelSummary` menggunakan parameter `out` untuk menyuplai data jumlah sampah organik, anorganik, B3, grand total, skor, dan koin reward ke `Panel_GameOver` secara dinamis.

---

## 4. Rencana Selanjutnya (Next Steps)

### 📌 Agenda Utama Besok:
- [ ] **Sistem Sinkronisasi Data Kemenangan (Game Win UI):**
  - Mengimplementasikan estafet parameter data `out` (`organic, inorganic, b3`) dari `CheckWinCondition()` di `AppGameManager` ke fungsi `ShowGameWin()` di `AppUIManager`.
  - Melakukan *drag & drop* komponen referensi `winRecap...` pada Inspector `_UIManager` di Unity Editor agar data tercetak secara dinamis saat menang.

### 📌 Fitur Gameplay & Koin:
- [ ] **Sistem Kalkulasi & Perolehan Koin Real-Time:**
  - Sinkronisasi teks UI Koin di layar gameplay (HUD utama) agar bertambah dengan animasi/efek teks berdenyut halus (*juice/punch scale*) saat sampah berhasil dipilah.
  - Memastikan penyimpanan data koin hasil `ClaimReward()` masuk secara permanen ke `PlayerPrefs` menggunakan skrip `DataKeyPlayerPrefs.cs` melalui `AppInventoryManager`.

### 📌 Sistem Upgrade & Toko:
- [ ] **Sistem Upgrade UI Window:** Pembuatan panel UI toko/shop memanfaatkan fungsi `TryUpgradeBin()` dari `AppInventoryManager` untuk meningkatkan kapasitas tong sampah menggunakan koin yang ditabung.
---

_Dokumen ini dibuat untuk memudahkan sinkronisasi progress pengembangan sistem Eco Garbage Collector._
