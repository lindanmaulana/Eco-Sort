using UnityEngine;
using System.Collections;

public class GarbageVisual : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartLifetimeCountdown(float totalLifetime)
    {
        float blinkDelayTime = totalLifetime - 1.2f;

        if (blinkDelayTime < 0) blinkDelayTime = 0f;

        StartCoroutine(BlinkScheduleRoutine(blinkDelayTime));
        
        Destroy(gameObject, totalLifetime);
    }

    IEnumerator BlinkScheduleRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        while (true)
        {
            Color currentColor = spriteRenderer.color;
            currentColor.a = 0.3f;
            spriteRenderer.color = currentColor;

            yield return new WaitForSeconds(0.15f); 

            currentColor.a = 1f;
            spriteRenderer.color = currentColor;

            yield return new WaitForSeconds(0.15f);
        }
    }
}