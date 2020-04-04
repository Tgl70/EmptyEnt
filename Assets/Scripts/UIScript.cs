using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    public TextMeshProUGUI introductoryText;
    private float countdown = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        switch (SceneManager.GetActiveScene().buildIndex){
            case 1: introductoryText.SetText("Find the key and proceed to the next room");
                break;
            case 2: introductoryText.SetText("A timer has started, clear all the rooms and find the treasure before the time runs out");
                break;
            case 3: introductoryText.SetText("Be warned! In this ice cavern you cannot climb back");
                break;
            case 4: introductoryText.SetText("To swing on the wines press E to grab it and Q to jump off");
                break;
            case 5: introductoryText.SetText("You reached the final challenge! Find the treasure");
                break;
            default: Destroy(introductoryText);
                break;
        }
            
        
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            Destroy(introductoryText);
        }
    }
}
