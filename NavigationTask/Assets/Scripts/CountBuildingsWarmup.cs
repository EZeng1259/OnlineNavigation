using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class CountBuildingsWarmup : MonoBehaviour
{
    public TMP_Text toptext;
    public int buildingCounter = 0;
    public GameObject arrow;
    private Boolean reachedArrow; 

    public float idleTimeLimit = 600f; // 10 minutes in seconds
    private float idleTimer = 0f;

    float minDist = 5f;
    List<string> buildingsVisited = new List<string>();

    float time = 0f;
    private float startTime;
    [SerializeField] float interval = 150f;

    private static string apiUrl = "https://infinite-cliffs-00012-b13a116af8e3.herokuapp.com/warmupnavigation"; // API URL

    [System.Serializable]
    public class Datapoint
    {
        public string participantID;
        public float timestamp;
        public float x;
        public float y;
        public float rotx;
        public float roty;

        public Datapoint(string id, float currTime, float x, float y, float rotate_x, float rotate_y)
        {
            this.participantID = id;
            this.timestamp = currTime;
            this.x = x;
            this.y = y;
            this.rotx = rotate_x;
            this.roty = rotate_y;
        }
    }

    public List<Datapoint> dataPoints = new List<Datapoint>();

    void Start()
    {
        Screen.fullScreen = true; 
        reachedArrow = false; 
        startTime = Time.time;
        toptext.SetText("Look to the left to find the starting white arrow on the ground");
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

        float dist = Vector3.Distance(FindClosestRedBuilding().GetComponentInChildren<TMP_Text>().transform.position, transform.position);

        if ((arrow.transform.position - transform.position).sqrMagnitude > minDist && !reachedArrow)
        {
            toptext.SetText("Look to the left to find the starting white arrow on the ground");
        }
        else if((arrow.transform.position - transform.position).sqrMagnitude <= minDist)
        {
            reachedArrow = true; 
        }
        else if (buildingsVisited.Contains(FindClosestRedBuilding().name))
        {
            toptext.SetText("Follow the white arrows");
        }
        else
        {
            toptext.SetText("Follow the white arrows");
            if (dist <= minDist)
            {
                toptext.SetText("Press 'Spacebar' when you are directly in front of the name of a red building to confirm your visit");
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    buildingCounter++;
                    Debug.Log(buildingCounter);
                    buildingsVisited.Add(FindClosestRedBuilding().name);
                    foreach (TMP_Text g in FindClosestRedBuilding().GetComponentsInChildren<TMP_Text>())
                    {
                        g.color = new Color(0, 0, 0);
                    }
                }
            }
        }

        if (buildingCounter == 3)
        {
            toptext.SetText("Freely explore the environment and press 'Return' when you're ready to move on");

            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("NavigationSlide1");
            }
        }

        time += Time.deltaTime;
        if (time > interval)
        {
            float x = transform.position.x;
            float y = transform.position.z;
            float rotate_x = CameraMovement.yRotation % 360;
            float rotate_y = (CameraMovement.yRotation % 360) * -1;
            float currTime = Time.time - startTime;

            Datapoint newPoint = new Datapoint(PlayerID.id, currTime, x, y, rotate_x, rotate_y);

            dataPoints.Add(newPoint);
            time = 0;
        }
        else
        {
            time++;
        }

        // writeData();
        // dataPoints.Clear();
    }

    /*
    public void writeData()
    {
        if (dataPoints.Count > 0)
        {
            foreach (var data in dataPoints)
            {
                SendDataToServer(data);
            }
        }
    }

    public void SendDataToServer(Datapoint data)
    {
        //Debug.Log("SendData method called.");
        StartCoroutine(SendDataCoroutine(data));
    }

    private IEnumerator SendDataCoroutine(Datapoint data)
    {
        //Debug.Log("Preparing data to send...");

        string jsonData = JsonUtility.ToJson(data);
        //Debug.Log("JSON data prepared: " + jsonData);

        UnityWebRequest www = new UnityWebRequest(apiUrl, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        //Debug.Log("Sending request to server...");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error sending data to WarmupNavigation: " + www.error);
        }
        else
        {
            Debug.Log("Data sent successfully to WarmupNavigation table");
        }
    }
    */

    public GameObject FindClosestRedBuilding()
    {
        GameObject[] redBuildings;
        redBuildings = GameObject.FindGameObjectsWithTag("RedBuilding");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in redBuildings)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
}
