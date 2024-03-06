using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sGameController : MonoBehaviour
{

    public GameObject prefabFishToSpawn; // Reference to the prefab to spawn

    // Reference to BG image
    public GameObject backgroundObject; // Reference to the object with the SpriteRenderer
    private SpriteRenderer backgroundSpriteRenderer;
    private Bounds backgroundBounds;

    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;

        // Get the SpriteRenderer component of the background object
        backgroundSpriteRenderer = backgroundObject.GetComponent<SpriteRenderer>();

        // Get the bounds of the background object
        backgroundBounds = backgroundSpriteRenderer.bounds;
    }

    // Update is called once per frame
    void Update()
    {
        float mapX = backgroundBounds.min.x;
        float mapY = backgroundBounds.min.y;
        float mapW = backgroundBounds.max.x - backgroundBounds.min.x;
        float mapH = backgroundBounds.max.y - backgroundBounds.min.y;

        // Spawn random fish outside of the camers view
		timer += Random.Range(0, 5) + 1 * Time.deltaTime;

		if (timer > 60) {
			timer = 0f;

			float randX = Random.Range(mapX, mapW) * 0.75f;
			float randY = Random.Range(mapY, mapH) * 0.75f;

	       // do {
			//	randX = Random.Range(backgroundBounds.min.x, backgroundBounds.max.x) * 0.75f;
			//	randY = Random.Range(backgroundBounds.min.y, backgroundBounds.max.y) * 0.75f;
	        //} while (!isOutsideView(randX, randY, mapX, mapY, mapW, mapH)); 

            // Spawn an instance of the prefab at the random position with no rotation
            Instantiate(prefabFishToSpawn, new Vector3(randX, randY, 0f), Quaternion.identity);
		}
    }

    
    bool isOutsideView(float x, float y, float camx, float camy, float camw, float camh) {
        return x < camx || x > camx + camw || y < camy || y > camy + camh;
    }
}
