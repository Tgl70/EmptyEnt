using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static float TOTAL_TIME = 240.0f;

    static float timeLeft; // static binds the variable to the script (and not the object) so i can keep track of the value

    static bool isRunning = false;

    public TextMeshProUGUI timeLeftText;
    public GameObject loosePanel;
    public TextMeshProUGUI loosingText;
    public Button exitButton;

    public void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            timeLeft = TOTAL_TIME;
        }

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            isRunning = true;
        }

        loosePanel.gameObject.SetActive(false);
        loosingText.enabled = false;
        exitButton.onClick.AddListener(Application.Quit);
        exitButton.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isRunning)
        {
            timeLeft -= Time.deltaTime;
        }

        timeLeftText.text = string.Format("{0:D2}:{1:D2}", Mathf.FloorToInt(timeLeft / 60), Mathf.FloorToInt(timeLeft % 60));

        if (isRunning && timeLeft < 0.0f)
        {
            timeLeft = 0.0f;
            isRunning = false;
            GenerateLoosingScreen();
        }

        if (timeLeft == 0.0f && Input.GetKey(KeyCode.Return)) {
            Application.Quit();
        }
    }

    private void GenerateLoosingScreen()
    {
        Debug.Log("Strarting generation of loosing screen");
    
        
        loosingText.SetText("Time run out, you lost!\n Press Enter to exit");
        loosingText.enabled = true;
        loosePanel.gameObject.SetActive(true);
        exitButton.enabled = true;
    }
}
