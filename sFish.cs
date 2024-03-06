using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sFish : MonoBehaviour
{
    // Sounnd
    public AudioClip collisionSound; // Sound effect to play
    private AudioSource audioSource; // Reference to the AudioSource component

    // Sprites
    private SpriteRenderer gFish;

    // Reference to BG image
    private GameObject backgroundObject; // Reference to the object with the SpriteRenderer
    private SpriteRenderer backgroundSpriteRenderer;
    private Bounds backgroundBounds;

	// Moving around
	/*
	 * 0: idle
	 * 1: wander up
	 * 2: wander right
	 * 3: wander down
	 * 4: wander left
	 */
	private int action;
	private float actionTimer;
	private float actionLength;

    // Movement
    public float accelSpeed = 0.2f;
    public float deccelSpeed = 0.08f;
    public float velMax = 4f;
    private float vX;
    private float vY;

    // String
    public string tagToFind = "gBG"; // Tag to search for
    
    // Start is called before the first frame update
    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();

        // Get player sprite component
        gFish = GetComponent<SpriteRenderer>();
        
        // Get the SpriteRenderer component of the background object
        //backgroundSpriteRenderer = backgroundObject.GetComponent<SpriteRenderer>();
        GameObject backgroundObjectTagged = GameObject.FindGameObjectWithTag(tagToFind);

        // Check if the GameObject with the tag was found
        if (backgroundObjectTagged != null)
        {
            // Get the SpriteRenderer component from the GameObject
            backgroundSpriteRenderer = backgroundObjectTagged.GetComponent<SpriteRenderer>();

            // Check if the SpriteRenderer component was found
            if (backgroundSpriteRenderer != null)
            {
                // Component found, you can now use it
                // Example: backgroundSpriteRenderer.color = Color.red;
            }
            else
            {
                //Debug.LogWarning("SpriteRenderer component not found on GameObject with tag: " + tagToFind);
            }
        }
        else
        {
            //Debug.LogWarning("GameObject with tag not found: " + tagToFind);
        }

        // Get the bounds of the background object
        backgroundBounds = backgroundSpriteRenderer.bounds;

        // Set random action
        actionTimer     = 0;
        actionLength    = (Random.Range(0, 3)) + 1 * Time.deltaTime;
        action          = Random.Range(0, 5);		// rand act start
    }

    // Update is called once per frame
    void Update()
    {
        // Handle idle
        if (action == 0)
            {
                actionTimer += Time.deltaTime;
                //animSpe = 0.25;

                if (actionTimer > actionLength)
                {
                    RandAct();
                }
            }

            // Wander up
            if (action == 1)
            {
                vY -= accelSpeed;
                //animSpe = 1;

                // Do timer
                actionTimer += Time.deltaTime;
                if (actionTimer > actionLength)
                {
                    RandAct();
                }
            }

            // Wander right
            if (action == 2)
            {
                vX += accelSpeed;
                //animSpe = 1;

                // Do timer
                actionTimer += Time.deltaTime;
                if (actionTimer > actionLength)
                {
                    RandAct();
                }
            }

            // Wander down
            if (action == 3)
            {
                vY += accelSpeed;
                //animSpe = 1;

                // Do timer
                actionTimer += Time.deltaTime;
                if (actionTimer > actionLength)
                {
                    RandAct();
                }
            }

            // Wander left
            if (action == 4) {
                vX -= accelSpeed;
                //animSpe = 1;

                // Do timer
                actionTimer += Time.deltaTime;
                if (actionTimer > actionLength)
                {
                    RandAct();
                }
            }

            // If idle stop movement
            if (action == 0) {
                vX = vX - vX * deccelSpeed;
                vY = vY - vY * deccelSpeed;
            }

            // Fish vel max
            if (vX > velMax) {
                vX = velMax;
            }
            if (vY > velMax) {
                vY = velMax;
            }
            if (vX < -velMax) {
                vX = -velMax;
            }
            if (vY < -velMax) {
                vY = -velMax;
            }


            // Calculate movement amount
            Vector2 movement = new Vector2(vX, vY) * Time.deltaTime;

            // Move the fish
            transform.Translate(movement); 

            // Fish map bounds
            WrapAround();

            /// Animations ///

            // Facing direction for flip
            if (vX <= 0) {
                gFish.flipX = true;
            } else {
                gFish.flipX = false;
            }

            // Fish death
           /* if (checkCollision( targetX, targetY, targetW, targetH, x, y, w, h)) {

                // Spawn gulp texture
                obj->Spawn(objects,x+w/2, y+h/2, 134, 68, 0);

                // Spawn text in mouth location
                obj->Spawn(objects,targetX+targetW/2, targetY+targetH/2, 32, 32, 1);

                score += 20;
                hunger += 25;

                alive = false;
                this->count--;

                Mix_PlayChannel(-1, sGulp, 0);
            }*/
        
    }

    // Wrap around the fish position if it goes beyond the background bounds
    private void WrapAround()
    {
        Vector3 position = transform.position;
        Collider2D fishCollider = GetComponent<Collider2D>();

        float offsetX = 0f;
        float offsetY = 0f;

        if (fishCollider != null)
        {
            // Calculate the horizontal and vertical extents of the fish's collider bounds
            offsetX = fishCollider.bounds.extents.x;
            offsetY = fishCollider.bounds.extents.y;
        }

        float width = backgroundBounds.max.x - backgroundBounds.min.x;
        float height = backgroundBounds.max.y - backgroundBounds.min.y;

        float backgroundLeftEdge = backgroundBounds.min.x;
        float backgroundRightEdge = backgroundBounds.max.x;
        float backgroundTopEdge = backgroundBounds.min.y;
        float backgroundBottomEdge = backgroundBounds.max.y;

        float fishLeftEdge = position.x - offsetX;
        float fishRightEdge = position.x + offsetX;
        float fishTopEdge = position.y - offsetY;
        float fishBottomEdge = position.y + offsetY;

        // Loop fish around bg
        if (fishLeftEdge > backgroundRightEdge)
        {
            position.x = backgroundLeftEdge - offsetX;
        }
        if (fishRightEdge < backgroundLeftEdge)
        {
            position.x = backgroundRightEdge + offsetX;
        }
        if (fishTopEdge > backgroundBottomEdge)
        {
            position.y = backgroundTopEdge - offsetY;
        }
        if (fishBottomEdge < backgroundTopEdge)
        {
            position.y = backgroundBottomEdge + offsetY;
        }

        transform.position = position;
    }

	// Play fish action
	void RandAct() {

		// set random action
		action = Random.Range(0, 5);

		// reset action timer
		actionTimer = 0;

		// set random action length from 1 - 3 seconds
		actionLength = (Random.Range(0, 3)) + 1 * Time.deltaTime;
	}

    void OnCollisionEnter2D(Collision2D  collision)
    {
        // Check if the collision involves the player
        if (collision.gameObject.CompareTag("Player"))
        {
        // Play the collision sound effect
        if (collisionSound != null && audioSource != null)
        {
            Debug.Log("Playing collision sound effect");
            audioSource.PlayOneShot(collisionSound);
        }
        else
        {
            Debug.LogWarning("Audio clip or audio source is null");
        }

            // Remove the object
            Destroy(gameObject);
        }
    }
}

