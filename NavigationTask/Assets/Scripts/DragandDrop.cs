using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class DragandDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private float startTime;
    public Image image;

    [System.Serializable]
    public class MapPlacement
    {
        public string participantID;
        public float timestamp;
        public string buildingName;

        public MapPlacement(float timestamp, string buildingName)
        {
            participantID = PlayerID.id;
            this.timestamp = timestamp;
            this.buildingName = buildingName;
        }
    }

    public static List<string> alreadyDragged = new List<string>();
    private static int counter = 0;
    private static string apiUrl = "https://infinite-cliffs-00012-b13a116af8e3.herokuapp.com/mapplacementorder"; // Updated API URL
    private static string apiUrl2 = "https://infinite-cliffs-00012-b13a116af8e3.herokuapp.com/mapcoordinates"; // Updated API URL

    private float accumulatedTime = 0f;
    private bool isTimerRunning = false;

    [SerializeField] GameObject[] Objects;

    [System.Serializable]
    public class MapCoords
    {
        public string participantID;
        public string buildingName;
        public float xpos;
        public float ypos;

        public MapCoords(string buildingName, float xpos, float ypos)
        {
            participantID = PlayerID.id;
            this.buildingName = buildingName;
            this.xpos = xpos;
            this.ypos = ypos;
        }
    }

    private void Start()
    {
        startTime = Time.time;

        if (counter == 0)
        {
            counter = 1;
        }

        // Start the timer
        isTimerRunning = true;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            accumulatedTime += Time.deltaTime;

            if (accumulatedTime >= 120f) // 2xf minutes
            {
                isTimerRunning = false;
                RecordLocations();
                SceneManager.LoadScene("Finished");
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition;
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Begin Drag
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        RectTransform imageCoords = image.rectTransform;
        Vector3[] corners = new Vector3[4];
        imageCoords.GetWorldCorners(corners);

        if (!alreadyDragged.Contains(rectTransform.name))
        {
            if (rectTransform.position.x < corners[2].x && rectTransform.position.x > corners[0].x && rectTransform.position.y < corners[2].y && rectTransform.position.y > corners[0].y)
            {
                float currTime = Time.time - startTime;
                //SendMapPlacementOrderToServer(currTime, rectTransform.name);
                alreadyDragged.Add(rectTransform.name);
            }
        }
    }

    /*
    public void SendMapPlacementOrderToServer(float timestamp, string buildingName)
    {
        StartCoroutine(SendMapPlacementOrderCoroutine(timestamp, buildingName));
    }

    private IEnumerator SendMapPlacementOrderCoroutine(float timestamp, string buildingName)
    {
        MapPlacement data = new MapPlacement(timestamp, buildingName);

        //Debug.Log("Preparing data to send: " + JsonUtility.ToJson(data));
        string jsonData = JsonUtility.ToJson(data);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(apiUrl, "POST"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error sending data to mapplacementorder table: " + www.error);
            }
            else
            {
                Debug.Log("Map placement order data sent successfully");
            }
        }
    }
    */

    public void RecordLocations()
    {
        string[] objNames = new string[19]
        {
            "Gym", "Hardware Store", "Music Store", "Pharmacy",  "Bakery", "Bank", "Dentist", "Cafe",
            "Jewelry", "Butcher", "Supermarket", "Bike Shop", "Pizzeria", "Toy Store", "Book Store",
            "Barber", "Boutique", "Gallery", "Pet Store"
        };

        for (int i = 0; i < Objects.Length; i++)
        {
            float xPos = Objects[i].transform.position.x;
            float yPos = Objects[i].transform.position.y;
            //SendMapCoordinatesToServer(PlayerID.id, Objects[i].name, xPos, yPos);
        }

        // Move to next scene
        SceneManager.LoadScene("Finished");
    }

    /*
    public void SendMapCoordinatesToServer(string playerId, string storeName, float xPos, float yPos)
    {
        StartCoroutine(SendMapCoordinatesCoroutine(storeName, xPos, yPos));
    }

    private IEnumerator SendMapCoordinatesCoroutine(string storeName, float xPos, float yPos)
    {
        MapCoords data = new MapCoords(storeName, xPos, yPos);

        //Debug.Log("Preparing data to send: " + JsonUtility.ToJson(data));
        string jsonData = JsonUtility.ToJson(data);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(apiUrl2, "POST"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error sending data to mapcoordinates table: " + www.error);
            }
            else
            {
                Debug.Log("Map coordinates data sent successfully");
            }
        }
    }
    */
}
