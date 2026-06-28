using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GarbageSpawner : MonoBehaviour
{
    public GameObject[] garbagePrefabs; 
    public float spawnInterval = 1f;   
    public Vector2 spawnRangeX = new Vector2(-7f, 7f);
    public Vector2 spawnRangeY = new Vector2(-4f, 4f);

    [Header("Base Settings (Awal Game)")]
    [Tooltip("Jeda waktu awal antar spawn (detik)")]
    public float baseSpawnInterval = 1.6f;   
    [Tooltip("Jumlah minimal sampah di awal game")]
    public int baseMinSpawn = 1;
    [Tooltip("Jumlah maksimal sampah di awal game")]
    public int baseMaxSpawn = 2; 

    [Header("Difficulty Progression (Tingkat Kesulitan)")]
    [Tooltip("Setiap berapa detik kesulitan akan naik? (35 detik sekali sangat pas untuk game 4 menit)")]
    public float difficultyInterval = 35f;
    [Tooltip("Pengurangan jeda waktu setiap tingkat kesulitan naik")]
    public float intervalDecrease = 0.22f;
    [Tooltip("Batas paling cepat jeda spawn (detik)")]
    public float minAllowedInterval = 0.4f;
    [Tooltip("Penambahan jumlah maksimal sampah yang keluar setiap kesulitan naik")]
    public int maxSpawnIncrease = 1;
    [Tooltip("Batas paling banyak sampah yang boleh keluar sekaligus")]
    public int absoluteMaxSpawn = 5;

    // Variabel dinamis internal
    private float currentSpawnInterval;
    private int currentMinSpawn;
    private int currentMaxSpawn;
    private int lastDifficultyLevel = 0;

    private List<int> lockedSpawnIndices = new List<int>();
    private int currentListIndex = 0;

    private Coroutine notificationCoroutine;
    private Coroutine lightningCoroutine;

    void Start()
    {
        currentSpawnInterval = baseSpawnInterval;
        currentMinSpawn = baseMinSpawn;
        currentMaxSpawn = baseMaxSpawn;

        InitializeShuffleDeck();
        StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        if (GamePlay.Instance != null)
        {
            float totalWaktuGame = GamePlay.Instance.gameElapsedTime;

            // Membatasi alur agar spawner berhenti menaikkan tingkat kesulitan jika waktu sudah lewat 4 menit (240 detik)
            if (totalWaktuGame > 240f) return;

            int currentDifficultyLevel = Mathf.FloorToInt(totalWaktuGame / difficultyInterval);

            if (currentDifficultyLevel > lastDifficultyLevel)
            {
                lastDifficultyLevel = currentDifficultyLevel;
                IncreaseDifficulty(currentDifficultyLevel);
            }
        }
    }

    void IncreaseDifficulty(int newLevel)
    {
        if (currentSpawnInterval > minAllowedInterval)
        {
            currentSpawnInterval -= intervalDecrease;
            if (currentSpawnInterval < minAllowedInterval) currentSpawnInterval = minAllowedInterval;
            Debug.Log($"[SPAWNER] Kecepatan naik! Jeda sekarang: {currentSpawnInterval} detik.");
        }

        if (currentMaxSpawn < absoluteMaxSpawn)
        {
            currentMaxSpawn += maxSpawnIncrease;
            if (currentMaxSpawn >= 4) currentMinSpawn += 1; 
            Debug.Log($"[SPAWNER] Jumlah naik! Sekali spawn: {currentMinSpawn} - {currentMaxSpawn} sampah.");
        }

        if (GamePlay.Instance != null)
        {
            GamePlay.Instance.SetDifficultyVisuals(newLevel);
        }
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentSpawnInterval);

            if (garbagePrefabs.Length == 0) continue;

            int totalToSpawn = Random.Range(currentMinSpawn, currentMaxSpawn + 1);

            for (int i = 0; i < totalToSpawn; i++)
            {
                SpawnSingleGarbage();
            }
        }
    }

    void InitializeShuffleDeck()
    {
        if (garbagePrefabs.Length == 0) return;

        lockedSpawnIndices.Clear();
        for (int i = 0; i < garbagePrefabs.Length; i++)
        {
            lockedSpawnIndices.Add(i);
        }

        for (int i = lockedSpawnIndices.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            int temp = lockedSpawnIndices[i];
            lockedSpawnIndices[i] = lockedSpawnIndices[rnd];
            lockedSpawnIndices[rnd] = temp;
        }

        currentListIndex = 0;
    }

    void SpawnSingleGarbage()
    {
        if (currentListIndex >= lockedSpawnIndices.Count)
        {
            InitializeShuffleDeck();
        }

        int randomIndex = lockedSpawnIndices[currentListIndex];
        currentListIndex++;
        
        if (garbagePrefabs[randomIndex] == null) return;
        
        Vector2 spawnPos = new Vector2(Random.Range(spawnRangeX.x, spawnRangeX.y), Random.Range(spawnRangeY.x, spawnRangeY.y));
        
        GameObject go = Instantiate(garbagePrefabs[randomIndex], spawnPos, Quaternion.identity);

        Vector3 smallTargetScale = new Vector3(0.4f, 0.4f, 1f); 
        StartCoroutine(ScaleUp(go, smallTargetScale));

        Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 randomForce = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            rb.AddForce(randomForce, ForceMode2D.Impulse);
        }

        // Mengubah pengali menjadi 3.0f agar sampah tidak terlalu lama menumpuk saat tempo spawn sangat cepat
        float destroyDelay = currentSpawnInterval * 3.0f;

        if (destroyDelay < 1.8f) 
        {
            destroyDelay = 1.8f;
        }

        GarbageVisual visualScript = go.GetComponent<GarbageVisual>();
        
        if (visualScript != null)
        {
            visualScript.StartLifetimeCountdown(destroyDelay);
        }
        else
        {
            Destroy(go, destroyDelay);
        }
    }

    IEnumerator ScaleUp(GameObject target, Vector3 targetScale)
    {
        if (target == null) yield break;

        target.transform.localScale = Vector3.zero; 
        float timer = 0f;
        float duration = 0.2f;
        
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