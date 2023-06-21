using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private int levelX, levelY, startLength, fpsTarget;
    [SerializeField] private Vector2 startPos, startDir, foodPos;
    [SerializeField] private float snakeSpeed;

    [SerializeField] private Snake snakePrefab;

    private GridManager grid;
    private Snake snake;

    private bool gameStarted, gamePaused;

    public int GetLevelX() { return levelX; }
    public int GetLevelY() { return levelY; }
    public bool isStarted() { return gameStarted; }
    public bool isPaused() { return gamePaused; }

    public void PauseGame() 
    {
        Debug.Log("pausing game");
        Time.timeScale = 0.0f; 
        gamePaused = true;
    }

    public void UnpauseGame() 
    {
        Debug.Log("unpausing game");
        Time.timeScale = 1.0f;
        gamePaused = false;
    }

    public void StartGame() 
    { 
        Time.timeScale = 1.0f;
        snake.Grow();
        snake.SampleInput();
        gameStarted = true;
    }

    void Awake()
    {
        Application.targetFrameRate = fpsTarget;
        grid = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();
    }

    void Start()
    {
        var snakeObject = Instantiate(snakePrefab);
        snakeObject.name = "Snake";
        snake = snakeObject.GetComponent<Snake>();
        snake.Init(startPos, startDir, startLength, snakeSpeed);
        Time.timeScale = 0.0f;
        gameStarted = false;
        gamePaused = false;
    }
}
