using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public static string northKey = "w";
    public static string westKey = "a";
    public static string southKey = "s";
    public static string eastKey = "d";

    public Direction queuedInput;
    public Direction lastFrame;

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
        lastFrame = queuedInput;
        // if the game hasn't started and the player queues an input, start the game
        if (!_game.isPlaying() && queuedInput != Direction.None) _game.StartGame();

        if (Input.GetKeyDown(northKey) && queuedInput != Direction.South) queuedInput = Direction.North;
        if (Input.GetKeyDown(westKey) && queuedInput != Direction.East) queuedInput = Direction.West;
        if (Input.GetKeyDown(southKey) && queuedInput != Direction.North) queuedInput = Direction.South;
        if (Input.GetKeyDown(eastKey) && queuedInput != Direction.West) queuedInput = Direction.East;

        if (lastFrame != queuedInput) Debug.Log("registered input: " + queuedInput);
    }
}
