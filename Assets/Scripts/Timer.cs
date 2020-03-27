using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    static float TOTAL_TIME = 180.0f;

    float timeLeft;

    bool isRunning = false;

    public TextMeshProUGUI timeLeftText;

    private void Awake()
    {
        timeLeft = TOTAL_TIME;
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void Start()
    {
        StartTimer();
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
        }
    }
}
