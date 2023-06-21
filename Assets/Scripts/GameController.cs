using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private int levelX, levelY, startX, startY, foodX, foodY;
    [SerializeField] private int startLength;
    [SerializeField] private Direction startDir;
    [SerializeField] private int fpsTarget;
    [SerializeField] private float tilesPerSecond;
    [SerializeField] private bool hasStarted;

    [SerializeField] private Snake snakePrefab;

    private GridManager grid;
    private Snake snake;

    public int GetLevelX() { return levelX; }
    public int GetLevelY() { return levelY; }
    public bool CheckStarted() { return hasStarted; }
    public void StartGame() { hasStarted = true; }

    void Awake()
    {
        Application.targetFrameRate = fpsTarget;
        grid = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();
        hasStarted = false;
    }

    void Start()
    {
        var snakeObject = Instantiate(snakePrefab);
        snakeObject.name = "Snake";
        snake = snakeObject.GetComponent<Snake>();
        snake.Init(new GridPosition(startX, startY, startDir), startLength);
    }
}
