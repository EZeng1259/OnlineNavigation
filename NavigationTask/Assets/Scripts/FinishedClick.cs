using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class FinishedClick : MonoBehaviour
{
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

    [SerializeField] GameObject[] Objects;
    public Button button;
    private static string apiUrl = "https://infinite-cliffs-00012-b13a116af8e3.herokuapp.com/mapcoordinates";

    private void Start()
    {
        button.gameObject.SetActive(false);
        button.onClick.AddListener(RecordLocations);
    }

    void Update()
    {
        if (DragandDrop.alreadyDragged.Count == 19)
        {
            button.gameObject.SetActive(true);
        }
    }

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

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(apiUrl, "POST"))
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
