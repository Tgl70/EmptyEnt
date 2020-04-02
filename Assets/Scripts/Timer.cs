using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    static float TOTAL_TIME = 180.0f;

    static float timeLeft; // static binds the variable to the script (and not the object) so i can keep track of the value

    static bool isRunning = false;

    public TextMeshProUGUI timeLeftText;

    public void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            timeLeft = TOTAL_TIME;
        }

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            isRunning = true;
        }
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
    }

    private void GenerateLoosingScreen()
    {
        GameObject mCanvas = GameObject.Find("Loose");

        TextMeshProUGUI loosingTextBox = new TextMeshProUGUI();
        loosingTextBox.SetText("Time run out, you lost!");
        loosingTextBox.transform.position = new Vector3(0, 50, 0);
        loosingTextBox.transform.SetParent(mCanvas.transform);

        GameObject exitButton = new GameObject();
        exitButton.AddComponent<CanvasRenderer>();
        exitButton.AddComponent<RectTransform>();
        TextMeshProUGUI buttonText = exitButton.AddComponent<TextMeshProUGUI>();
        buttonText.SetText("Exit Game");
        Button mButton = exitButton.AddComponent<Button>();
        mButton.onClick.AddListener(Application.Quit);
        exitButton.transform.position = new Vector3(0, 0, 0);
        exitButton.transform.SetParent(mCanvas.transform);
    }
}
