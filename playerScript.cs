using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    public Rigidbody2D rb;

    public float moveSpeed;
    public float jumpForce;
    public float fallMultiplier;

    private float horizontal;
    private float vertical;
    Vector2 vecGravity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //vecGravity = new Vector2(0, -Physics2D.gravity.y);

        // Moving left and right
        horizontal = Input.GetAxis("Horizontal") * moveSpeed* Time.deltaTime;

        // Moving up and down
        vertical = Input.GetAxis("Vertical") * moveSpeed* Time.deltaTime;

        // Apply movement to Player X & Y rigidbody
        transform.Translate(new Vector2(horizontal, vertical));

        // Jumping
        if (Input.GetButtonDown("Jump"))
        {
            //rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Increase fall speed if moving down
        if (rb.velocity.y < 0)
        {
            //rb.velocity -= vecGravity * fallMultiplier * Time.deltaTime;
        }
    }
}
