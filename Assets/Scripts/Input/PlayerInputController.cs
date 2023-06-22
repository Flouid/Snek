using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public static string northKey = "w";
    public static string westKey = "a";
    public static string southKey = "s";
    public static string eastKey = "d";

    public Vector2 queuedInput;

    private Snake _snake;
    private GameController _game;

    void Awake()
    {
        _snake = GetComponent<Snake>();
        _game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void Start()
    {
        queuedInput = Vector2.zero;
    }

    void Update()
    {
        if (_game.IsEnded()) return;
        
        HandleGameStart();
        HandleInput();
        HandlePause();
    }

    void HandleGameStart()
    {
        if (!_game.IsStarted() && queuedInput != Vector2.zero) _game.StartGame();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(northKey) && _snake.CurrentInput() != Vector2.down) queuedInput = Vector2.up;
        if (Input.GetKeyDown(westKey) && _snake.CurrentInput() != Vector2.right) queuedInput = Vector2.left;
        if (Input.GetKeyDown(southKey) && _snake.CurrentInput() != Vector2.up) queuedInput = Vector2.down;
        if (Input.GetKeyDown(eastKey) && _snake.CurrentInput() != Vector2.left) queuedInput = Vector2.right;
    }

    void HandlePause()
    {
        if (!_game.IsStarted()) return;                 // block pause/unpause until after game start 
        if (!Input.GetKeyDown(KeyCode.Space)) return;   // pause toggle is held

        if (_game.IsPaused()) _game.UnpauseGame();
        else _game.PauseGame();
    }
}
