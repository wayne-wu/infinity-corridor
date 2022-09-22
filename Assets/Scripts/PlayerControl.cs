using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{
    public float rotationSpeed = 3.0f;
    public float gravityScale = 2.0f;

    public AudioClip swooshClip;

    const float MOVE = 10.0f/3.0f;
    Vector3 rightVec;  // the right vector
    Vector3 upVec;

    Rigidbody rbd;

    Animator animator;

    SkinnedMeshRenderer renderer;

    public bool hasInvisible;

    bool alive;
    bool isGrounded;

    public float distToGround = 1.0f;

    const float eps = 0.0001f;
    float rotate;


    Vector3 touchStartPos;
    Vector3 touchEndPos;


    // Start is called before the first frame update
    void Start()
    {
        rbd = gameObject.GetComponent<Rigidbody>();
        renderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();

        animator = gameObject.GetComponentInChildren<Animator>();

        rightVec = Vector3.right;
        upVec = Vector3.up;
        rotate = 0.0f;

        MakeVisible();  // make sure collision is on
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;            
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                touchEndPos = touch.position;

                Vector3 dir = touchEndPos - touchStartPos;
                if (dir.magnitude < 0.01)
                {
                    Dodge(touchEndPos.x > Screen.width / 2.0f);
                }
                else if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                {
                    ChangeGravity(dir.x > 0 ? 90 : -90);
                }
                else if (dir.y > 0)
                {
                    ChangeGravity(180);
                }
            }
        }
        else
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

        animator.SetBool("IsGrounded", Physics.Raycast(
            transform.position, 
            Vector3.Normalize(Physics.gravity), 
            distToGround + 0.1f));
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

        if (Mathf.Abs(rotate) > eps)  // Finish the last rotation
            transform.Rotate(Vector3.forward * rotate);

        Quaternion rot = Quaternion.Euler(0, 0, angle);

        Physics.gravity = rot * Physics.gravity;

        Vector3 prevRightVec = rightVec;
        rightVec = rot * rightVec;

        // TODO: See if there's a cleaner way to do this
        // Check if it's turning left or right
        if (Mathf.Abs(Vector3.Dot(rightVec, prevRightVec)) < eps)
        {   
            transform.position = Vector3.Scale(
                new Vector3(Mathf.Abs(prevRightVec.x), Mathf.Abs(prevRightVec.y), 1), 
                transform.position) - Mathf.Sign(angle) * MOVE * rightVec;
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

        Color c = Color.red;
        renderer.material.EnableKeyword("_EMISSION");

        // Temporarily ignore collision between player and obstacles
        Physics.IgnoreLayerCollision(3, 6, true);

        // Make visible again after few seconds
        Invoke("MakeVisible", 3.0f);
    }

    void MakeVisible()
    {
        Color c = Color.white;
        // renderer.material.SetColor("_Color", c);
        renderer.material.DisableKeyword("_EMISSION");
        Physics.IgnoreLayerCollision(3, 6, false);
    }

}
