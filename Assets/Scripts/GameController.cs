using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private int levelX, levelY;
    [SerializeField] private int startX, startY, foodX, foodY;
    [SerializeField] private int startLength;
    [SerializeField] private Direction startDir;
    [SerializeField] private int fpsTarget;
    [SerializeField] private float tilesPerSecond;
    [SerializeField] private bool started;

    [SerializeField] private Snake snakePrefab;

    private GridManager grid;
    private Snake snake;
    private float clock;                                        // move clock
    private float moveProgress;                                 // (0, 1) - progress until next move step

    public int GetLevelX() { return levelX; }
    public int GetLevelY() { return levelY; }

    void Awake()
    {
        Application.targetFrameRate = fpsTarget;
        grid = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();
        started = false;
        moveProgress = 0.0f;
    }

    void Start()
    {
        var snakeObject = Instantiate(snakePrefab);
        snakeObject.name = "Snake";
        snake = snakeObject.GetComponent<Snake>();
        snake.Init(new GridPosition(startX, startY, startDir), startLength);

        clock = 0.0f;
    }
}
