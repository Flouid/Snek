using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private int startLength, fpsTarget;
    [SerializeField] private Vector2 levelSize, startPos, startDir, foodPos;
    [SerializeField] private float snakeSpeed;

    [SerializeField] private Snake snakePrefab;
    [SerializeField] private Apple applePrefab;

    private GridManager grid;
    private Snake snake;
    private Apple apple;

    private int score;
    private bool gameStarted, gamePaused, gameEnded;

    public Vector2 GetLevelSize() { return levelSize; }
    public bool IsStarted() { return gameStarted; }
    public bool IsPaused() { return gamePaused; }
    public bool IsEnded() { return gameEnded; }
    public bool DoesSnakeOccupy(Vector2 pos) { return snake.IsOccupied(pos); }

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
        snake.SampleInput();
        gameStarted = true;
    }

    public void EndGame()
    {
        Time.timeScale = 0.0f;
        gameEnded = true;
    }

    public void CollectFood()
    {
        ++score;
        snake.Grow();
    }

    private void CreateSnake()
    {
        var snakeObject = Instantiate(snakePrefab);
        snakeObject.name = "Snake";
        snake = snakeObject.GetComponent<Snake>();
        snake.Init(startPos, startDir, startLength, snakeSpeed);
    }

    private void CreateApple()
    {
        var appleObject = Instantiate(applePrefab);
        appleObject.name = "Apple";
        apple = appleObject.GetComponent<Apple>();
        apple.Init(foodPos);
    }

    void Awake()
    {
        Application.targetFrameRate = fpsTarget;
        grid = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();
    }

    void Start()
    {
        CreateSnake();
        CreateApple();

        Time.timeScale = 0.0f;
        gameStarted = false;
        gamePaused = false;
        gameEnded = false;
        score = 0;
    }
}
