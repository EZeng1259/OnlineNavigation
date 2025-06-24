using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; 

public class Break : MonoBehaviour
{
    public Button button;
    public TMP_Text roundsLeft; 
    public float timeToWait = 30f;
    private float currentWaitTime;
    private bool checkTime;

    public float idleTimeLimit = 600f; // 10 minutes in seconds
    private float idleTimer = 0f;

    void Awake()
    {
        ResetTimer();
    }

    private void Start()
    {
        roundsLeft.SetText((8 - CountBuildings.trialNum).ToString());
    }

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

        if (checkTime)
        {
            currentWaitTime -= Time.deltaTime;
            button.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.Round(currentWaitTime).ToString();
            if (currentWaitTime < 0)
            {
                TimerFinished();
                checkTime = false;
            }
        }
    }

    public void ResetTimer()
    {
        currentWaitTime = timeToWait;
        checkTime = true;
        button.interactable = false; 
    }
    void TimerFinished()
    {
        button.interactable = true;
        button.GetComponentInChildren<TextMeshProUGUI>().text = "Continue"; 
        button.onClick.AddListener(changeScene);
    }

    void changeScene()
    {
        SceneManager.LoadScene("ReminderScene");
    }
}
