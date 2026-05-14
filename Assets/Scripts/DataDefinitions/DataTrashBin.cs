using UnityEngine;

public enum TrashBinType { Organic, Inorganic, B3 }

[CreateAssetMenu(fileName = "Data Trash Bin", menuName = "SistemGame/DataTrashBin")]
public class TrashBinData: ScriptableObject
{
    [Header("Basic Information")]
    public string binName;
    public Sprite binIcon; 
    public TrashBinType type;

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
