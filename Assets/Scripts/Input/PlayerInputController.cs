using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public static string forwardKey = "w";
    public static string leftKey = "a";
    public static string downKey = "s";
    public static string rightKey = "d";

    public Direction queuedInput;

    private GameController _game;

    void Awake()
    {
        _game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void Start()
    {
        queuedInput = Direction.None;
    }

    void Update()
    {
        // if the game hasn't started and the player queues an input, start the game
        if (!_game.CheckStarted() && queuedInput != Direction.None) _game.StartGame();

        if (Input.GetKeyDown(forwardKey)) queuedInput = Direction.North;
        if (Input.GetKeyDown(leftKey)) queuedInput = Direction.West;
        if (Input.GetKeyDown(downKey)) queuedInput = Direction.South;
        if (Input.GetKeyDown(rightKey)) queuedInput = Direction.East;
    }
}
