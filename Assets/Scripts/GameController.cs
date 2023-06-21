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

    public int GetLevelX() { return levelX; }
    public int GetLevelY() { return levelY; }
    public bool isPlaying() { return Time.timeScale > 0; }
    public void PauseGame() { Time.timeScale = 0.0f; }

    public void StartGame() 
    { 
        Time.timeScale = 1.0f; 
        snake.Grow();
        snake.SampleInput(); 
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
        PauseGame();
    }
}
