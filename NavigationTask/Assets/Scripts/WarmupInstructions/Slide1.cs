using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;

public class Slide1 : MonoBehaviour
{
    public float idleTimeLimit = 600f; // 10 minutes in seconds
    private float idleTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown || Input.anyKey || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            // Reset the idle timer when there is any input
            idleTimer = 0f;
        }
        else
        {
            // Increment the idle timer
            idleTimer += Time.deltaTime;
        }

        // Check if the idle timer exceeds the idle time limit
        if (idleTimer >= idleTimeLimit)
        {
            // Close the application
            SceneManager.LoadScene("WarningScene");
        } 

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SceneManager.LoadScene("WarmupSlide2");
        }
    }
}
