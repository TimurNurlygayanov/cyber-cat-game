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
    
    private int previous_static_platform = 1;

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

        nextSpawnY = playerY - 2f;
        
        for (int xx = 0; xx < 5; xx++)
        {
            this.SpawnPlatform();
        }
    }

    void SpawnPlatform()
    {
        Vector3 left = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 0));
        Vector3 right = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 0));
        float platformHalfWidth = staticPlatformPrefab.GetComponent<SpriteRenderer>().bounds.extents.x;

        
        // Select platform type:
        bool platform_type = Random.value < movingPlatformChance;

        if (activePlatforms.Count == 0) platform_type = false;

        float x = 0;
        
        if (platform_type)
        {
            x = left.x + Random.Range(0.1f, 1.0f);
            float y = nextSpawnY + 1.0f + Random.Range(0.0f, 0.3f);
            Vector3 spawnPos = new Vector3(x, y, platformZ);
            
            GameObject prefab = movingPlatformPrefab;
            GameObject platform = Instantiate(prefab, spawnPos, Quaternion.identity);

            activePlatforms.Add(platform);
            nextSpawnY = y;

            previous_static_platform = 0;
        }
        else
        {
            if (previous_static_platform == 1)
            {
                x = right.x - platformHalfWidth;
                previous_static_platform = 2;
            } else if (previous_static_platform == 2)
            {
                x = left.x + platformHalfWidth;
                previous_static_platform = 1;
            }
            else
            {
                x = left.x + platformHalfWidth;
                previous_static_platform = 1;
            }

            if (activePlatforms.Count == 0)
            {
                Vector3 pos = playerTransform.position;
                pos.x = x;
                playerTransform.position = pos;
            }

            float y = nextSpawnY + 1.0f + Random.Range(0.0f, 0.3f);
            Vector3 spawnPos = new Vector3(x, y, platformZ);
            
            GameObject prefab = staticPlatformPrefab;
            GameObject platform = Instantiate(prefab, spawnPos, Quaternion.identity);

            activePlatforms.Add(platform);
            nextSpawnY = y;
        }
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
