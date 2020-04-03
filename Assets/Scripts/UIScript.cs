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
        DontDestroyOnLoad(transform.gameObject);
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            Destroy(introductoryText);
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
