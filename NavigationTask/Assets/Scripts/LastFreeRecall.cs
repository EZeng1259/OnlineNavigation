using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LastFreeRecall : MonoBehaviour
{
    public TMP_InputField input;

    public float beginTime; 

    private static string apiUrl = "https://infinite-cliffs-00012-b13a116af8e3.herokuapp.com/freerecall"; // Updated API URL

    [System.Serializable]
    public class Recall
    {
        public string participantID;
        public float timestamp;
        public string buildingName;

        public Recall(float timestamp, string buildingName)
        {
            participantID = PlayerID.id;
            this.timestamp = timestamp;
            this.buildingName = buildingName;
        }

        public Recall()
        {
            participantID = PlayerID.id;
            this.timestamp = 0;
            this.buildingName = "";
        }
    }

    public List<Recall> itemList = new List<Recall>();
    public static List<string> wordList = new List<string>();

    void Start()
    {
        beginTime = Time.time; 

        wordList.Clear();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        input.ActivateInputField();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            wordList.Add(input.text.Trim().ToLower());

            float currTime = Time.time - beginTime; 
            Recall item = new Recall(currTime, input.text);
            itemList.Add(item);
            input.text = "";
            input.ActivateInputField();
        }

        //writeList();
        //itemList.Clear();
    }

    /*
    public void writeList()
    {
        if (itemList.Count > 0)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                SendDataToServer(PlayerID.id, itemList[i].timestamp, itemList[i].buildingName);
            }
        }
    }

    public void SendDataToServer(string playerId, float timestamp, string buildingName)
    {
        StartCoroutine(SendDataCoroutine(timestamp, buildingName));
    }

    private IEnumerator SendDataCoroutine(float timestamp, string buildingName)
    {
        Recall data = new Recall(timestamp, buildingName);

        string jsonData = JsonUtility.ToJson(data);
        //Debug.Log("JSON data prepared: " + jsonData);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(apiUrl, "POST"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error sending data to freerecall table: " + www.error);
            }
            else
            {
                Debug.Log("Data sent successfully to FreeRecall table");
            }
        }
    }
    */
}
