using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MazeConstructor))]

public class GameController : MonoBehaviour
{
    public CharacterController controller;
    private MazeConstructor generator;
    public Canvas winningCanvas;
    public Button winningButton;
    private bool won = false;

    void Start()
    {
        generator = GetComponent<MazeConstructor>();
        StartNewGame();
        winningButton.onClick.AddListener(Application.Quit);

    }

    private void FixedUpdate()
    {
        if (won && Input.GetKey(KeyCode.Return))
        {
            Application.Quit();
        }
    }

    private void StartNewGame()
    {
        StartNewMaze();
    }

    private void StartNewMaze()
    {
        generator.GenerateNewMaze(21, 21, OnStartTrigger, OnGoalTrigger);

        winningCanvas.enabled = false;

        float x = generator.startCol * generator.hallWidth;
        float y = 1;
        float z = generator.startRow * generator.hallWidth;
        Vector3 spawnPosition = new Vector3(x, y, z);
        controller.Move(spawnPosition - transform.position);
    }

    void Update()
    {

    }

    private void OnGoalTrigger(GameObject trigger, GameObject other)
    {
        winningCanvas.enabled = true;
        won = true;
        Destroy(trigger);
    }

    private void OnStartTrigger(GameObject trigger, GameObject other)
    {
   
    }
}
