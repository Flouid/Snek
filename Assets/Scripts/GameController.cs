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

    [SerializeField] private Snake snakePrefab;

    private GridManager grid;
    private Snake snake;

    public void Awake()
    {
        Application.targetFrameRate = fpsTarget;
        grid = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();
    }

    public int GetLevelX()
    {
        return levelX;
    }

    public int GetLevelY()
    {
        return levelY;
    }

    void Start()
    {
        var snakeObject = Instantiate(snakePrefab);
        snakeObject.name = "Snake";
        snake = snakeObject.GetComponent<Snake>();
        snake.Init(new GridPosition(startX, startY, startDir), startLength);
    }
}
