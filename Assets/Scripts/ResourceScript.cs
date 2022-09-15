using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScript : MonoBehaviour
{
    public AudioClip resourceClip;

    PlayerControl player; 

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            AudioSource.PlayClipAtPoint(resourceClip, gameObject.transform.position, 5.0f);

            player.hasInvisible = true;
            Destroy(gameObject);
        }
    }
}
