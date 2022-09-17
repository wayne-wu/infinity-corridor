using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowScript : MonoBehaviour
{
    public AudioClip woodCrack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            Debug.Log("Window Collision");
            Rigidbody rbd = other.gameObject.GetComponent<Rigidbody>();
            rbd.AddForce(Physics.gravity * 100);

            AudioSource.PlayClipAtPoint(woodCrack, gameObject.transform.position);
        }
    }
}
