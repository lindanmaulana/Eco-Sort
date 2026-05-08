using UnityEngine;

public class AppDataManager: MonoBehaviour
{
    public static AppDataManager instance;

    [System.Serializable]
    public class DataUpgrade
    {
        public int level;
        public int kapasitas;
        public int hargaUpgrade;
    }

    [System.Serializable]
    public class SistemTong {
        public string jenis;
        public Color warnaTema;
        public DataUpgrade[] infoLevel;
    }

    [Header("Sistem Game")]
    public SistemTong configOrganik;
    public SistemTong configAnorganik;
    public SistemTong configB3;

    private void Awake() {
        if (instance == null) instance = this;
    }
}
