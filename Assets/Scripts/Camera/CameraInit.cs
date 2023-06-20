using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInit : MonoBehaviour
{
    [SerializeField] private float padding = 0.5f;

    private GameController _game;
    private Camera _cam;
    private int _screenX, _screenY;

    void Awake()
    {
        _game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        _screenX = _game.GetLevelX();
        _screenY = _game.GetLevelY();
        _cam = GetComponent<Camera>();
    }

    void Start()
    {
        _cam.transform.position = new Vector3((float)_screenX / 2 - 0.5f, (float)_screenY /2 - 0.5f, -10);
        _cam.orthographicSize = (float)_screenY / 2 + padding;
    }
}
