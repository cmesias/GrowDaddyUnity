using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    // Gulp popup
    public GameObject prefabGulp; // Reference to the prefab to spawn

    // Sounnd
    public AudioSource sGulpSFX; // Reference to the AudioSource component
    public AudioSource sDashSFX; // Reference to the AudioSource component

    // Animations
    public Animator animator;

    // Sprites
    private SpriteRenderer gPlayer;

    // Reference to BG image
    public GameObject backgroundObject; // Reference to the object with the SpriteRenderer
    private SpriteRenderer backgroundSpriteRenderer;
    private Bounds backgroundBounds;

    // Movement
    public float accelSpeed  = 1f;
    public float deccelSpeed = 0.04f;
    public float velMax     = 4f;

    private float horizontal;
    private float vertical;

    private float velX, velY;   // regualr movement
    private float velX2, velY2; // used for boosting
    private float M_PI =  3.14159265358979323846264338327950288f;   /* pi */
    private float angle = 0f;

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
        // Spacebar is pressed, perform your actions here
        if (Input.GetKeyDown(KeyCode.Space))
        {
			DashAtAngle(10);
        }

        // A button on Xbox controller is pressed, perform your actions here
        if (Input.GetButtonDown("Jump"))
        {
			DashAtAngle(10);
        }

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

	    // Player angle
        angle = Mathf.Atan2(velY, velX) * Mathf.Rad2Deg;
		if (angle < 0) {
			angle = 360 - (-angle);
		}

        // Handle movement
        if (moveL) {
            if (velX > -velMax) {
                velX -= accelSpeed;
            }
            moving = true;
            facing = "left";
        }
        else if (moveR) {
            if (velX < velMax) {
                velX += accelSpeed;
            }
            moving = true;
            facing = "right";
        }
        if (moveU) {
            if (velY > -velMax) {
                velY -= accelSpeed;
            }
            moving = true;
        }
        else if (moveD) {
            if (velY < velMax) {
                velY += accelSpeed;
            }
            moving = true;
        }

		// Max velocity
		if (velX > 10){
			velX = 10;
		}
		if (velX < -10){
			velX = -10;
		}
		if (velY > 10){
			velY = 10;
		}
		if (velY < -10){
			velY = -10;
		}

		// Max velocity
		if (velX2 > 10){
			velX2 = 10;
		}
		if (velX2 < -10){
			velX2 = -10;
		}
		if (velY2 > 10){
			velY2 = 10;
		}
		if (velY2 < -10){
			velY2 = -10;
		}

        // Calculate movement amount
        float valueVX = velX+velX2;
        float valueVY = velY+velY2;
        Vector2 movement = new Vector2(valueVX, valueVY) * Time.deltaTime;

        // Move the player
        transform.Translate(movement); 

        // Decceleration
        if (!moveU && !moveD) {
            velY = velY - velY * deccelSpeed;
        }
        if (!moveL && !moveR) {
            velX = velX - velX * deccelSpeed;
        }
		velX2 = velX2 - velX2 * deccelSpeed;
		velY2 = velY2 - velY2 * deccelSpeed;

        // Stop movement check
        if (!moveL && !moveR && !moveU && !moveD) {
            moving = false;
        }

        /// Animations ///
        // NOT Moving
        if (!moving) {
            animator.SetBool("Swimming", false);
        } 
        
        // Moving
        else {
            animator.SetBool("Swimming", true);
        }
        
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
            // Determine the center position of the player's body
            Vector3 fishCenterPosition = collision.gameObject.transform.position;

            Instantiate(prefabGulp, fishCenterPosition, Quaternion.identity);

            // Play the audio clip
            sGulpSFX.Play();

            // Remove the object
            Destroy(collision.gameObject);
        }
    }

    
    // Metheods for player
    void DashAtAngle(float value) {

        // Update particles angle based on its X and Y velocities
        //particle[i].angle = atan2 ( particle[i].vY, particle[i].vX) * 180 / 3.14159265;
        velX2 += Mathf.Cos(angle * Mathf.Deg2Rad) * value;
        velY2 += Mathf.Sin(angle * Mathf.Deg2Rad) * value;

        // Play SFX
        sDashSFX.Play();

        // Spawn VFX
        //part.SpawnBubblesVFX(particle, tailX+tailW/2, tailY+tailH/2);

    }
}
