using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject tileObj;

    public float speed;
    public int visibleTiles;

    public bool stop;

    Queue<GameObject> tiles;
    GameObject lastTile;

    // Start is called before the first frame update
    void Start()
    {
        stop = false;

        tiles = new Queue<GameObject>();

        for(int i = 0; i < visibleTiles; i++)
            SpawnTile(0);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        if (stop) return;

        foreach(GameObject t in tiles)
        {
            t.transform.position -= speed * Vector3.forward;
        }
    }

    public void SpawnTile(int obstacles=3)
    {
        Vector3 spawnPos = !lastTile ? Vector3.zero : lastTile.transform.GetChild(0).position;
        GameObject newTile = Instantiate(tileObj, spawnPos, Quaternion.identity);
        newTile.GetComponent<TileScript>().obstaclesPerTile = obstacles;
        lastTile = newTile;
        tiles.Enqueue(newTile);
    }

    public void RemoveTile()
    {
        Invoke("DestroyTile", 2);
    }

    void DestroyTile()
    {
        Destroy(tiles.Dequeue());
    }
}
