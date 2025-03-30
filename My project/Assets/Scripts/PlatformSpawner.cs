using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("Platform Prefabs")]
    public GameObject staticPlatformPrefab;
    public GameObject movingPlatformPrefab;
    
    public GameObject staticPlatformWithEnemy1;
    public GameObject staticPlatformWithEnemy2;
    public GameObject staticPlatformWithEnemy3;
    
    public GameObject staticPlatform_dangerous1;
    public GameObject staticPlatform_dangerous2;
    public GameObject staticPlatform_dangerous3;
    
    public GameObject bonusLife_prefab;

    [Header("Player Reference")]
    public Transform playerTransform;

	public int totalPlatformsSpawned = 0 ;

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
    private float default_platforms_distance = 1.0f;

    void Awake()
    {
        float camHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float platformHalfWidth = staticPlatform_dangerous3.GetComponent<SpriteRenderer>().bounds.extents.x;

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
        
        for (int xx = 0; xx < 10; xx++)
        {
            this.SpawnPlatform();
        }
    }
    
    string GetWeightedRandom(List<(string text, float weight)> options)
    {
        float totalWeight = 0f;
        foreach (var option in options)
            totalWeight += option.weight;

        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var option in options)
        {
            cumulative += option.weight;
            if (randomValue < cumulative)
                return option.text;
        }

        // На случай если ничего не выбралось (теоретически невозможно)
        return options[options.Count - 1].text;
    }

    void SpawnPlatform()
    {
		this.totalPlatformsSpawned += 1;

        Vector3 left = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 0));
        Vector3 right = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 0));
        float platformHalfWidth = staticPlatformPrefab.GetComponent<SpriteRenderer>().bounds.extents.x;

		List<(string text, float weight)> options = new List<(string, float)>()
            {
                ("static_safe", 0.8f),
                ("moving", 0.2f),
            };
        
        // Select platform type - and make sure the complexity
        /// is growing over the progress
		if (this.totalPlatformsSpawned < 5) {
			options = new List<(string, float)>()
            {
                ("static_safe", 1f),
            };

			default_platforms_distance = 0.8f;
			
		} else if (this.totalPlatformsSpawned < 7) {
			options = new List<(string, float)>()
            {
				("moving", 1f),
            };

			default_platforms_distance = 0.8f;
			
		} else if (this.totalPlatformsSpawned < 8) {
			options = new List<(string, float)>()
            {
				("platform_with_enemy1", 1f),
            };

			default_platforms_distance = 1.1f;
			
		} else if (this.totalPlatformsSpawned < 12) {
			options = new List<(string, float)>()
            {
				("static_safe", 0.5f),
                ("moving", 0.5f),
            };

			default_platforms_distance = 1.1f;
			
		} else if (this.totalPlatformsSpawned < 13) {
			options = new List<(string, float)>()
            {
				("platform_with_enemy2", 1f),
            };

			default_platforms_distance = 1f;
			
		} else if (this.totalPlatformsSpawned < 17) {
			options = new List<(string, float)>()
            {
				("static_safe", 0.3f),
				("moving", 0.7f),
            };

			default_platforms_distance = 1.1f;
			
		} else if (this.totalPlatformsSpawned < 18) {
			options = new List<(string, float)>()
            {
				("platform_with_enemy3", 1f),
            };

			default_platforms_distance = 1.2f;
			
		} else if (this.totalPlatformsSpawned < 25) {
			options = new List<(string, float)>()
            {
				("moving", 0.3f),
                ("platform_with_enemy3", 0.2f),
				("platform_dangerous2", 0.5f),
            };

			default_platforms_distance = 1.2f;
			
		} else if (this.totalPlatformsSpawned < 50) {
			options = new List<(string, float)>()
            {
				("platform_dangerous1", 0.1f),
				("moving", 0.5f),
                ("platform_with_enemy2", 0.2f),
                ("platform_with_enemy2", 0.2f),
            };

			default_platforms_distance = 1.1f;
			
		} else if (this.totalPlatformsSpawned < 75) {
			options = new List<(string, float)>()
            {
                ("static_safe", 0.1f),
				("moving", 0.3f),
				("platform_with_enemy2", 0.2f),
				("platform_with_enemy3", 0.2f),
				("platform_dangerous3", 0.1f),
				("platform_dangerous3", 0.1f),
            };

			default_platforms_distance = 1.2f;
			
		} else if (this.totalPlatformsSpawned < 100) {
			options = new List<(string, float)>()
            {
                ("static_safe", 0.2f),
				("moving", 0.4f),
				("platform_dangerous2", 0.2f),
                ("platform_with_enemy3", 0.2f),
            };

			default_platforms_distance = 1.1f;
			
		} else
