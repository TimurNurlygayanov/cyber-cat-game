using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("Platform Prefabs")]
    public GameObject staticPlatformPrefab;
    public GameObject movingPlatformPrefab;

    [Header("Player Reference")]
    public Transform playerTransform;

    [Header("Spawn Settings")]
    public float minY = 0.8f;
    public float maxY = 1.2f;
    public float generationBuffer = 10f;
    public float deleteBelowYBuffer = 15f;
    [Range(0, 1)] public float movingPlatformChance = 0.2f;

    [Header("Z Position")]
    public float platformZ = 0.5f;

    [Header("Debug/Control")]
    public bool resetPlatforms = false;

    private float minX;
    private float maxX;
    private float nextSpawnY = 0f;
    private List<GameObject> activePlatforms = new List<GameObject>();

    void Awake()
    {
        float camHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float platformHalfWidth = staticPlatformPrefab.GetComponent<SpriteRenderer>().bounds.extents.x;

        minX = -camHalfWidth + platformHalfWidth;
        maxX = camHalfWidth - platformHalfWidth;
    }

    void Start()
    {
        nextSpawnY = playerTransform.position.y - 1f;
        SpawnInitialPlatforms();
    }

    void Update()
    {
        if (resetPlatforms)
        {
            ResetPlatforms();
            resetPlatforms = false;
        }

        while (nextSpawnY < playerTransform.position.y + generationBuffer)
        {
            SpawnPlatform();
        }

        for (int i = activePlatforms.Count - 1; i >= 0; i--)
        {
            if (activePlatforms[i].transform.position.y < playerTransform.position.y - deleteBelowYBuffer)
            {
                Destroy(activePlatforms[i]);
                activePlatforms.RemoveAt(i);
            }
        }
    }

    void SpawnInitialPlatforms()
    {
        float playerX = playerTransform.position.x;
        float playerY = playerTransform.position.y;

        Vector3 underPlayerPos = new Vector3(playerX, playerY - 1f, platformZ);
        GameObject basePlatform = Instantiate(staticPlatformPrefab, underPlayerPos, Quaternion.identity);
        activePlatforms.Add(basePlatform);

        nextSpawnY = underPlayerPos.y;

        int additionalCount = 4;
        float startY = nextSpawnY + 0.9f;

        for (int i = 0; i < additionalCount; i++)
        {
            float x = Random.Range(minX, maxX);
            Vector3 pos = new Vector3(x, startY, platformZ);
            GameObject platform = Instantiate(staticPlatformPrefab, pos, Quaternion.identity);
            activePlatforms.Add(platform);
            startY += 0.9f;
            nextSpawnY = startY;
        }
    }

    void SpawnPlatform()
    {
        float x = Random.Range(minX, maxX);
        float y = nextSpawnY + Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(x, y, platformZ);

        GameObject prefab = Random.value < movingPlatformChance ? movingPlatformPrefab : staticPlatformPrefab;
        GameObject platform = Instantiate(prefab, spawnPos, Quaternion.identity);

        activePlatforms.Add(platform);
        nextSpawnY = y;
    }

    void ResetPlatforms()
    {
        foreach (var plat in activePlatforms)
        {
            if (plat != null)
                Destroy(plat);
        }

        activePlatforms.Clear();
        nextSpawnY = playerTransform.position.y - 1f;
        SpawnInitialPlatforms();
    }
}
