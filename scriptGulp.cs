using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptGulp : MonoBehaviour
{
    public float targetScaleMultiplier = 1.2f; // Target scale multiplier
    public float animationDuration = 0.3f; // Duration of the expansion and shrinking animation in seconds

    private Vector3 initialScale; // Initial scale of the prefab

    private void Start()
    {
        // Store the initial scale
        initialScale = transform.localScale;

        // Start the scaling coroutine
        StartCoroutine(ExpandAndShrinkAndDestroy());
    }

    // Coroutine to expand the prefab, then shrink it back, and finally destroy it
    private IEnumerator ExpandAndShrinkAndDestroy()
    {
        float elapsedTime = 0f;
        float halfDuration = animationDuration * 0.5f;

        // Part 1: Expand the prefab
        while (elapsedTime < halfDuration)
        {
            float scaleMultiplier = Mathf.Lerp(1f, targetScaleMultiplier, elapsedTime / halfDuration);
            Vector3 newScale = initialScale * scaleMultiplier;
            transform.localScale = newScale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Part 2: Shrink the prefab back
        while (elapsedTime < animationDuration)
        {
            float scaleMultiplier = Mathf.Lerp(targetScaleMultiplier, 1f, (elapsedTime - halfDuration) / halfDuration);
            Vector3 newScale = initialScale * scaleMultiplier;
            transform.localScale = newScale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the prefab reaches the initial scale
        transform.localScale = initialScale;

        // Remove the prefab immediately
        Destroy(gameObject);
    }
}
