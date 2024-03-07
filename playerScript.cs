using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    // Sounnd
    public AudioSource sGulpSFX; // Reference to the AudioSource component

    // Sprites
    private SpriteRenderer gPlayer;

    // Reference to BG image
    public GameObject backgroundObject; // Reference to the object with the SpriteRenderer
    private SpriteRenderer backgroundSpriteRenderer;
    private Bounds backgroundBounds;

    // Movement
    public float accelSpeed  = 0.5f;
    public float deccelSpeed = 0.07f;
    public float velMax     = 4f;

    private float horizontal;
    private float vertical;

    private float velX;
    private float velY;

    private bool moveL = false;
    private bool moveR = false;
    private bool moveU = false;
    private bool moveD = false;
    private bool moving = false;

    private string facing = "right";

    // Start is called before the first frame update
    void Start()
    {
        // Get player sprite component
        gPlayer = GetComponent<SpriteRenderer>();        
        
        // Get the SpriteRenderer component of the background object
        backgroundSpriteRenderer = backgroundObject.GetComponent<SpriteRenderer>();

        // Get the bounds of the background object
        backgroundBounds = backgroundSpriteRenderer.bounds;
    }

    // Update is called once per frame
    void Update()
    {   
        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////
        // Get horizontal input
        float horizontalInput = Input.GetAxis("Horizontal");

        // Get vertical input
        float verticalInput = Input.GetAxis("Vertical");

        // Handle movement X
        if (horizontalInput < 0) {
            moveL = true;
        }
        else if (horizontalInput > 0) {
            moveR = true;
        } 
        else if (horizontalInput > -1 && horizontalInput < 1) {
            moveL = false;
            moveR = false;
        }

        // Handle movement Y
        if (verticalInput < 0) {
            moveU = true;
        }
        else if (verticalInput > 0) {
            moveD = true;
        } 
        else if (verticalInput > -1 && verticalInput < 1) {
            velY = velY - velY * deccelSpeed;
            moveU = false;
            moveD = false;
        }

        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////

        // Handle movement
        if (moveL) {
            velX -= accelSpeed;
            moving = true;
            facing = "left";
        }
        else if (moveR) {
            velX += accelSpeed;
            moving = true;
            facing = "right";
        }
        if (moveU) {
            velY -= accelSpeed;
            moving = true;
        }
        else if (moveD) {
            velY += accelSpeed;
            moving = true;
        }

        // Stop movement check
        if (!moveL && !moveR && !moveU && !moveD) {
            moving = false;
        }

        // Decceleration
        if (!moving) {
            velX = velX - velX * deccelSpeed;
        }

        // Set max movement
        if (velX < -velMax) {
            velX = -velMax;
        }
        if (velX > velMax) {
            velX = velMax;
        }
        if (velY < -velMax) {
            velY = -velMax;
        }
        if (velY > velMax) {
            velY = velMax;
        }

        // Calculate movement amount
        Vector2 movement = new Vector2(velX, velY) * Time.deltaTime;

        // Move the player
        transform.Translate(movement); 
        
        // Player map bounds
        WrapAround();

        // Animations
        if (facing=="left") {
            gPlayer.flipX = true;
        }
        if (facing=="right") {
            gPlayer.flipX = false;
        }
    }

    // Wrap around the player position if it goes beyond the background bounds
    private void WrapAround()
    {
        Vector3 position = transform.position;
        Collider2D playerCollider = GetComponent<Collider2D>();

        float offsetX = 0f;
        float offsetY = 0f;

        if (playerCollider != null)
        {
            // Calculate the horizontal and vertical extents of the player's collider bounds
            offsetX = playerCollider.bounds.extents.x;
            offsetY = playerCollider.bounds.extents.y;
        }

        float width = backgroundBounds.max.x - backgroundBounds.min.x;
        float height = backgroundBounds.max.y - backgroundBounds.min.y;

        float backgroundLeftEdge = backgroundBounds.min.x;
        float backgroundRightEdge = backgroundBounds.max.x;
        float backgroundTopEdge = backgroundBounds.min.y;
        float backgroundBottomEdge = backgroundBounds.max.y;

        float playerLeftEdge = position.x - offsetX;
        float playerRightEdge = position.x + offsetX;
        float playerTopEdge = position.y - offsetY;
        float playerBottomEdge = position.y + offsetY;

        // Loop player around bg
        if (playerLeftEdge > backgroundRightEdge)
        {
            position.x = backgroundLeftEdge - offsetX;
        }
        if (playerRightEdge < backgroundLeftEdge)
        {
            position.x = backgroundRightEdge + offsetX;
        }
        if (playerTopEdge > backgroundBottomEdge)
        {
            position.y = backgroundTopEdge - offsetY;
        }
        if (playerBottomEdge < backgroundTopEdge)
        {
            position.y = backgroundBottomEdge + offsetY;
        }

        transform.position = position;
    }

    public void OnCollisionEnter2D(Collision2D  collision)
    {

        // Check if the collision involves the player
        if (collision.gameObject.tag =="FishTag")
        {
            // Play the audio clip
            sGulpSFX.Play();

            // Remove the object
            Destroy(collision.gameObject);
        }
    }
}
