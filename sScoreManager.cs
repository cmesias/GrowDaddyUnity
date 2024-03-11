using UnityEngine;
using UnityEngine.UI;

public class sScoreManager : MonoBehaviour
{
    public int score = 0;
    public Text scoreText;

    void Start()
    {
        // Initialize the score text
        UpdateScoreText();
    }

    // Add points to the score
    public void AddPoints(int points)
    {
        score += points;
        UpdateScoreText();
    }

    // Update the UI Text to display the current score
    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }
}
