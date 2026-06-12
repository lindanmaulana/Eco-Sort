using UnityEngine;

[CreateAssetMenu(fileName = "Data Trash Bin", menuName = "SistemGame/DataTrashBin")]
public class TrashBinData: ScriptableObject
{
    [Header("Unique Identifier (TIDAK BOLEH DIUBAH SETELAH DISET)")]
    [Tooltip("Gunakan ID unik tanpa spasi, contoh: bin_organic_v1, bin_b3_v2")]
    public string binID;
    
    [Header("Basic Information")]
    public string binName;
    public Sprite binIcon; 
    public EcoGarbageCategory type;

    [Header("Stats & Economy")]
    public int basePrice;
    public float baseCapacity;

    [Header("Upgrade System")]
    public int maxLevel = 5;
    public int upgradeCostPerLevel;
    public float capacityBonusPerLevel;

    // Fungsi keren untuk menghitung total kapasitas berdasarkan level
    public float GetTotalCapacity(int currentLevel)
    {
        return baseCapacity + (capacityBonusPerLevel * (currentLevel - 1));
    }

    // Fungsi untuk menghitung biaya upgrade ke level berikutnya
    public int GetUpgradeCost(int currentLevel)
    {
        return upgradeCostPerLevel * currentLevel;
    }
}
