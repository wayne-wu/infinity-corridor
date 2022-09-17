using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject tileObj;

    public float speed;
    public int visibleTiles;
    public int windowPeriod = 2;

    public bool stop;

    Queue<GameObject> tiles;
    GameObject lastTile;

    int counter = 0;

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

        if (counter == windowPeriod)
        {
            SetWallVariant(/*left wall*/ newTile.transform.GetChild(3), 2);
            SetWallVariant(/*right wall*/ newTile.transform.GetChild(4), 2);
            counter = 0;
        }
        counter++;

        newTile.GetComponent<TileScript>().obstaclesPerTile = obstacles;
        lastTile = newTile;
        tiles.Enqueue(newTile);

        counter++;
    }

    void SetWallVariant(Transform xform, int idx)
    {
        for(int i = 1; i < xform.childCount; i++)
        {
            xform.GetChild(i).gameObject.SetActive(i == idx);
        }
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
