using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public float minRespawnDistance;

    private GameController _game;
    private Transform _trans;

    public void Init(Vector2 pos)
    {
        Move(pos);
    }

    void Respawn()
    {
        Vector2 levelSize = _game.GetLevelSize();
        Vector2 oldPos = new Vector2(_trans.position.x, _trans.position.y);
        Vector2 pos;

        // look for positions to spawn the apple until a valid one is found
        while (true)
        {
            int x = UnityEngine.Random.Range(0, (int)levelSize.x);
            int y = UnityEngine.Random.Range(0, (int)levelSize.y);
            pos = new Vector2(x, y);
            if (!_game.DoesSnakeOccupy(pos) && Vector2.Distance(oldPos, pos) > minRespawnDistance) break;
        }

        Debug.Log("respawning apple at " + pos);

        Move(pos);
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
