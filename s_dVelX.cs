using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class s_dVelX : MonoBehaviour
{
    private Text displayText; // Reference to the Text component
    
    // Start is called before the first frame update
    void Start()
    {
        // Get the Text component
        displayText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get horizontal input
        float horizontalInput = Input.GetAxis("Horizontal");

        // Display horizontal input
        displayText.text = "Input: " + horizontalInput.ToString("F2");
    }
}
