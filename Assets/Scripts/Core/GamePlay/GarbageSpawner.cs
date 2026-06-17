using UnityEngine;
using System.Collections;

public class GarbageSpawner: MonoBehaviour
{
    public GameObject[] garbagePrefabs; 
    public float spawnInterval = 1f;   
    public Vector2 spawnRangeX = new Vector2(-7f, 7f);
    public Vector2 spawnRangeY = new Vector2(-4f, 4f);

    void Start()
    {
        InvokeRepeating("SpawnGarbage", 1f, spawnInterval);
    }

    void SpawnGarbage()
    {
        if (garbagePrefabs.Length == 0) return;

        int randomIndex = Random.Range(0, garbagePrefabs.Length);
        
        Vector2 spawnPos = new Vector2(Random.Range(spawnRangeX.x, spawnRangeX.y), Random.Range(spawnRangeY.x, spawnRangeY.y));

        GameObject go = Instantiate(garbagePrefabs[randomIndex], spawnPos, Quaternion.identity);

        StartCoroutine(ScaleUp(go));

        Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 randomForce = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            rb.AddForce(randomForce, ForceMode2D.Impulse);
        }
    }

    IEnumerator ScaleUp(GameObject target)
    {
        if (target == null) yield break;

        target.transform.localScale = Vector3.zero; 
        float timer = 0f;
        float duration = 0.2f;
        Vector3 targetScale = new Vector3(0.4f, 0.4f, 1f); 
        
        while (timer < duration)
        {
            if (target == null) yield break; 

            timer += Time.deltaTime;
            target.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, timer / duration);
            
            yield return null;
        }

        if (target != null)
        {
            target.transform.localScale = targetScale;
        }
    }
}
