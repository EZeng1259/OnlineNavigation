using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationSceneTimer : MonoBehaviour
{
    private float accumulatedTimeInNavigationScene = 0f;
    private bool isInNavigationScene = false;

    private static NavigationSceneTimer instance;

    void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "NavigationScene")
        {
            isInNavigationScene = true;
        }
        else
        {
            isInNavigationScene = false;
        }
    }

    void Update()
    {
        if (isInNavigationScene)
        {
            accumulatedTimeInNavigationScene += Time.deltaTime;

            if (accumulatedTimeInNavigationScene >= 3600f)
            {
                ActivateFunction();
                // Optionally, reset the timer if you want the function to be called again after another hour
                accumulatedTimeInNavigationScene = 0f;
            }
        }
    }

    // Function to be activated after one hour in NavigationScene
    void ActivateFunction()
    {
        Debug.Log("Function activated after one hour in NavigationScene!");


        if(CountBuildings.trialNum == 2 || CountBuildings.trialNum == 3)
        {
            SceneManager.LoadScene("StartRecallScene");
        }

    }
}
