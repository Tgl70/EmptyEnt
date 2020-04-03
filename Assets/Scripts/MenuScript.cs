using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public Button playButton;

    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(Play);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Play()
    {
        SceneManager.LoadScene(1);
    }
}
