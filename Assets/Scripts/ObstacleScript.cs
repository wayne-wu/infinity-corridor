using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    public AudioClip fallClip;
    public AudioClip hitClip;

    GlobalManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GlobalManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            AudioSource.PlayClipAtPoint(hitClip, gameObject.transform.position);
            gameManager.EndGame();
        }
        else
        {
            AudioSource.PlayClipAtPoint(fallClip, gameObject.transform.position);
        }
    }
}
