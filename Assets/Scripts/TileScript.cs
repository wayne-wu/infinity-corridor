using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{

    public GameObject staticObstaclePrefab;
    public GameObject dynamicObstaclePrefab;
    public GameObject resourcePrefab;

    public int obstaclesPerTile;

    WorldGenerator generator;

    // Start is called before the first frame update
    void Start()
    {
        generator = GameObject.FindObjectOfType<WorldGenerator>();
        SpawnObstacles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            generator.SpawnTile();
            generator.RemoveTile();
        }
    }

    void SpawnObstacles()
    {
        for(int i = 0; i < obstaclesPerTile; i++)
        {
            int obstacleSpawnIdx = Random.Range(0, 28);
            if (obstacleSpawnIdx > 0)
            {
                obstacleSpawnIdx += 4;
                Transform spawnPoint = transform.GetChild(obstacleSpawnIdx);
                GameObject prefab = Random.value < 0.2 ? dynamicObstaclePrefab : staticObstaclePrefab;
                if (Random.value < 0.05)
                    Instantiate(resourcePrefab, spawnPoint.position, Quaternion.identity, transform);
                else
                {
                    int x_rot = Random.Range(-1, 1);
                    int y_rot = Random.Range(-1, 1);
                    int z_rot = Random.Range(-1, 1);
                    Instantiate(prefab, spawnPoint.position, Quaternion.Euler(x_rot * 90.0f, y_rot * 90.0f, z_rot * 90.0f), transform);
                }
            }
        }
    }
}
