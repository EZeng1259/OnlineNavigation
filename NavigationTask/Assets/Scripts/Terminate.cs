using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class Terminate : MonoBehaviour
{
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(terminate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void terminate()
    {
        Application.Quit(); 
    }
}
