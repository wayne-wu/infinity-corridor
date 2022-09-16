using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{
    public float rotationSpeed = 3.0f;
    public float gravityScale = 2.0f;

    public AudioClip swooshClip;

    const float MOVE = 3.0f;
    Vector3 rightVec;  // the right vector

    Rigidbody rbd;

    MeshRenderer renderer;

    public bool hasInvisible;

    bool alive;

    const float eps = 0.0001f;
    float rotate; 

    // Start is called before the first frame update
    void Start()
    {
        rbd = gameObject.GetComponent<Rigidbody>();
        renderer = gameObject.GetComponent<MeshRenderer>();

        rightVec = Vector3.right;
        rotate = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("left"))
        {
            Dodge(false);
        }
        else if (Input.GetKeyDown("right"))
        {
            Dodge(true);
        }

        bool changeGravity = Input.GetKey(KeyCode.LeftShift);
        if (changeGravity && Input.GetKeyDown("left"))
            ChangeGravity(-90);
        else if (changeGravity && Input.GetKeyDown("right"))
            ChangeGravity(90);
        else if (changeGravity && Input.GetKeyDown("up"))
            ChangeGravity(180);            
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(rotate) > eps)
        {
            float rotateAmount = rotationSpeed * rotate * Time.deltaTime;
            if (Mathf.Abs(rotateAmount) > Mathf.Abs(rotate))
                rotateAmount = rotate;
            transform.Rotate(Vector3.forward * rotateAmount);
            rotate -= rotateAmount;

            // Adding an extra force here so that the player falls faster
            rbd.AddForce(Physics.gravity*gravityScale);
        }
    }

    void Dodge(bool isRight)
    {
        float scale = isRight ? MOVE : -MOVE;
        Vector3 newPos = transform.position + scale * rightVec;
        if (Mathf.Abs(Vector3.Dot(newPos, rightVec)) < MOVE + 1.0f)
            transform.position = newPos;
    }

    void ChangeGravity(float angle)
    {
        AudioSource.PlayClipAtPoint(swooshClip, gameObject.transform.position);

        Physics.gravity = Quaternion.Euler(0, 0, angle) * Physics.gravity;

        if (Mathf.Abs(rotate) > eps)  // Finish the last rotation
            transform.Rotate(Vector3.forward * rotate);

        Vector3 prevRightVec = rightVec;
        rightVec = Quaternion.Euler(0, 0, angle) * rightVec;

        // TODO: See if there's a cleaner way to do this
        if(Mathf.Abs(Vector3.Dot(rightVec, prevRightVec)) < eps)
        {
            Vector3 newPos = Mathf.Sign(angle) * -MOVE * rightVec;
            newPos.z = transform.position.z;
            transform.position = newPos;
        }

        rotate = angle;
    }

    public void Die()
    {
        // Make player invisible. Not destroying because of the child camera
        renderer.enabled = false;
    }

    public void MakeInvisible()
    {
        hasInvisible = false;

        Color c = renderer.material.color;
        c.a = 0.25f;
        renderer.material.SetColor("_Color", c);

        // Temporarily ignore collision between player and obstacles
        Physics.IgnoreLayerCollision(3, 6, true);

        // Make visible again after few seconds
        Invoke("MakeVisible", 3.0f);
    }

    void MakeVisible()
    {
        Color c = renderer.material.color;
        c.a = 1.0f;
        renderer.material.SetColor("_Color", c);
        Physics.IgnoreLayerCollision(3, 6, false);
    }

}
