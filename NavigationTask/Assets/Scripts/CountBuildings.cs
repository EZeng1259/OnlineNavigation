using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;

public class CountBuildings : MonoBehaviour
{
    public TMP_Text input;
    public int buildingCounter = 0; //counts number of red buildings encountered

    public float idleTimeLimit = 600f; // 10 minutes in seconds
    private float idleTimer = 0f;

    public static int trialNum = 0;
    public static int score = 0;
    public static float bestTime;
    public static float newTime;
    public static float totalDistance;
    public static float bestTotalDistance = Mathf.Infinity;
    DatapointNavigation prevPoint = new DatapointNavigation();

    float minDist = 2.5f;
    List<string> buildingsVisited = new List<string>();

    float time = 0f;
    private float startTime;
    [SerializeField] float interval = 2f;

    public Transform cameraRotation;

    private static string apiUrlNavigation = "https://infinite-cliffs-00012-b13a116af8e3.herokuapp.com/navigation"; // API URL
    private static string apiUrlBuildingOrder = "https://infinite-cliffs-00012-b13a116af8e3.herokuapp.com/buildingvisitedorder"; // API URL

    [System.Serializable]
    public class DatapointNavigation
    {
        public string participantID;
        public int trialNum;
        public float timestamp;
        public float x;
        public float y;
        public float rotx;
        public float roty;

        public DatapointNavigation(float currTime, float x, float y, float rotate_x, float rotate_y)
        {
            this.participantID = PlayerID.id;
            this.trialNum = CountBuildings.trialNum;
            this.timestamp = currTime;
            this.x = x;
            this.y = y;
            this.rotx = rotate_x;
            this.roty = rotate_y;
        }

        public DatapointNavigation()
        {
            this.x = 0;
            this.y = 0; 
        }
    }

    [System.Serializable]
    public class VisitedBuildings
    {
        public string participantID;
        public int trialNum;
        public float timestamp;
        public string buildingName;

        public VisitedBuildings(float timestamp, string buildingName)
        {
            this.participantID = PlayerID.id;
            this.trialNum = CountBuildings.trialNum;
            this.timestamp = timestamp;
            this.buildingName = buildingName;
        }
    }

    public List<DatapointNavigation> dataPoints = new List<DatapointNavigation>();

    void Start()
    {
        Screen.fullScreen = true;
        startTime = Time.time;

        totalDistance = 0;
        prevPoint.x = transform.position.x;
        prevPoint.y = transform.position.y;
        trialNum++;
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
        if (buildingsVisited.Contains(FindClosestRedBuilding().name)) { }
        else
        {
            if (dist <= minDist)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    buildingCounter++;
                    buildingsVisited.Add(FindClosestRedBuilding().name);
                    String name = FindClosestRedBuilding().name;
                    foreach (TMP_Text g in FindClosestRedBuilding().GetComponentsInChildren<TMP_Text>())
                    {
                        g.color = new Color(1, 0, 0, 1);
                    }
                    input.text = "" + buildingCounter;

                    float currTime = Time.time - startTime;
                    //SendBuildingVisitedOrderToServer(currTime, name);
                }
            }
        }

        time += Time.deltaTime;
        if (time > interval)
        {
            float x = transform.position.x;
            float y = transform.position.z;
            float rotate_x = CameraMovement.yRotation % 360;
            float rotate_y = (CameraMovement.xRotation % 360) * -1;
            float currTime = Time.time - startTime;

            DatapointNavigation sample = new DatapointNavigation(currTime, x, y, rotate_x, rotate_y);
            dataPoints.Add(sample);
            time = 0;

            totalDistance += Mathf.Sqrt(Mathf.Pow((x - prevPoint.x), 2) + Mathf.Pow((y - prevPoint.y), 2));

            prevPoint.x = x;
            prevPoint.y = y;

        }

        //writeToDatabase();
        //dataPoints.Clear();
        

        if (buildingCounter == 19)
        {
            if (trialNum == 1)
            {
                bestTotalDistance = totalDistance;
                SceneManager.LoadScene("BestTimeScene");
            }
            else
            {
                if (totalDistance < bestTotalDistance)
                {
                    bestTotalDistance = totalDistance;
                    score += 5;
                    SceneManager.LoadScene("BeatBestTimeScene");
                }
                else
                {
                    SceneManager.LoadScene("FailBestTimeScene");
                }
            }
        }
    }


    /*
    public void writeToDatabase()
    {
        if (dataPoints.Count > 0)
        {
            foreach (DatapointNavigation data in dataPoints)
            {
                //Debug.Log("Writing data to database: " + JsonUtility.ToJson(data));
                SendNavigationDataToServer(data);
            }
        }
    }

    public void SendNavigationDataToServer(DatapointNavigation data)
    {
        //Debug.Log("SendNavigationData method called with data: " + JsonUtility.ToJson(data));
        StartCoroutine(SendNavigationDataCoroutine(data));
    }

    private IEnumerator SendNavigationDataCoroutine(DatapointNavigation data)
    {
        //Debug.Log("Preparing data to send: " + JsonUtility.ToJson(data));

        string jsonData = JsonUtility.ToJson(data);

        UnityWebRequest www = new UnityWebRequest(apiUrlNavigation, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        //Debug.Log("Sending request to server...");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error sending data to navigation table: " + www.error);
        }
        else
        {
            Debug.Log("Navigation data sent successfully");
        }
    }

    public void SendBuildingVisitedOrderToServer(float timestamp, string buildingName)
    {
        //Debug.Log("SendVisitedOrder method called.");
        StartCoroutine(SendBuildingVisitedOrderCoroutine(timestamp, buildingName));
    }

    private IEnumerator SendBuildingVisitedOrderCoroutine(float timestamp, string buildingName)
    {
        VisitedBuildings data = new VisitedBuildings(timestamp, buildingName);

        string jsonData = JsonUtility.ToJson(data);
        //Debug.Log("JSON data prepared: " + jsonData);

        UnityWebRequest www = new UnityWebRequest(apiUrlBuildingOrder, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        //Debug.Log("Sending request to server...");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error sending data to buildingvisitedorder table: " + www.error);
        }
        else
        {
            Debug.Log("Building visited order data sent successfully");
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
