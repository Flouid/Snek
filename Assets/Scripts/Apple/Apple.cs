using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public int maxIterations;

    private GameController _game;
    private Transform _trans;

    public void Init(Vector2 pos)
    {
        Move(pos);
    }

    void Respawn()
    {
        Vector2 levelSize = _game.GetLevelSize();
        Vector2 pos = FindRandomPos(levelSize);
        Move(pos);
    }

    private Vector2 FindRandomPos(Vector2 bounds)
    {
        Vector2 pos;
        for (int i = 0; i < maxIterations; ++i)
        {
            int x = UnityEngine.Random.Range(0, (int)bounds.x);
            int y = UnityEngine.Random.Range(0, (int)bounds.y);
            pos = new Vector2(x, y);
            if (!_game.DoesSnakeOccupy(pos)) return pos;
        }
        Debug.Log("failed to find a new position for the apple, using origin");
        return Vector2.zero;
    }

    void Move(Vector2 pos)
    {
        _trans.position = new Vector3(pos.x, pos.y, 0.0f);   
    }

    void Awake()
    {
        _game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        _trans = GetComponent<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log("player collected food");
            _game.CollectFood();
            Respawn();
        }
    }
}
