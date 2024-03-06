using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sCameraController : MonoBehaviour
{    

    public Transform target; // Reference to the player's Transform
    public GameObject backgroundObject; // Reference to the object with the SpriteRenderer
    public float followSpeed = 5f; // Speed at which the camera follows the player
    public float damping = 0.1f; // Damping factor for smooth movement

    private SpriteRenderer backgroundSpriteRenderer;
    private Bounds backgroundBounds;

    private Camera mainCamera;

    void Start()
    {
        // Get the SpriteRenderer component of the background object
        backgroundSpriteRenderer = backgroundObject.GetComponent<SpriteRenderer>();

        // Get the bounds of the background object
        backgroundBounds = backgroundSpriteRenderer.bounds;
        
        // Get the main camera
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            /////////////////////////////////////////////////////
            ///////////////// Center Camera to Player ///////////
            // Get the current position of the camera
            Vector3 currentPosition = transform.position;

            // Get the target position centered on the player
            float targetX = Mathf.Clamp(target.position.x, backgroundBounds.min.x, backgroundBounds.max.x);
            float targetY = Mathf.Clamp(target.position.y, backgroundBounds.min.y, backgroundBounds.max.y);
            Vector3 targetPosition = new Vector3(targetX, targetY, currentPosition.z);

            // Smoothly interpolate between the current position and the target position
            Vector3 newPosition = Vector3.Lerp(currentPosition, targetPosition, followSpeed * Time.deltaTime);

            // Apply damping to the interpolation for smoother movement
            transform.position = Vector3.Lerp(currentPosition, newPosition, damping);
            ///////////////// Center Camera to Player ///////////
            /////////////////////////////////////////////////////

            /////////////////////////////////////////////////////
            /// Bound the camera to the background img object ///
            // Calculate camera bounds
            float cameraHeight = 2f * mainCamera.orthographicSize;
            float cameraWidth = cameraHeight * mainCamera.aspect;
            float cameraMinX = backgroundBounds.min.x + cameraWidth / 2;
            float cameraMaxX = backgroundBounds.max.x - cameraWidth / 2;
            float cameraMinY = backgroundBounds.min.y + cameraHeight / 2;
            float cameraMaxY = backgroundBounds.max.y - cameraHeight / 2;

            // Get the current camera position
            Vector3 cameraPosition = mainCamera.transform.position;

            // Clamp camera position to stay within background bounds
            float clampedX = Mathf.Clamp(cameraPosition.x, cameraMinX, cameraMaxX);
            float clampedY = Mathf.Clamp(cameraPosition.y, cameraMinY, cameraMaxY);

            // Update camera position
            mainCamera.transform.position = new Vector3(clampedX, clampedY, cameraPosition.z);
            /// Bound the camera to the background img object ///
            /////////////////////////////////////////////////////
        }
    }
}
