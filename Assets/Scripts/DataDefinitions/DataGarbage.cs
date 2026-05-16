using UnityEngine;

[CreateAssetMenu(fileName = "New Garbage Data", menuName = "SistemGame/DataGarbage")]
public class GarbageData : ScriptableObject
{
    [Header("Identitas Sampah")]
    public string garbageName;
    public Sprite garbageIcon; 
    public EcoGarbageCategory type;

    [Header("Nilai Ekonomi")]
    public int scorePoint;
    public int penaltyPoint;
}