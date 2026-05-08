using UnityEngine;

[CreateAssetMenu(fileName = "Data Trash Bin", menuName = "SistemGame/DataTrashBin")]
public class DataTrashBin: ScriptableObject
{
    [Header("Identitas")]
    public string namaTong;
    public string ID_Tag;

    [Header("Visual")]
    public Sprite gambarTong;
    public Color warnaBar;

    [Header("Sistem Level")]
    public float kapasitasLevel1;
    public float kapasitasLevel2;
    public float kapasitasLevel3;
    
    [Header("Ekonomi")]
    public int hargaUpgrade;
    public int poinPerSampah;
}
