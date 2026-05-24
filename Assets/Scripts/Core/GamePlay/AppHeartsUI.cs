using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AppHeartsUI : MonoBehaviour
{
    [Header("Sprites Settings")]
    public Sprite fullHeart;   
    public Sprite emptyHeart;  

    [Header("UI Elements")]
    public List<Image> heartImages;

    public void UpdateHeartsVisuals(int currentHealth, int maxHealth)
    {
        float healthPercentage = (float)currentHealth / maxHealth;
        int heartsToFill = Mathf.CeilToInt(healthPercentage * heartImages.Count);

        for (int i = 0; i < heartImages.Count; i++)
        {
            if (i < heartsToFill)
            {
                heartImages[i].sprite = fullHeart; 
            }
            else
            {
                heartImages[i].sprite = emptyHeart;
            }
        }
    }
}