if (this.totalPlatformsSpawned < 200) {
			options = new List<(string, float)>()
            {
                ("static_safe", 0.1f),
                ("moving", 0.3f),
                ("platform_with_enemy1", 0.1f),  // always left
                ("platform_with_enemy2", 0.1f),  // always right
                ("platform_with_enemy3", 0.1f),  // random position
                ("platform_dangerous1", 0.1f),   // random position
                ("platform_dangerous2", 0.1f),  // random position
                ("platform_dangerous3", 0.1f),  // this one is deadly
            };

			default_platforms_distance = Random.Range(1.1f, 1.3f);
        } else {

            options = new List<(string, float)>()
            {
                ("moving", 0.1f),
                ("platform_with_enemy1", 0.2f),  // always left
                ("platform_with_enemy2", 0.2f),  // always right
                ("platform_with_enemy3", 0.2f),  // random position
                ("platform_dangerous1", 0.1f),   // random position
                ("platform_dangerous2", 0.1f),  // random position
                ("platform_dangerous3", 0.1f),  // this one is deadly
            };

			default_platforms_distance += 0.01f;
        }

        string platform_type = GetWeightedRandom(options);

        if (activePlatforms.Count == 0) platform_type = "static_safe";

        float x = 0;
        
        if (platform_type == "moving")
        {
            x = (left.x + right.x) / 2.0f;
            float y = nextSpawnY + default_platforms_distance + Random.Range(0.0f, 0.3f);
            Vector3 spawnPos = new Vector3(x, y, platformZ);
            
            GameObject prefab = movingPlatformPrefab;
            GameObject platform = Instantiate(prefab, spawnPos, Quaternion.identity);

            activePlatforms.Add(platform);
            nextSpawnY = y;

            previous_static_platform = 0;

            // 30% chance to give additional life with moving platform
            if (Random.value < 0.3f)
            {
                Vector3 bonusPos = new Vector3(Random.Range(left.x + 0.5f, right.x - 0.5f), y + 0.5f, 1);
                Instantiate(bonusLife_prefab, bonusPos, Quaternion.identity);
            }
        }
        else if (platform_type == "static_safe")
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

            float y = nextSpawnY + default_platforms_distance + Random.Range(0.0f, 0.3f);
            Vector3 spawnPos = new Vector3(x, y, platformZ);

            GameObject prefab = staticPlatformPrefab;
            GameObject platform = Instantiate(prefab, spawnPos, Quaternion.identity);
            
            activePlatforms.Add(platform);
            nextSpawnY = y;
        }
        else if (platform_type == "platform_with_enemy1")
        {
            x = left.x + platformHalfWidth;
            previous_static_platform = 1;
            
            float y = nextSpawnY + default_platforms_distance + Random.Range(0.0f, 0.3f);
            Vector3 spawnPos = new Vector3(x, y, platformZ);
            
            GameObject prefab = staticPlatformWithEnemy1;
            GameObject platform = Instantiate(prefab, spawnPos, Quaternion.identity);

            activePlatforms.Add(platform);
            nextSpawnY = y;
        }
        else if (platform_type == "platform_with_enemy2")
        {
            if (previous_static_platform == 1)
            {
                x = right.x - platformHalfWidth;
                previous_static_platform = 2;
            } else
            {
                x = left.x + platformHalfWidth;
                previous_static_platform = 1;
            }
            
            // 30% chance to put it in the middle of the screen:
            if (Random.Range(0.0f, 1.0f) < 0.3f)
            {
                x = (left.x + right.x) / 2.0f;
            }
            
            float y = nextSpawnY + default_platforms_distance + Random.Range(0.0f, 0.3f);
            Vector3 spawnPos = new Vector3(x, y, platformZ);
            
            GameObject prefab = staticPlatformWithEnemy2;
            GameObject platform = Instantiate(prefab, spawnPos, Quaternion.identity);

            activePlatforms.Add(platform);
            nextSpawnY = y;
        }
        else if (platform_type == "platform_with_enemy3")
        {
            if (previous_static_platform == 1)
            {
                x = right.x - platformHalfWidth;
                previous_static_platform = 2;
            } else
            {
                x = left.x + platformHalfWidth;
                previous_static_platform = 1;
            }
            
            // 30% chance to put it in the middle of the screen:
            if (Random.Range(0.0f, 1.0f) < 0.3f)
            {
                x = (left.x + right.x) / 2.0f;
            }
            
            float y = nextSpawnY + default_platforms_distance + Random.Range(0.0f, 0.3f);
            Vector3 spawnPos = new Vector3(x, y, platformZ);
            
            GameObject prefab = staticPlatformWithEnemy3;
            GameObject platform = Instantiate(prefab, spawnPos, Quaternion.identity);

            activePlatforms.Add(platform);
            nextSpawnY = y;
        }
        else if (platform_type == "platform_dangerous1")
        {
            if (previous_static_platform == 1)
            {
                x = right.x - platformHalfWidth;
                previous_static_platform = 2;
            } else
            {
                x = left.x + platformHalfWidth;
                previous_static_platform = 1;
            }
            
            float y = nextSpawnY + default_platforms_distance + Random.Range(0.0f, 0.3f);
            Vector3 spawnPos = new Vector3(x, y, platformZ);
            
            GameObject prefab = staticPlatform_dangerous1;
            GameObject platform = Instantiate(prefab, spawnPos, Quaternion.identity);

            activePlatforms.Add(platform);
            nextSpawnY = y;
        }
        else if (platform_type == "platform_dangerous2")
        {
            if (previous_static_platform == 1)
            {
                x = right.x - platformHalfWidth;
                previous_static_platform = 2;
            } else
            {
                x = left.x + platformHalfWidth;
                previous_static_platform = 1;
            }
            
            float y = nextSpawnY + default_platforms_distance + Random.Range(0.0f, 0.3f);
            Vector3 spawnPos = new Vector3(x, y, platformZ);
            
            GameObject prefab = staticPlatform_dangerous2;
            GameObject platform = Instantiate(prefab, spawnPos, Quaternion.identity);

            activePlatforms.Add(platform);
            nextSpawnY = y;
        }
        else if (platform_type == "platform_dangerous3")
        {
            if (previous_static_platform == 1)
            {
                x = right.x - platformHalfWidth;
                previous_static_platform = 2;
            } else
            {
                x = left.x + platformHalfWidth;
                previous_static_platform = 1;
            }
            
            // 30% chance to put it in the middle of the screen:
            if (Random.Range(0.0f, 1.0f) < 0.3f)
            {
                x = (left.x + right.x) / 2.0f;
            }
            
            float y = nextSpawnY + default_platforms_distance + Random.Range(0.0f, 0.3f);
            Vector3 spawnPos = new Vector3(x, y, platformZ);
            
            GameObject prefab = staticPlatform_dangerous3;
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
