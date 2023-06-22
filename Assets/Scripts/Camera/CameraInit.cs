using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInit : MonoBehaviour
{
    [SerializeField] private float padding = 0.5f;

    private GameController _game;
    private Camera _cam;
    private Vector2 _levelSize;

    void Awake()
    {
        _game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        _levelSize = _game.GetLevelSize();
        _cam = GetComponent<Camera>();
    }

    void Start()
    {
        _cam.transform.position = new Vector3(_levelSize.x / 2 - 0.5f, _levelSize.y /2 - 0.5f, -10);
        _cam.orthographicSize = _levelSize.y / 2 + padding;
    }
}
