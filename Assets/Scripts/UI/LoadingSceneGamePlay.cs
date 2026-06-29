using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LoadingSceneGamePlay : MonoBehaviour
{
    [Header("Carousel Setup")]
    [SerializeField] private AppCarouselScroll carouselSystem;
    
    [Header("Daftar ID Map (Urutkan sesuai urutan gambar di UI Content)")]
    [SerializeField] private List<string> mapIDs = new List<string> { "DanauToba", "Ciremai", "SekolahDasar", "PasarCiawi", "TerminalAncaran", "GunungRinjani" };

    [Header("Scene Target")]
    [SerializeField] private string loadingSceneName = "LoadingScene";

    public void TombolMainDiklik()
    {
        if (carouselSystem == null) return;

        int currentMapIndex = carouselSystem.GetCurrentSelectedIndex();

        if (currentMapIndex >= 0 && currentMapIndex < mapIDs.Count)
        {
            string chosenMapID = mapIDs[currentMapIndex];

            PlayerPrefs.SetString("Selected_Gameplay_Map", chosenMapID);
            PlayerPrefs.Save();

            Debug.Log($"[Menu] Memilih map: {chosenMapID}. Pindah ke Loading Scene...");

            SceneManager.LoadScene(loadingSceneName);
        }
    }
}