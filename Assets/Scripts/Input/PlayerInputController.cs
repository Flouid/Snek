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
    public Vector2 lastFrame;

    private GameController _game;

    void Awake()
    {
        _game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void Start()
    {
        queuedInput = Vector2.zero;
    }

    void Update()
    {
        lastFrame = queuedInput;
        // if the game hasn't started and the player queues an input, start the game
        if (!_game.isPlaying() && queuedInput != Vector2.zero) _game.StartGame();

        if (Input.GetKeyDown(northKey) && queuedInput != Vector2.down) queuedInput = Vector2.up;
        if (Input.GetKeyDown(westKey) && queuedInput != Vector2.right) queuedInput = Vector2.left;
        if (Input.GetKeyDown(southKey) && queuedInput != Vector2.up) queuedInput = Vector2.down;
        if (Input.GetKeyDown(eastKey) && queuedInput != Vector2.left) queuedInput = Vector2.right;

        if (lastFrame != queuedInput) Debug.Log("registered input: " + queuedInput);
    }
}
