using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.Networking;


public class LastShopCheck : MonoBehaviour
{
    List<string> wordsToCheck = LastFreeRecall.wordList;

    List<string> storeNames = new List<string>
            {"gym", "hardware store", "music store", "pharmacy", "bakery", "bank", "dentist", "cafe", "jewelry", "butcher", "supermarket",
               "bike shop", "pizzeria", "toy store", "book store", "barber", "boutique", "gallery", "pet store"};

    List<string> storesSeen = new List<string>();

    public TMP_Text rewardCounter;

    public static int recallScore = 0;

    private static string apiUrlScore = "https://infinite-cliffs-00012-b13a116af8e3.herokuapp.com/recordscore";

    [System.Serializable]
    public class ScoreRecord
    {
        public string participantID;
        public int freeRecallScore;
        public int navigationScore;
        public int totalScore; 

        public ScoreRecord(int freeRecallScore, int navigationScore, int totalScore)
        {
            participantID = PlayerID.id;
            this.freeRecallScore = freeRecallScore;
            this.navigationScore = navigationScore;
            this.totalScore = totalScore; 
        }
    }

    private void Start()
    {
        CheckWords();
        LogScore(recallScore, CountBuildings.score - recallScore, CountBuildings.score);
    }

    private void CheckWords()
    {
        for (int i = 0; i < wordsToCheck.Count; i++)
        {
            if (wordsToCheck[i].Equals("pizzaria") || wordsToCheck[i].Equals("pizzerria") || wordsToCheck[i].Equals("pizeria") || wordsToCheck[i].Equals("pizzareia") || wordsToCheck[i].Equals("pizza"))
            {
                wordsToCheck[i] = "pizzeria";
            }

            if (wordsToCheck[i].Equals("jewery") || wordsToCheck[i].Equals("jewlery") || wordsToCheck[i].Equals("jewellery") || wordsToCheck[i].Equals("jewellery") || wordsToCheck[i].Equals("jewelru"))
            {
                wordsToCheck[i] = "jewelry";
            }

            if (wordsToCheck[i].Equals("mucis") || wordsToCheck[i].Equals("musicstore"))
            {
                wordsToCheck[i] = "music";
            }

            if (wordsToCheck[i].Equals("gallary"))
            {
                wordsToCheck[i] = "gallery";
            }

            if (wordsToCheck[i].Equals("botique") || wordsToCheck[i].Equals("bontique") || wordsToCheck[i].Equals("bouqiet"))
            {
                wordsToCheck[i] = "boutique";
            }

            if (wordsToCheck[i].Equals("baurber"))
            {
                wordsToCheck[i] = "barber";
            }

            if (wordsToCheck[i].Equals("bookstore"))
            {
                wordsToCheck[i] = "book store";
            }

            if (wordsToCheck[i].Equals("bikeshop"))
            {
                wordsToCheck[i] = "bike shop";
            }

            if (wordsToCheck[i].Equals("toystore"))
            {
                wordsToCheck[i] = "toy store";
            }

            if (wordsToCheck[i].Equals("petstore"))
            {
                wordsToCheck[i] = "pet store";
            }

            if (wordsToCheck[i].Equals("hardwarestore"))
            {
                wordsToCheck[i] = "hardware store";
            }

            if (wordsToCheck[i].Equals("super market"))
            {
                wordsToCheck[i] = "supermarket";
            }
        }

        foreach (string word in wordsToCheck)
        {
            if ((!storesSeen.Contains(word)) && (storeNames.Contains(word) || storeNames.Any(word1 => word1.Contains(word) && word.Length > word1.Length * 0.3)))
            {
                CountBuildings.score += 1;
                recallScore += 1; 
                storesSeen.Add(word);
            }

        }
        rewardCounter.text = CountBuildings.score.ToString();
    }

    public void LogScore(int freeRecallScore, int navigationScore, int totalScore)
    {
        StartCoroutine(SendScoreCoroutine(freeRecallScore, navigationScore, totalScore));
    }

    private IEnumerator SendScoreCoroutine(int freeRecallScore, int navigationScore, int totalScore)
    {
        ScoreRecord data = new ScoreRecord(freeRecallScore, navigationScore, totalScore);

        string jsonData = JsonUtility.ToJson(data);
        Debug.Log("JSON data prepared: " + jsonData);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(apiUrlScore, "POST"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Score data sent successfully to RecordScore table");
            }
        }
    }
}
